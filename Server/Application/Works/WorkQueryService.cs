using Application.Authors;
using Application.AcademicYears;
using Application.Shared.Services;
using Application.SystemConfigs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Application.AuthorRegistrations;
using Domain.Enums;

namespace Application.Works
{
    public class WorkQueryService : IWorkQueryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper<WorkDto, Work> _mapper;
        private readonly IGenericMapper<AuthorDto, Author> _authorMapper;
        private readonly IWorkRepository _workRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAcademicYearService _academicYearService;
        private readonly IAuthorService _authorService;
        private readonly ISystemConfigService _systemConfigService;
        private readonly IDistributedCache _cache;
        private readonly ILogger<WorkQueryService> _logger;

        public WorkQueryService(
            IUnitOfWork unitOfWork,
            IGenericMapper<WorkDto, Work> mapper,
            IGenericMapper<AuthorDto, Author> authorMapper,
            IWorkRepository workRepository,
            ICurrentUserService currentUserService,
            IAcademicYearService academicYearService,
            IAuthorService authorService,
            ISystemConfigService systemConfigService,
            IDistributedCache cache,
            ILogger<WorkQueryService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authorMapper = authorMapper;
            _workRepository = workRepository;
            _currentUserService = currentUserService;
            _academicYearService = academicYearService;
            _authorService = authorService;
            _systemConfigService = systemConfigService;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<WorkDto>> GetAllWorksWithAuthorsAsync(CancellationToken cancellationToken = default)
        {
            var works = await _workRepository.GetWorksWithAuthorsAsync();
            var workDtos = _mapper.MapToDtos(works).ToList();

            await FillCoAuthorUserIdsForMultipleWorksAsync(workDtos, cancellationToken);

            return workDtos;
        }

        public async Task<WorkDto?> GetWorkByIdWithAuthorsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(id);
            if (work is null)
                return null;

            var dto = _mapper.MapToDto(work);
            await FillCoAuthorUserIdsAsync(dto, cancellationToken); // Điền CoAuthorUserIds
            return dto;
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var authors = await _unitOfWork.Repository<Author>()
                .FindAsync(a => a.UserId == userId);

            if (!authors.Any())
            {
                return Enumerable.Empty<WorkDto>();
            }

            var workIds = authors.Select(a => a.WorkId).Distinct();
            var works = await _workRepository.GetWorksWithAuthorsByIdsAsync(workIds, cancellationToken);

            var filteredWorks = works.Select(work =>
            {
                var userAuthor = authors.FirstOrDefault(a => a.WorkId == work.Id);
                if (userAuthor is null)
               {
                  return null;
               }
                
                    var filteredWork = _mapper.MapToDto(work);
                    filteredWork.Authors = new List<AuthorDto> { _authorMapper.MapToDto(userAuthor) };
                    return filteredWork;
            }).Where(w => w is not null).ToList();

            await FillCoAuthorUserIdsForMultipleWorksAsync(filteredWorks!, cancellationToken);

            return filteredWorks!;
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            var works = await _workRepository.GetWorksByDepartmentIdAsync(departmentId, cancellationToken);
            return _mapper.MapToDtos(works);
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                // Lấy năm học hiện tại
                var currentAcademicYear = await _academicYearService.GetCurrentAcademicYear(cancellationToken);
                if (currentAcademicYear == null)
                {
                    _logger.LogWarning("Không tìm thấy năm học hiện tại");
                    return Enumerable.Empty<WorkDto>();
                }

                _logger.LogInformation("Năm học hiện tại: {AcademicYearName} (ID: {AcademicYearId})", 
                    currentAcademicYear.Name, currentAcademicYear.Id);

                // 1. Lấy danh sách tác giả có thể đăng ký của người dùng
                var registrableAuthors = await _authorService.GetAllRegistableAuthorsOfUser(userId, cancellationToken);

                // 2. Lấy danh sách tác giả đã được đăng ký trong năm học hiện tại (qua bảng AuthorRegistration)
                var authorRegistrations = await _unitOfWork.Repository<AuthorRegistration>()
                    .Include(ar => ar.Author)
                    .Where(ar => ar.AcademicYearId == currentAcademicYear.Id && ar.Author.UserId == userId)
                    .ToListAsync(cancellationToken);
                
                var registeredAuthorIds = authorRegistrations.Select(ar => ar.AuthorId).ToList();
                
                _logger.LogInformation("Số lượng tác giả đã đăng ký quy đổi: {Count}", registeredAuthorIds.Count);
                
                // Lấy tất cả các tác giả của người dùng đã được đăng ký trong năm học hiện tại
                var registeredAuthors = await _unitOfWork.Repository<Author>()
                    .Include(a => a.Work)
                    .Include(a => a.AuthorRole)
                    .Include(a => a.Purpose)
                    .Include(a => a.SCImagoField)
                    .Include(a => a.Field)
                    .Include(a => a.AuthorRegistration)
                    .Where(a => registeredAuthorIds.Contains(a.Id) && a.UserId == userId)
                    .ToListAsync(cancellationToken);

                // 3. Lấy danh sách công trình mà người dùng được thêm làm đồng tác giả
                var coAuthorWorks = await _unitOfWork.Repository<WorkAuthor>()
                    .Include(wa => wa.Work)
                    .Where(wa => wa.UserId == userId)
                    .Select(wa => wa.Work)
                    .ToListAsync(cancellationToken);

                // Kết hợp hai danh sách tác giả
                var allAuthors = registrableAuthors.ToList();
                
                // Đảm bảo AuthorRegistration được gán cho các tác giả đã đăng ký
                foreach (var author in registeredAuthors)
                {
                    var authorDto = _authorMapper.MapToDto(author);
                    
                    // Tìm AuthorRegistration tương ứng
                    var registration = authorRegistrations.FirstOrDefault(ar => ar.AuthorId == author.Id);
                    if (registration != null)
                    {
                        // Gán thông tin AuthorRegistration
                        authorDto.AuthorRegistration = new AuthorRegistrationDto
                        {
                            Id = registration.Id,
                            AuthorId = registration.AuthorId,
                            AcademicYearId = registration.AcademicYearId
                        };
                        
                        _logger.LogInformation("Tác giả {AuthorId} đã được đăng ký với AuthorRegistration {RegistrationId}", 
                            author.Id, registration.Id);
                    }
                    
                    // Kiểm tra xem tác giả có tồn tại trong danh sách registrableAuthors chưa
                    var existingAuthor = allAuthors.FirstOrDefault(a => a.Id == author.Id);
                    if (existingAuthor != null)
                    {
                        // Cập nhật AuthorRegistration cho tác giả đã tồn tại
                        existingAuthor.AuthorRegistration = authorDto.AuthorRegistration;
                    }
                    else
                    {
                        // Thêm tác giả mới vào danh sách
                        allAuthors.Add(authorDto);
                    }
                }

                if (!allAuthors.Any() && !coAuthorWorks.Any())
                {
                    _logger.LogInformation("Không tìm thấy tác giả nào có thể đăng ký hoặc đã đăng ký cho userId={UserId}", userId);
                    return Enumerable.Empty<WorkDto>();
                }

                // Lấy danh sách workId từ tất cả tác giả và công trình đồng tác giả
                var authorWorkIds = allAuthors.Select(a => a.WorkId).Distinct().ToList();
                var coAuthorWorkIds = coAuthorWorks.Select(w => w.Id).Distinct().ToList();
                var allWorkIds = authorWorkIds.Union(coAuthorWorkIds).Distinct().ToList();

                // Lấy thông tin đầy đủ của các công trình
                var works = await _workRepository.GetWorksWithAuthorsByIdsAsync(allWorkIds, cancellationToken);
                
                // Tạo WorkDto từ mỗi công trình
                var workDtos = works.Select(work => 
                {
                    var dto = _mapper.MapToDto(work);
                    
                    // Lọc chỉ giữ lại thông tin tác giả của người dùng hiện tại
                    var authorDto = allAuthors.FirstOrDefault(a => a.WorkId == work.Id);
                    if (authorDto != null)
                    {
                        dto.Authors = new List<AuthorDto> { authorDto };
                    }
                    else
                    {
                        dto.Authors = new List<AuthorDto>();
                    }
                    
                    return dto;
                }).ToList();
            
                // Fill coAuthorUserIds cho tất cả các công trình
                await FillCoAuthorUserIdsForMultipleWorksAsync(workDtos, cancellationToken);
            
                return workDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy công trình của người dùng {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<WorkDto>> GetAllWorksByCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            // Lấy danh sách các WorkId mà user là tác giả (có trong bảng Author)
            var authors = await _unitOfWork.Repository<Author>()
                .FindAsync(a => a.UserId == userId);

            var authorWorkIds = authors.Select(a => a.WorkId).Distinct().ToList();

            // Lấy danh sách các WorkId mà user được thêm làm đồng tác giả (có trong bảng WorkAuthor)
            var workAuthors = await _unitOfWork.Repository<WorkAuthor>()
                .FindAsync(wa => wa.UserId == userId);

            var workAuthorWorkIds = workAuthors.Select(wa => wa.WorkId).Distinct().ToList();

            // Kết hợp tất cả WorkId (loại bỏ trùng lặp)
            var allWorkIds = authorWorkIds.Union(workAuthorWorkIds).Distinct().ToList();

            if (!allWorkIds.Any())
            {
                return Enumerable.Empty<WorkDto>();
            }

            // Lấy thông tin đầy đủ của các công trình
            var works = await _workRepository.GetWorksWithAuthorsByIdsAsync(allWorkIds, cancellationToken);

            // Lọc và xử lý dữ liệu
            var filteredWorks = works.Select(work =>
            {
                // Kiểm tra xem user đã có thông tin tác giả trong công trình này chưa
                var userAuthor = authors.FirstOrDefault(a => a.WorkId == work.Id);
                var isCoAuthor = workAuthorWorkIds.Contains(work.Id);

                // Chỉ giữ thông tin tác giả của user hiện tại (nếu có)
                var filteredWork = _mapper.MapToDto(work);
                filteredWork.Authors = userAuthor != null
                    ? new List<AuthorDto> { _authorMapper.MapToDto(userAuthor) }
                    : new List<AuthorDto>();

                return filteredWork;
            }).Where(w => w != null).ToList();

            await FillCoAuthorUserIdsForMultipleWorksAsync(filteredWorks, cancellationToken);

            return filteredWorks;
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByAcademicYearIdAsync(Guid academicYearId, CancellationToken cancellationToken = default)
        {
            var works = await _workRepository.GetWorksByAcademicYearIdAsync(academicYearId, cancellationToken);
            var workDtos = _mapper.MapToDtos(works);
            await FillCoAuthorUserIdsForMultipleWorksAsync(workDtos, cancellationToken);
            return workDtos;
        }

        public async Task<IEnumerable<WorkDto>> GetMyWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default)
        {
            // Kiểm tra nếu đây là năm học hiện tại
            var currentAcademicYear = await _academicYearService.GetCurrentAcademicYear(cancellationToken);
            var isCurrentYear = currentAcademicYear.Id == academicYearId;
            
            // 1. Lấy các công trình mà người dùng đã kê khai trong năm học này
            var declaredWorks = await _workRepository.GetDeclaredWorksByAcademicYearIdAsync(userId, academicYearId, cancellationToken);
            
            // 2. Lấy các công trình mà người dùng được người khác thêm vào danh sách đồng tác giả trong năm học này
            var coAuthorWorks = await _workRepository.GetCoAuthorWorksByAcademicYearIdAsync(userId, academicYearId, cancellationToken);
            
            // 3. Nếu đây là năm học hiện tại, thêm cả các công trình từ các năm học trước mà chưa được đăng ký
            var previousWorks = new List<Work>();
            
            if (isCurrentYear)
            {
                // Lấy các công trình chưa đánh dấu quy đổi từ các đợt trước
                previousWorks = (await _workRepository.GetUnmarkedPreviousWorksAsync(userId, academicYearId, cancellationToken)).ToList();
            }
            
            // Kết hợp tất cả công trình, loại bỏ trùng lặp
            var allWorks = declaredWorks
                .Union(coAuthorWorks)
                .Union(previousWorks)
                .DistinctBy(w => w.Id)
                .ToList();
            
            // Chuyển đổi sang DTO và cập nhật thông tin CoAuthorUserIds
            var workDtos = _mapper.MapToDtos(allWorks);
            await FillCoAuthorUserIdsForMultipleWorksAsync(workDtos, cancellationToken);
            
            return workDtos;
        }

        public async Task<IEnumerable<WorkDto>> GetAllMyWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("GetAllMyWorksByAcademicYearIdAsync - Lấy tất cả công trình của người dùng {UserId} theo năm học {AcademicYearId}", userId, academicYearId);
                
                // 1. Lấy các công trình mà user là tác giả trong năm học này (bao gồm cả import và do người dùng kê khai)
                var authorWorks = await _workRepository.GetAuthorWorksByAcademicYearIdAsync(userId, academicYearId, cancellationToken);
                _logger.LogInformation("GetAllMyWorksByAcademicYearIdAsync - Đã lấy được {Count} công trình mà user là tác giả", authorWorks.Count());
                
                // 2. Lấy các công trình mà user là đồng tác giả trong năm học này
                var coAuthorWorks = await _workRepository.GetCoAuthorWorksByAcademicYearIdAsync(userId, academicYearId, cancellationToken);
                _logger.LogInformation("GetAllMyWorksByAcademicYearIdAsync - Đã lấy được {Count} công trình mà user là đồng tác giả", coAuthorWorks.Count());
                
                // 3. Kết hợp tất cả công trình, loại bỏ trùng lặp
                var allWorks = authorWorks
                    .Union(coAuthorWorks)
                    .DistinctBy(w => w.Id)
                    .ToList();
                
                _logger.LogInformation("GetAllMyWorksByAcademicYearIdAsync - Tổng cộng {Count} công trình", allWorks.Count);
                
                // 4. Chuyển đổi sang DTO và cập nhật thông tin CoAuthorUserIds
                var workDtos = _mapper.MapToDtos(allWorks);
                await FillCoAuthorUserIdsForMultipleWorksAsync(workDtos, cancellationToken);
                
                return workDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tất cả công trình của người dùng {UserId} theo năm học {AcademicYearId}", userId, academicYearId);
                throw;
            }
        }

        //public async Task<IEnumerable<WorkDto>> GetMyRegisterableWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        _logger.LogInformation("GetMyRegisterableWorksByAcademicYearIdAsync - Lấy công trình có thể đăng ký quy đổi của người dùng {UserId} theo năm học {AcademicYearId}", userId, academicYearId);
                
        //        // Lấy các công trình có thể đăng ký quy đổi
        //        var currentDate = DateTime.UtcNow;
        //        var works = await _workRepository.GetRegisterableWorksByAcademicYearIdAsync(userId, academicYearId, currentDate, cancellationToken);
                
        //        _logger.LogInformation("GetMyRegisterableWorksByAcademicYearIdAsync - Tìm thấy {Count} công trình có thể đăng ký quy đổi", works.Count());
                
        //        // Chuyển đổi sang DTO và cập nhật thông tin CoAuthorUserIds
        //        var workDtos = _mapper.MapToDtos(works);
        //        await FillCoAuthorUserIdsForMultipleWorksAsync(workDtos, cancellationToken);
                
        //        return workDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Lỗi khi lấy công trình có thể đăng ký quy đổi của người dùng {UserId} theo năm học {AcademicYearId}", userId, academicYearId);
        //        throw;
        //    }
        //}

        public async Task<IEnumerable<WorkDto>> GetMyRegisteredWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("GetMyRegisteredWorksByAcademicYearIdAsync - Lấy công trình đã đăng ký quy đổi của người dùng {UserId} theo năm học {AcademicYearId}", userId, academicYearId);
                
                // Lấy các công trình đã đăng ký quy đổi
                var works = await _workRepository.GetRegisteredWorksByAcademicYearIdAsync(userId, academicYearId, cancellationToken);
                
                _logger.LogInformation("GetMyRegisteredWorksByAcademicYearIdAsync - Tìm thấy {Count} công trình đã đăng ký quy đổi", works.Count());
                
                // Chuyển đổi sang DTO và cập nhật thông tin CoAuthorUserIds
                var workDtos = _mapper.MapToDtos(works);
                await FillCoAuthorUserIdsForMultipleWorksAsync(workDtos, cancellationToken);
                
                return workDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy công trình đã đăng ký quy đổi của người dùng {UserId} theo năm học {AcademicYearId}", userId, academicYearId);
                throw;
            }
        }

        public async Task<IEnumerable<WorkDto>> GetMyWorksByAcademicYearAndProofStatusAsync(Guid userId, Guid academicYearId, ProofStatus proofStatus, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("GetMyWorksByAcademicYearAndProofStatusAsync - Lấy công trình của người dùng {UserId} theo năm học {AcademicYearId} và trạng thái {ProofStatus}", userId, academicYearId, proofStatus);
                
                // Lấy các công trình theo năm học và trạng thái
                var works = await _workRepository.GetWorksByAcademicYearAndProofStatusAsync(userId, academicYearId, proofStatus, cancellationToken);
                
                _logger.LogInformation("GetMyWorksByAcademicYearAndProofStatusAsync - Tìm thấy {Count} công trình", works.Count());
                
                // Chuyển đổi sang DTO và cập nhật thông tin CoAuthorUserIds
                var workDtos = _mapper.MapToDtos(works);
                await FillCoAuthorUserIdsForMultipleWorksAsync(workDtos, cancellationToken);
                
                return workDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy công trình của người dùng {UserId} theo năm học {AcademicYearId} và trạng thái {ProofStatus}", userId, academicYearId, proofStatus);
                throw;
            }
        }

        public async Task<IEnumerable<WorkDto>> GetMyWorksByAcademicYearAndSourceAsync(Guid userId, Guid academicYearId, WorkSource source, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("GetMyWorksByAcademicYearAndSourceAsync - Lấy công trình của người dùng {UserId} theo năm học {AcademicYearId} và nguồn {Source}", userId, academicYearId, source);
                
                // Lấy các công trình theo năm học và nguồn
                var works = await _workRepository.GetWorksByAcademicYearAndSourceAsync(userId, academicYearId, source, cancellationToken);
                
                _logger.LogInformation("GetMyWorksByAcademicYearAndSourceAsync - Tìm thấy {Count} công trình", works.Count());
                
                // Chuyển đổi sang DTO và cập nhật thông tin CoAuthorUserIds
                var workDtos = _mapper.MapToDtos(works);
                await FillCoAuthorUserIdsForMultipleWorksAsync(workDtos, cancellationToken);
                
                return workDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy công trình của người dùng {UserId} theo năm học {AcademicYearId} và nguồn {Source}", userId, academicYearId, source);
                throw;
            }
        }

        public async Task<IEnumerable<WorkDto>> GetMyCoAuthorWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("GetMyCoAuthorWorksByAcademicYearIdAsync - Lấy công trình mà người dùng {UserId} là đồng tác giả theo năm học {AcademicYearId}", userId, academicYearId);
                
                // Lấy các công trình mà user là đồng tác giả
                var works = await _workRepository.GetCoAuthorWorksByAcademicYearIdAsync(userId, academicYearId, cancellationToken);
                
                _logger.LogInformation("GetMyCoAuthorWorksByAcademicYearIdAsync - Tìm thấy {Count} công trình", works.Count());
                
                // Chuyển đổi sang DTO và cập nhật thông tin CoAuthorUserIds
                var workDtos = _mapper.MapToDtos(works);
                await FillCoAuthorUserIdsForMultipleWorksAsync(workDtos, cancellationToken);
                
                return workDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy công trình mà người dùng {UserId} là đồng tác giả theo năm học {AcademicYearId}", userId, academicYearId);
                throw;
            }
        }

        public async Task<IEnumerable<WorkDto>> GetUserWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("GetUserWorksByAcademicYearIdAsync - Admin lấy công trình của người dùng {UserId} theo năm học {AcademicYearId}", userId, academicYearId);
                
                // Lấy các công trình của một user cụ thể (dành cho admin)
                var works = await _workRepository.GetUserWorksByAcademicYearIdAsync(userId, academicYearId, cancellationToken);
                
                _logger.LogInformation("GetUserWorksByAcademicYearIdAsync - Tìm thấy {Count} công trình", works.Count());
                
                // Chuyển đổi sang DTO và cập nhật thông tin CoAuthorUserIds
                var workDtos = _mapper.MapToDtos(works);
                await FillCoAuthorUserIdsForMultipleWorksAsync(workDtos, cancellationToken);
                
                return workDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi admin lấy công trình của người dùng {UserId} theo năm học {AcademicYearId}", userId, academicYearId);
                throw;
            }
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByDepartmentAndAcademicYearIdAsync(Guid departmentId, Guid academicYearId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("GetWorksByDepartmentAndAcademicYearIdAsync - Lấy công trình theo phòng ban {DepartmentId} và năm học {AcademicYearId}", departmentId, academicYearId);
                
                // Lấy các công trình theo phòng ban và năm học
                var works = await _workRepository.GetWorksByDepartmentAndAcademicYearIdAsync(departmentId, academicYearId, cancellationToken);
                
                _logger.LogInformation("GetWorksByDepartmentAndAcademicYearIdAsync - Tìm thấy {Count} công trình", works.Count());
                
                // Chuyển đổi sang DTO và cập nhật thông tin CoAuthorUserIds
                var workDtos = _mapper.MapToDtos(works);
                await FillCoAuthorUserIdsForMultipleWorksAsync(workDtos, cancellationToken);
                
                return workDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy công trình theo phòng ban {DepartmentId} và năm học {AcademicYearId}", departmentId, academicYearId);
                throw;
            }
        }

        private async Task FillCoAuthorUserIdsAsync(WorkDto workDto, CancellationToken cancellationToken = default)
        {
            if (workDto is null)
                return;

            // Lấy userId từ service
            var (isSuccess, currentUserId, _) = _currentUserService.GetCurrentUser();
            
            _logger.LogInformation("FillCoAuthorUserIdsAsync - WorkDto {WorkId}: Start", workDto.Id);

            try {
                // Lấy những UserId đã có thông tin Author
                var authors = await _unitOfWork.Repository<Author>()
                    .FindAsync(a => a.WorkId == workDto.Id);
                var authorUserIds = authors.Select(a => a.UserId).ToList();

                // Lấy những UserId trong bảng WorkAuthor
                var workAuthors = await _unitOfWork.Repository<WorkAuthor>()
                    .FindAsync(wa => wa.WorkId == workDto.Id);
                var coAuthorUserIds = workAuthors.Select(wa => wa.UserId).ToList();

                _logger.LogInformation("FillCoAuthorUserIdsAsync - Authors found: {Count}", authors.Count());
                _logger.LogInformation("FillCoAuthorUserIdsAsync - WorkAuthors found: {Count}", workAuthors.Count());
                _logger.LogInformation("Authors UserIds: {AuthorIds}", string.Join(", ", authorUserIds));
                _logger.LogInformation("WorkAuthors UserIds: {WorkAuthors}", string.Join(", ", coAuthorUserIds));

                // Kết hợp tất cả UserId, loại bỏ currentUserId (nếu có), và đảm bảo không có trùng lặp
                var combinedUserIds = authorUserIds.Union(coAuthorUserIds);
                if (isSuccess)
                {
                    combinedUserIds = combinedUserIds.Where(uid => uid != currentUserId);
                }
                workDto.CoAuthorUserIds = combinedUserIds.Distinct().ToList();
                
                _logger.LogInformation("Final CoAuthorUserIds: {Count} users - {FinalIds}", 
                    workDto.CoAuthorUserIds.Count,
                    string.Join(", ", workDto.CoAuthorUserIds));
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error filling CoAuthorUserIds for WorkDto {WorkId}", workDto.Id);
                // Đảm bảo ít nhất trả về một danh sách rỗng thay vì null
                workDto.CoAuthorUserIds = new List<Guid>();
            }
        }

        private async Task FillCoAuthorUserIdsForMultipleWorksAsync(IEnumerable<WorkDto> workDtos, CancellationToken cancellationToken = default)
        {
            if (workDtos is null || !workDtos.Any())
                return;

            // Lấy userId từ service
            var (isSuccess, currentUserId, _) = _currentUserService.GetCurrentUser();

            // Lấy tất cả workIds
            var workIds = workDtos.Select(w => w.Id).Distinct().ToList();

            // Thực hiện 2 truy vấn cho tất cả công trình cùng lúc
            var allAuthors = await _unitOfWork.Repository<Author>()
                .FindAsync(a => workIds.Contains(a.WorkId));

            var allWorkAuthors = await _unitOfWork.Repository<WorkAuthor>()
                .FindAsync(wa => workIds.Contains(wa.WorkId));

            // Nhóm dữ liệu theo WorkId để tránh lọc lặp đi lặp lại
            var authorsByWorkId = allAuthors.GroupBy(a => a.WorkId)
                .ToDictionary(g => g.Key, g => g.Select(a => a.UserId).ToList());

            var workAuthorsByWorkId = allWorkAuthors.GroupBy(wa => wa.WorkId)
                .ToDictionary(g => g.Key, g => g.Select(wa => wa.UserId).ToList());

            // Điền dữ liệu cho từng WorkDto
            foreach (var workDto in workDtos)
            {
                // Lấy danh sách userIds của authors (nếu có)
                var authorUserIds = authorsByWorkId.TryGetValue(workDto.Id, out var authors) 
                    ? authors 
                    : new List<Guid>();

                // Lấy danh sách userIds của work authors (nếu có)
                var coAuthorUserIds = workAuthorsByWorkId.TryGetValue(workDto.Id, out var workAuthors) 
                    ? workAuthors 
                    : new List<Guid>();

                // Kết hợp, loại bỏ trùng lặp và loại bỏ userId của người dùng đang đăng nhập
                var combinedUserIds = authorUserIds.Union(coAuthorUserIds);
                if (isSuccess)
                {
                    combinedUserIds = combinedUserIds.Where(uid => uid != currentUserId);
                }
                workDto.CoAuthorUserIds = combinedUserIds.Distinct().ToList();
            }
        }
    }
} 