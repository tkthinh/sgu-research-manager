using Application.Authors;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Domain.Enums;
using Application.SystemConfigs;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Globalization;
using System.Data;
using System.Linq.Expressions;
using Application.Shared.Messages;
using Application.AcademicYears;
using Microsoft.EntityFrameworkCore;

namespace Application.Works
{
    public class WorkService : GenericCachedService<WorkDto, Work>, IWorkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper<WorkDto, Work> _mapper;
        private readonly IGenericMapper<AuthorDto, Author> _authorMapper;
        private readonly IGenericRepository<AuthorRole> _authorRoleRepository;
        private readonly IGenericRepository<Factor> _factorRepository;
        private readonly IWorkRepository _workRepository;
        private readonly ISystemConfigService _systemConfigService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<WorkService> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAcademicYearService _academicYearService;

        public WorkService(
            IUnitOfWork unitOfWork,
            IGenericMapper<WorkDto, Work> mapper,
            IGenericMapper<AuthorDto, Author> authorMapper,
            IDistributedCache cache,
            ILogger<WorkService> logger,
            IWorkRepository workRepository,
            ISystemConfigService systemConfigService,
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            ICurrentUserService currentUserService,
            IAcademicYearService academicYearService)
            : base(unitOfWork, mapper, cache, logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authorMapper = authorMapper;
            _authorRoleRepository = unitOfWork.Repository<AuthorRole>();
            _factorRepository = unitOfWork.Repository<Factor>();
            _workRepository = workRepository;
            _systemConfigService = systemConfigService;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _logger = logger;
            _currentUserService = currentUserService;
            _academicYearService = academicYearService;
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

        public async Task<WorkDto> CreateWorkWithAuthorAsync(CreateWorkRequestDto request, CancellationToken cancellationToken = default)
        {
            // Kiểm tra trạng thái hệ thống
            if (!await _systemConfigService.IsSystemOpenAsync(DateTime.Now, cancellationToken))
                throw new Exception(ErrorMessages.SystemClosedNewWork);

            var existingWork = await _unitOfWork.Repository<Work>()
                .FirstOrDefaultAsync(w => w.Title == request.Title && 
                    w.WorkTypeId == request.WorkTypeId && 
                    w.WorkLevelId == request.WorkLevelId, cancellationToken);
            if (existingWork != null)
                throw new Exception(ErrorMessages.WorkAlreadyExists);

            var work = new Work
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                TimePublished = request.TimePublished,
                TotalAuthors = request.TotalAuthors,
                TotalMainAuthors = request.TotalMainAuthors,
                Details = request.Details,
                Source = WorkSource.NguoiDungKeKhai,
                WorkTypeId = request.WorkTypeId,
                WorkLevelId = request.WorkLevelId,
                ExchangeDeadline = request.TimePublished.HasValue ? request.TimePublished.Value.AddMonths(18) : null,
                CreatedDate = DateTime.UtcNow,
                IsLocked = false // Công trình mới luôn có IsLocked = false
            };

            var factor = await FindFactorAsync(
                work.WorkTypeId,
                work.WorkLevelId,
                request.Author.PurposeId,
                request.Author.AuthorRoleId,
                request.Author.ScoreLevel,
                cancellationToken);

            if (factor == null)
                throw new Exception(ErrorMessages.FactorNotFound);

            var workHour = CalculateWorkHour(request.Author.ScoreLevel, factor);

            await _unitOfWork.Repository<Work>().CreateAsync(work);

            // Lấy UserId từ service
            var (isSuccess, userId, _) = _currentUserService.GetCurrentUser();
            if (!isSuccess)
            {
                throw new Exception(ErrorMessages.UserIdNotDetermined);
            }

            var author = new Author
            {
                Id = Guid.NewGuid(),
                WorkId = work.Id,
                UserId = userId,
                AuthorRoleId = request.Author.AuthorRoleId,
                PurposeId = request.Author.PurposeId,
                Position = request.Author.Position,
                ScoreLevel = request.Author.ScoreLevel,
                WorkHour = workHour,
                SCImagoFieldId = request.Author.SCImagoFieldId,
                FieldId = request.Author.FieldId,
                ProofStatus = ProofStatus.ChuaXuLy,
                CreatedDate = DateTime.UtcNow
            };

            var authorHour = await CalculateAuthorHour(workHour, request.TotalAuthors ?? 0,
                request.TotalMainAuthors ?? 0, request.Author.AuthorRoleId);
            author.AuthorHour = authorHour;
            author.MarkedForScoring = false;

            // Loại bỏ userId của tác giả chính khỏi request.CoAuthorUserIds để tránh trùng lặp
            var coAuthorUserIds = request.CoAuthorUserIds
                .Where(uid => uid != userId) // Loại bỏ userId nếu có
                .Distinct() // Đảm bảo không có trùng lặp trong danh sách
                .ToList();

            // Lưu thông tin đồng tác giả vào WorkAuthor
            foreach (var coAuthorUserId in coAuthorUserIds)
            {
                var workAuthor = new WorkAuthor
                {
                    Id = Guid.NewGuid(),
                    WorkId = work.Id,
                    UserId = coAuthorUserId
                };
                await _unitOfWork.Repository<WorkAuthor>().CreateAsync(workAuthor);
            }

            // Thêm tác giả chính vào WorkAuthor (nếu cần thiết)
            var existingWorkAuthor = await _unitOfWork.Repository<WorkAuthor>()
                .FirstOrDefaultAsync(wa => wa.WorkId == work.Id && wa.UserId == userId, cancellationToken);
            if (existingWorkAuthor == null) // Chỉ thêm nếu chưa tồn tại
            {
                var workAuthor = new WorkAuthor
                {
                    Id = Guid.NewGuid(),
                    WorkId = work.Id,
                    UserId = userId
                };
                await _unitOfWork.Repository<WorkAuthor>().CreateAsync(workAuthor);
            }

            await _unitOfWork.Repository<Author>().CreateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var workDto = _mapper.MapToDto(work);
            // Gán danh sách đồng tác giả không bao gồm tác giả chính
            workDto.CoAuthorUserIds = coAuthorUserIds;
            await SafeInvalidateCacheAsync(work.Id);

            // Nếu là đề tài, tạo bài báo khoa học tự động
            if (work.WorkTypeId == Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348"))
            {
                await CreateScientificArticleFromProjectAsync(work, userId, cancellationToken);
            }

            return workDto;
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

        public async Task SetMarkedForScoringAsync(Guid authorId, bool marked, CancellationToken cancellationToken = default)
        {
            // Kiểm tra trạng thái hệ thống
            if (!await _systemConfigService.IsSystemOpenAsync(DateTime.Now, cancellationToken))
                throw new Exception(ErrorMessages.SystemClosedMarkingWork);

            // Lấy thông tin tác giả và công trình
            var author = await _unitOfWork.Repository<Author>()
                .Include(a => a.Work)
                .FirstOrDefaultAsync(a => a.Id == authorId);

            if (author == null)
                throw new Exception(ErrorMessages.AuthorNotFound);

            var work = author.Work;
            if (work == null)
                throw new Exception(ErrorMessages.WorkNotFound);

            // Kiểm tra giới hạn nếu có
            if (marked)
            {
                // 1. Lấy Factor để kiểm tra giới hạn
                var factor = await FindFactorAsync(
                    work.WorkTypeId,
                    work.WorkLevelId,
                    author.PurposeId,
                    author.AuthorRoleId,
                    author.ScoreLevel,
                    cancellationToken);

                // 2. Đếm số lượng công trình đã được đánh dấu với cùng điều kiện
                var markedAuthors = await _unitOfWork.Repository<Author>().FindAsync(a =>
                    a.UserId == author.UserId &&
                    a.MarkedForScoring &&
                    a.PurposeId == author.PurposeId &&
                    a.ScoreLevel == author.ScoreLevel);

                // 3. Lấy thông tin WorkType và WorkLevel để hiển thị thông báo lỗi
                var workType = await _unitOfWork.Repository<WorkType>().GetByIdAsync(work.WorkTypeId);
                WorkLevel? workLevel = null;
                if (work.WorkLevelId.HasValue)
                {
                    workLevel = await _unitOfWork.Repository<WorkLevel>().GetByIdAsync(work.WorkLevelId.Value);
                }

                // 4. Kiểm tra giới hạn nếu có MaxAllowed
                if (factor?.MaxAllowed != null)
                {
                    if (markedAuthors.Count() >= factor.MaxAllowed)
                    {
                        var workTypeInfo = workType?.Name ?? "Không xác định";
                        var workLevelInfo = workLevel?.Name ?? "Không xác định";
                        var scoreLevelInfo = author.ScoreLevel.HasValue ? author.ScoreLevel.ToString() : "Không xác định";

                        throw new Exception(string.Format(ErrorMessages.ScoreLimitExceeded,
                            factor.MaxAllowed,
                            workTypeInfo,
                            workLevelInfo,
                            scoreLevelInfo));
                    }
                }

                // 5. Tạo bản ghi AuthorRegistration nếu chưa có
                var currentAcademicYear = await _academicYearService.GetCurrentAcademicYear(cancellationToken);
                var existingRegistration = await _unitOfWork.Repository<AuthorRegistration>()
                    .FirstOrDefaultAsync(ar => ar.AuthorId == authorId && ar.AcademicYearId == currentAcademicYear.Id);

                if (existingRegistration == null)
                {
                    var authorRegistration = new AuthorRegistration
                    {
                        AuthorId = authorId,
                        AcademicYearId = currentAcademicYear.Id,
                    };
                    await _unitOfWork.Repository<AuthorRegistration>().CreateAsync(authorRegistration);
                }
            }
            else
            {
                // Nếu bỏ đánh dấu, xóa bản ghi AuthorRegistration nếu có
                var currentAcademicYear = await _academicYearService.GetCurrentAcademicYear(cancellationToken);
                var existingRegistration = await _unitOfWork.Repository<AuthorRegistration>()
                    .FirstOrDefaultAsync(ar => ar.AuthorId == authorId && ar.AcademicYearId == currentAcademicYear.Id);

                if (existingRegistration != null)
                {
                    await _unitOfWork.Repository<AuthorRegistration>().DeleteAsync(existingRegistration.Id);
                }
            }

            // Cập nhật trạng thái đánh dấu
            author.MarkedForScoring = marked;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<WorkDto> UpdateWorkByAdminAsync(Guid workId, Guid userId, UpdateWorkWithAuthorRequestDto request, CancellationToken cancellationToken = default)
        {
            // Truy vấn tuần tự để tránh xung đột DbContext
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(workId);
            if (work is null) throw new Exception(ErrorMessages.WorkNotFound);

            // Tìm Author tương ứng với UserId và WorkId
            var author = await _unitOfWork.Repository<Author>()
                .FirstOrDefaultAsync(a => a.WorkId == workId && a.UserId == userId, cancellationToken);
            if (author is null) throw new Exception(ErrorMessages.AuthorNotFoundInWork);

            // Cập nhật thông tin công trình (Work) nếu có WorkRequest
            if (request.WorkRequest is not null)
            {
                await UpdateWorkDetailsByAdmin(work, request.WorkRequest);
            }

            // Cập nhật thông tin tác giả (Author) nếu có AuthorRequest
            if (request.AuthorRequest is not null)
            {
                await UpdateAuthorDetailsByAdminAsync(author, work, request.AuthorRequest, request.WorkRequest, cancellationToken);
            }

            // Lưu thay đổi vào database - cập nhật tuần tự
            await _unitOfWork.Repository<Work>().UpdateAsync(work);
            await _unitOfWork.Repository<Author>().UpdateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Invalidate cache
            await SafeInvalidateCacheAsync(workId);

            return _mapper.MapToDto(work);
        }

        private async Task UpdateWorkDetailsByAdmin(Work work, UpdateWorkRequestDto workRequest)
        {
            // Cập nhật isLocked dựa trên trạng thái của các tác giả
            var authors = await _unitOfWork.Repository<Author>().FindAsync(a => a.WorkId == work.Id);
            work.IsLocked = authors.Any(a => a.ProofStatus == ProofStatus.HopLe);

            work.Title = workRequest.Title ?? work.Title;
            work.TimePublished = workRequest.TimePublished ?? work.TimePublished;
            work.TotalAuthors = workRequest.TotalAuthors ?? work.TotalAuthors;
            work.TotalMainAuthors = workRequest.TotalMainAuthors ?? work.TotalMainAuthors;
            work.Details = workRequest.Details ?? work.Details;
            work.Source = workRequest.Source;
            work.WorkTypeId = workRequest.WorkTypeId ?? work.WorkTypeId;
            work.WorkLevelId = workRequest.WorkLevelId ?? work.WorkLevelId;
            work.ExchangeDeadline = work.TimePublished.HasValue ? work.TimePublished.Value.AddMonths(18) : null;
            work.ModifiedDate = DateTime.UtcNow;
        }

        private async Task UpdateAuthorDetailsByAdminAsync(
            Author author, 
            Work work, 
            UpdateAuthorRequestDto authorRequest, 
            UpdateWorkRequestDto? workRequest,
            CancellationToken cancellationToken)
        {
            // Cập nhật các thuộc tính cơ bản
            UpdateAuthorBasicProperties(author, authorRequest);

                // Tính lại WorkHour và AuthorHour nếu có thay đổi liên quan
            if (ShouldRecalculateAuthorHours(authorRequest, workRequest))
            {
                await UpdateAuthorHoursAsync(author, work, authorRequest.ScoreLevel, workRequest, cancellationToken);
            }

            // Xử lý ProofStatus và Note
            UpdateAuthorProofStatus(author, authorRequest);

            author.ModifiedDate = DateTime.UtcNow;
        }

        private void UpdateAuthorBasicProperties(Author author, UpdateAuthorRequestDto authorRequest)
        {
            author.AuthorRoleId = authorRequest.AuthorRoleId ?? author.AuthorRoleId;
            author.PurposeId = authorRequest.PurposeId ?? author.PurposeId;
            author.Position = authorRequest.Position ?? author.Position;
            author.ScoreLevel = authorRequest.ScoreLevel ?? author.ScoreLevel;
            author.SCImagoFieldId = authorRequest.SCImagoFieldId ?? author.SCImagoFieldId;
            author.FieldId = authorRequest.FieldId ?? author.FieldId;
        }

        private bool ShouldRecalculateAuthorHours(UpdateAuthorRequestDto authorRequest, UpdateWorkRequestDto? workRequest)
        {
            return authorRequest.ScoreLevel.HasValue ||
                (workRequest is not null && (workRequest.TotalAuthors.HasValue || workRequest.TotalMainAuthors.HasValue));
        }

        private async Task UpdateAuthorHoursAsync(
            Author author,
            Work work,
            ScoreLevel? newScoreLevel,
            UpdateWorkRequestDto? workRequest,
            CancellationToken cancellationToken)
        {
            var effectiveScoreLevel = newScoreLevel ?? author.ScoreLevel;

            // Ghi log thông tin trước khi tính toán
            _logger.LogInformation(
                "UpdateAuthorHoursAsync - Bắt đầu tính giờ quy đổi: WorkTypeId={WorkTypeId}, WorkLevelId={WorkLevelId}, PurposeId={PurposeId}, AuthorRoleId={AuthorRoleId}, ScoreLevel={ScoreLevel}",
                work.WorkTypeId, work.WorkLevelId, author.PurposeId, author.AuthorRoleId, effectiveScoreLevel);

            // Sử dụng phương thức mới để tìm factor
            var factor = await FindFactorAsync(
                work.WorkTypeId,
                work.WorkLevelId,
                author.PurposeId,
                author.AuthorRoleId,
                effectiveScoreLevel,
                cancellationToken);

            if (factor is null)
            {
                _logger.LogError("UpdateAuthorHoursAsync - Không tìm thấy Factor phù hợp");
                throw new Exception(ErrorMessages.FactorNotFound);
            }

            // Tính toán WorkHour và AuthorHour
            author.WorkHour = CalculateWorkHour(effectiveScoreLevel, factor);
            
            // Sử dụng các giá trị từ workRequest nếu có, nếu không thì dùng các giá trị từ work
            int totalAuthors = workRequest?.TotalAuthors ?? work.TotalAuthors ?? 0;
            int totalMainAuthors = workRequest?.TotalMainAuthors ?? work.TotalMainAuthors ?? 0;
            
            author.AuthorHour = await CalculateAuthorHour(
                author.WorkHour,
                totalAuthors,
                totalMainAuthors,
                author.AuthorRoleId);
                
            _logger.LogInformation(
                "UpdateAuthorHoursAsync - Kết quả: Factor.ConvertHour={ConvertHour}, WorkHour={WorkHour}, AuthorHour={AuthorHour}, TotalAuthors={TotalAuthors}, TotalMainAuthors={TotalMainAuthors}",
                factor.ConvertHour, author.WorkHour, author.AuthorHour, totalAuthors, totalMainAuthors);
        }

        private void UpdateAuthorProofStatus(Author author, UpdateAuthorRequestDto authorRequest)
        {
            if (authorRequest.ProofStatus.HasValue)
            {
                author.ProofStatus = authorRequest.ProofStatus.Value;
                author.Note = authorRequest.Note ?? author.Note;
            }
        }

        public async Task<WorkDto> UpdateWorkByAuthorAsync(Guid workId, UpdateWorkWithAuthorRequestDto request, Guid userId, CancellationToken cancellationToken = default)
        {
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(workId);
            if (work is null)
                throw new Exception(ErrorMessages.WorkNotFound);

            _logger.LogInformation("UpdateWorkByAuthorAsync - Received request for workId {WorkId}", workId);
            
            // Lấy coAuthorUserIds từ WorkRequest
            List<Guid> coAuthorUserIds = request.WorkRequest?.CoAuthorUserIds ?? new List<Guid>();
            
            _logger.LogInformation("Found CoAuthorUserIds in WorkRequest: {Count} ids - {CoAuthorUserIds}", 
                coAuthorUserIds.Count, 
                string.Join(", ", coAuthorUserIds));

            await ValidateSystemStateAsync(work, userId, cancellationToken);

            if (request.WorkRequest is not null)
            {
                await UpdateWorkDetailsByAuthor(work, request.WorkRequest);

                // Luôn gọi UpdateCoAuthorsAsync để cập nhật đúng danh sách đồng tác giả
                // Ngay cả khi danh sách rỗng, việc này cũng đảm bảo xóa các đồng tác giả cũ nếu người dùng muốn
                _logger.LogInformation("Updating CoAuthors with list: {Count} userIds - {UserIds}", 
                    coAuthorUserIds.Count, string.Join(", ", coAuthorUserIds));
                await UpdateCoAuthorsAsync(work, coAuthorUserIds, userId, cancellationToken);
            }

            var author = await GetOrCreateAuthorAsync(work, userId, request, cancellationToken);

            // Lưu các thay đổi vào database
            await SaveChangesAsync(work, author, cancellationToken);
            
            _logger.LogInformation("Changes saved successfully for workId {WorkId}", workId);

            // Ánh xạ sang DTO và điền thông tin đồng tác giả
            var workDto = _mapper.MapToDto(work);
            
            _logger.LogInformation("WorkDto mapped. Filling CoAuthorUserIds...");
            await FillCoAuthorUserIdsAsync(workDto, cancellationToken);
            
            _logger.LogInformation("UpdateWorkByAuthorAsync complete. Returning CoAuthorUserIds: {CoAuthorUserIds}", 
                string.Join(", ", workDto.CoAuthorUserIds));
                
            return workDto;
        }

        private async Task UpdateCoAuthorsAsync(Work work, List<Guid> newCoAuthorUserIds, Guid currentUserId, CancellationToken cancellationToken)
        {
            try {
                // Đảm bảo newCoAuthorUserIds không bao gồm currentUserId
                newCoAuthorUserIds = newCoAuthorUserIds.Where(id => id != currentUserId).Distinct().ToList();


                // Lấy tất cả WorkAuthor hiện có của công trình
                var existingWorkAuthors = await _unitOfWork.Repository<WorkAuthor>()
                    .FindAsync(wa => wa.WorkId == work.Id);
                var existingCoAuthorIds = existingWorkAuthors.Select(wa => wa.UserId).ToList();

                // Lấy thêm tất cả Author của công trình (trừ tác giả chính)
                var existingAuthors = await _unitOfWork.Repository<Author>()
                    .FindAsync(a => a.WorkId == work.Id && a.UserId != currentUserId);
                var existingAuthorUserIds = existingAuthors.Select(a => a.UserId).ToList();

                // Tổng hợp tất cả User IDs hiện có (từ cả WorkAuthor và Author)
                var allExistingUserIds = existingCoAuthorIds.Union(existingAuthorUserIds).Distinct().ToList();

                // 1. Xóa các WorkAuthor không còn trong danh sách mới
                var workAuthorsToRemove = existingWorkAuthors.Where(wa => 
                    wa.UserId != currentUserId && !newCoAuthorUserIds.Contains(wa.UserId)).ToList();

                // 2. Xóa các Author không còn trong danh sách mới
                // * Quan trọng: Không xóa Author của người dùng hiện tại (currentUserId)
                var authorsToRemove = existingAuthors.Where(a => 
                    a.UserId != currentUserId && !newCoAuthorUserIds.Contains(a.UserId)).ToList();
                
                foreach (var author in authorsToRemove)
                {
                    work.Authors.Remove(author);
                    // Xóa Author khỏi cơ sở dữ liệu
                    await _unitOfWork.Repository<Author>().DeleteAsync(author.Id);
                }

                // Tìm các userId cần thêm mới: những userId có trong newCoAuthorUserIds nhưng không có trong allExistingUserIds
                var userIdsToAdd = newCoAuthorUserIds.Where(id => 
                    !allExistingUserIds.Contains(id) && id != currentUserId).ToList();

                // Thêm các đồng tác giả mới
                foreach (var coAuthorId in userIdsToAdd)
                {
                    try 
                    {
                        var workAuthor = new WorkAuthor
                        {
                            Id = Guid.NewGuid(),
                            WorkId = work.Id,
                            UserId = coAuthorId
                        };
                        await _unitOfWork.Repository<WorkAuthor>().CreateAsync(workAuthor);
                        
                        // Thêm vào danh sách WorkAuthors của đối tượng Work
                        work.WorkAuthors.Add(workAuthor);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error adding coauthor {UserId} to work {WorkId}", coAuthorId, work.Id);
                    }
                }
                
                _logger.LogInformation("UpdateCoAuthorsAsync completed for work {WorkId}", work.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateCoAuthorsAsync for work {WorkId}", work.Id);
            }
        }

        private async Task ValidateSystemStateAsync(Work work, Guid userId, CancellationToken cancellationToken)
        {
            var currentAuthor = await _unitOfWork.Repository<Author>()
                .FirstOrDefaultAsync(a => a.WorkId == work.Id && a.UserId == userId, cancellationToken);

            if (!await _systemConfigService.IsSystemOpenAsync(DateTime.Now, cancellationToken))
            {
                if (currentAuthor is null || currentAuthor.ProofStatus != ProofStatus.KhongHopLe)
                    throw new Exception(ErrorMessages.SystemClosedEditWork);
            }
        }

        private async Task UpdateWorkDetailsByAuthor(Work work, UpdateWorkRequestDto workRequest)
        {
            // Cập nhật isLocked dựa trên trạng thái của các tác giả
            var authors = await _unitOfWork.Repository<Author>().FindAsync(a => a.WorkId == work.Id);
            work.IsLocked = authors.Any(a => a.ProofStatus == ProofStatus.HopLe);

            work.Title = workRequest.Title ?? work.Title;
            work.TimePublished = workRequest.TimePublished ?? work.TimePublished;
            work.TotalAuthors = workRequest.TotalAuthors ?? work.TotalAuthors;
            work.TotalMainAuthors = workRequest.TotalMainAuthors ?? work.TotalMainAuthors;
            work.Details = workRequest.Details ?? work.Details;
            work.Source = WorkSource.NguoiDungKeKhai; // Luôn đặt nguồn là NguoiDungKeKhai
            work.WorkTypeId = workRequest.WorkTypeId ?? work.WorkTypeId;
            work.WorkLevelId = workRequest.WorkLevelId ?? work.WorkLevelId;
            work.ExchangeDeadline = work.TimePublished.HasValue ? work.TimePublished.Value.AddMonths(18) : null;
            work.ModifiedDate = DateTime.UtcNow;
        }

        private async Task<Author> GetOrCreateAuthorAsync(Work work, Guid userId, UpdateWorkWithAuthorRequestDto request, CancellationToken cancellationToken)
        {
            var author = await _unitOfWork.Repository<Author>()
                .FirstOrDefaultAsync(a => a.WorkId == work.Id && a.UserId == userId, cancellationToken);

            if (author is null)
            {
                // Tạo mới tác giả
                author = await CreateNewAuthorAsync(work, userId, request, cancellationToken);
            }
            else if (request.AuthorRequest is not null)
            {
                // Cập nhật tác giả đã tồn tại
                await UpdateExistingAuthorAsync(author, work, request, cancellationToken);
            }

            return author;
        }

        private async Task<Author> CreateNewAuthorAsync(Work work, Guid userId, UpdateWorkWithAuthorRequestDto request, CancellationToken cancellationToken)
        {
            if (request.AuthorRequest is null)
            {
                throw new Exception(ErrorMessages.AuthorInfoRequired);
            }

            // Tính toán WorkHour và AuthorHour cho tác giả mới
            var factor = await FindFactorAsync(
                work.WorkTypeId,
                work.WorkLevelId,
                request.AuthorRequest.PurposeId ?? Guid.Empty,
                request.AuthorRequest.AuthorRoleId,
                request.AuthorRequest.ScoreLevel,
                cancellationToken);

            if (factor is null)
                throw new Exception(ErrorMessages.FactorNotFound);

            var workHour = CalculateWorkHour(request.AuthorRequest.ScoreLevel, factor);
            var authorHour = await CalculateAuthorHour(
                workHour,
                work.TotalAuthors ?? 0,
                work.TotalMainAuthors ?? 0,
                request.AuthorRequest.AuthorRoleId);

            // Tạo mới thông tin tác giả
            return new Author
            {
                UserId = userId,
                WorkId = work.Id,
                AuthorRoleId = request.AuthorRequest.AuthorRoleId,
                PurposeId = request.AuthorRequest.PurposeId ?? Guid.Empty,
                Position = request.AuthorRequest.Position,
                ScoreLevel = request.AuthorRequest.ScoreLevel,
                WorkHour = workHour,
                AuthorHour = authorHour,
                SCImagoFieldId = request.AuthorRequest.SCImagoFieldId,
                FieldId = request.AuthorRequest.FieldId,
                ProofStatus = ProofStatus.ChuaXuLy,
                CreatedDate = DateTime.UtcNow
            };
        }

        private async Task<Factor> FindFactorAsync(Guid workTypeId, Guid? workLevelId, Guid purposeId, Guid? authorRoleId, ScoreLevel? scoreLevel, CancellationToken cancellationToken = default)
        {
            // Tìm kiếm chính xác theo tất cả điều kiện
            var factors = await _factorRepository.FindAsync(f =>
                f.WorkTypeId == workTypeId &&
                // Xử lý WorkLevelId null
                ((workLevelId == null && f.WorkLevelId == null) || (workLevelId != null && f.WorkLevelId == workLevelId)) &&
                f.PurposeId == purposeId &&
                // Xử lý AuthorRoleId null
                ((authorRoleId == null && f.AuthorRoleId == null) || (authorRoleId != null && f.AuthorRoleId == authorRoleId)) &&
                // Xử lý ScoreLevel null
                ((scoreLevel == null && f.ScoreLevel == null) || (scoreLevel != null && f.ScoreLevel == scoreLevel)));

            var factor = factors.FirstOrDefault();

            // Nếu không tìm thấy và authorRoleId có giá trị, thử tìm với authorRoleId = null
            if (factor == null && authorRoleId.HasValue)
            {
                factors = await _factorRepository.FindAsync(f =>
                    f.WorkTypeId == workTypeId &&
                    ((workLevelId == null && f.WorkLevelId == null) || (workLevelId != null && f.WorkLevelId == workLevelId)) &&
                    f.PurposeId == purposeId &&
                    f.AuthorRoleId == null &&
                    ((scoreLevel == null && f.ScoreLevel == null) || (scoreLevel != null && f.ScoreLevel == scoreLevel)));

                factor = factors.FirstOrDefault();
            }

            // Log thông tin debug
            if (factor != null)
            {
                _logger.LogInformation(
                    "Tìm thấy Factor với WorkTypeId={WorkTypeId}, WorkLevelId={WorkLevelId}, PurposeId={PurposeId}, AuthorRoleId={AuthorRoleId}, ScoreLevel={ScoreLevel}. ConvertHour={ConvertHour}",
                    workTypeId, workLevelId, purposeId, authorRoleId, scoreLevel, factor.ConvertHour);
            }
            else
            {
                _logger.LogWarning(
                    "Không tìm thấy Factor cho WorkTypeId={WorkTypeId}, WorkLevelId={WorkLevelId}, PurposeId={PurposeId}, AuthorRoleId={AuthorRoleId}, ScoreLevel={ScoreLevel}",
                    workTypeId, workLevelId, purposeId, authorRoleId, scoreLevel);
            }

            return factor;
        }

        private async Task UpdateExistingAuthorAsync(Author author, Work work, UpdateWorkWithAuthorRequestDto request, CancellationToken cancellationToken)
        {
            // Cập nhật thông tin tác giả
            if (request.AuthorRequest is not null)
            {
                // Ghi log trạng thái ban đầu
                _logger.LogInformation(
                    "UpdateExistingAuthorAsync - Trạng thái ban đầu: AuthorId={AuthorId}, WorkHour={WorkHour}, AuthorHour={AuthorHour}, ScoreLevel={ScoreLevel}",
                    author.Id, author.WorkHour, author.AuthorHour, author.ScoreLevel);
                
                // AuthorRoleId và PurposeId là Guid không nullable, gán trực tiếp
                author.AuthorRoleId = request.AuthorRequest.AuthorRoleId;
                author.PurposeId = request.AuthorRequest.PurposeId ?? Guid.Empty;

                author.Position = request.AuthorRequest.Position ?? author.Position;
                
                // Lưu trữ giá trị ScoreLevel ban đầu và mới để so sánh
                var oldScoreLevel = author.ScoreLevel;
                author.ScoreLevel = request.AuthorRequest.ScoreLevel ?? author.ScoreLevel;
                var hasScoreLevelChanged = request.AuthorRequest.ScoreLevel.HasValue;

                // SCImagoFieldId và FieldId là nullable Guid
                author.SCImagoFieldId = request.AuthorRequest.SCImagoFieldId;
                author.FieldId = request.AuthorRequest.FieldId;

                // Tính lại WorkHour và AuthorHour khi có thay đổi liên quan
                // Luôn tính toán lại nếu ScoreLevel thay đổi
                bool shouldRecalculate = hasScoreLevelChanged ||
                    (request.WorkRequest is not null && (request.WorkRequest.TotalAuthors.HasValue || request.WorkRequest.TotalMainAuthors.HasValue));

                if (shouldRecalculate)
                {
                    _logger.LogInformation(
                        "UpdateExistingAuthorAsync - Tính toán lại giờ quy đổi: WorkTypeId={WorkTypeId}, WorkLevelId={WorkLevelId}, PurposeId={PurposeId}, AuthorRoleId={AuthorRoleId}, ScoreLevel={ScoreLevel}",
                        work.WorkTypeId, work.WorkLevelId, author.PurposeId, author.AuthorRoleId, author.ScoreLevel);
                        
                    var factor = await FindFactorAsync(
                        work.WorkTypeId,
                        work.WorkLevelId,
                        author.PurposeId,
                        author.AuthorRoleId,
                        author.ScoreLevel,
                        cancellationToken);

                    if (factor is null)
                    {
                        _logger.LogError(
                            "UpdateExistingAuthorAsync - Không tìm thấy Factor: WorkTypeId={WorkTypeId}, WorkLevelId={WorkLevelId}, PurposeId={PurposeId}, AuthorRoleId={AuthorRoleId}, ScoreLevel={ScoreLevel}",
                            work.WorkTypeId, work.WorkLevelId, author.PurposeId, author.AuthorRoleId, author.ScoreLevel);
                        throw new Exception(ErrorMessages.FactorNotFound);
                    }

                    // Tính giờ quy đổi
                    author.WorkHour = CalculateWorkHour(author.ScoreLevel, factor);
                    author.AuthorHour = await CalculateAuthorHour(
                        author.WorkHour,
                        work.TotalAuthors ?? 0,
                        work.TotalMainAuthors ?? 0,
                        author.AuthorRoleId);
                        
                    _logger.LogInformation(
                        "UpdateExistingAuthorAsync - Kết quả tính toán: Factor.ConvertHour={ConvertHour}, WorkHour={WorkHour}, AuthorHour={AuthorHour}",
                        factor.ConvertHour, author.WorkHour, author.AuthorHour);
                }
                else
                {
                    _logger.LogInformation("UpdateExistingAuthorAsync - Không cần tính lại giờ quy đổi");
                }

                author.ModifiedDate = DateTime.UtcNow;
                
                _logger.LogInformation(
                    "UpdateExistingAuthorAsync - Trạng thái sau cập nhật: AuthorId={AuthorId}, WorkHour={WorkHour}, AuthorHour={AuthorHour}, ScoreLevel={ScoreLevel}",
                    author.Id, author.WorkHour, author.AuthorHour, author.ScoreLevel);
            }
        }

        private async Task SaveChangesAsync(Work work, Author author, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.Repository<Work>().UpdateAsync(work);

            if (author.Id == Guid.Empty)
            {
                await _unitOfWork.Repository<Author>().CreateAsync(author);
            }
            else
            {
                await _unitOfWork.Repository<Author>().UpdateAsync(author);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SafeInvalidateCacheAsync(work.Id);
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
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

            // Lấy năm học hiện tại
            var currentAcademicYear = await _academicYearService.GetCurrentAcademicYear(cancellationToken);
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

            // Lọc và xử lý dữ liệu
            var filteredWorks = works
                .Where(work => work.Source == WorkSource.NguoiDungKeKhai) // Chỉ lấy công trình do người dùng kê khai
                .Select(work =>
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
                })
                .Where(w => w != null)
                .ToList();

            var filteredWorksByRegistration = new List<WorkDto>();
            foreach (var work in filteredWorks)
            {
                var author = work.Authors?.FirstOrDefault();

                // 2. Nếu user là đồng tác giả (không phải tác giả chính), thêm vào danh sách
                if (author == null && workAuthorWorkIds.Contains(work.Id))
                {
                    filteredWorksByRegistration.Add(work);
                    continue;
                }

                // Bỏ qua nếu không phải tác giả chính
                if (author == null) continue;

                // Kiểm tra xem công trình có thuộc đợt hiện tại không
                var workDate = DateOnly.FromDateTime(work.CreatedDate);
                var isCurrentPeriod = workDate >= currentAcademicYear.StartDate && workDate <= currentAcademicYear.EndDate;

                // 1. Nếu là công trình của đợt hiện tại, thêm vào danh sách
                if (isCurrentPeriod)
                {
                    filteredWorksByRegistration.Add(work);
                    continue;
                }

                // 3. Xử lý công trình của các đợt trước
                var authorEntity = await _unitOfWork.Repository<Author>()
                    .FirstOrDefaultAsync(a => a.Id == author.Id);

                if (authorEntity == null) continue;

                // Nếu đã đánh dấu quy đổi thì bỏ qua
                if (authorEntity.MarkedForScoring) continue;

                // Kiểm tra thời hạn quy đổi
                if (work.TimePublished.HasValue)
                {
                    var exchangeDeadline = work.TimePublished.Value.AddMonths(18);
                    
                    // Nếu quá hạn, cập nhật WorkHour = 0
                    if (currentDate > exchangeDeadline)
                    {
                        authorEntity.WorkHour = 0;
                        authorEntity.AuthorHour = 0;
                        await _unitOfWork.Repository<Author>().UpdateAsync(authorEntity);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                }

                // Thêm vào danh sách vì chưa đánh dấu quy đổi
                filteredWorksByRegistration.Add(work);
            }

            await FillCoAuthorUserIdsForMultipleWorksAsync(filteredWorksByRegistration, cancellationToken);

            return filteredWorksByRegistration;
        }

        /// <summary>
        /// Lấy tất cả công trình của người dùng, bao gồm cả công trình do người dùng kê khai và công trình được admin import vào
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <param name="cancellationToken">Token hủy</param>
        /// <returns>Danh sách công trình</returns>
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

        private int CalculateWorkHour(ScoreLevel? scoreLevel, Factor factor)
        {
            // Nếu factor không tồn tại, trả về 0
            if (factor is null)
            {
                _logger.LogWarning("CalculateWorkHour - Factor is null, returning 0");
                return 0;
            }
            
            // Kiểm tra xem ScoreLevel có khớp không
            if ((scoreLevel.HasValue != factor.ScoreLevel.HasValue) || 
                (scoreLevel.HasValue && factor.ScoreLevel.HasValue && scoreLevel.Value != factor.ScoreLevel.Value))
            {
                _logger.LogWarning("CalculateWorkHour - ScoreLevel mismatch: Request={RequestLevel}, Factor={FactorLevel}, nhưng vẫn trả về ConvertHour={ConvertHour}",
                    scoreLevel.HasValue ? scoreLevel.Value.ToString() : "null",
                    factor.ScoreLevel.HasValue ? factor.ScoreLevel.Value.ToString() : "null",
                    factor.ConvertHour);
                    
                // Vẫn trả về ConvertHour của Factor nếu Factor được tìm thấy
                return factor.ConvertHour;
            }
            
            // Nếu mọi thứ khớp hoặc cả hai đều null, trả về ConvertHour
            _logger.LogInformation("CalculateWorkHour - Returning ConvertHour={ConvertHour} cho ScoreLevel={ScoreLevel}", 
                factor.ConvertHour, 
                scoreLevel.HasValue ? scoreLevel.Value.ToString() : "null");
            return factor.ConvertHour;
        }

        private async Task<decimal> CalculateAuthorHour(int workHour, int totalAuthors, int totalMainAuthors, Guid? authorRoleId, CancellationToken cancellationToken = default)
        {
            // Hằng số tỷ lệ phân bổ giờ chuẩn
            const double MAIN_AUTHOR_RATIO = 1.0 / 3;
            const double REGULAR_RATIO = 2.0 / 3;
            
            // Kiểm tra các điều kiện không hợp lệ
            if (workHour <= 0 || totalAuthors <= 0 || totalMainAuthors <= 0 || totalMainAuthors > totalAuthors || authorRoleId == null)
            {
                return 0;
            }

            // Lấy thông tin vai trò tác giả
            var authorRole = await _authorRoleRepository.GetByIdAsync(authorRoleId.Value);
            if (authorRole is null)
            {
                throw new Exception(ErrorMessages.AuthorRoleNotFound);
            }

            decimal authorHour;
            
            // Tính toán giờ chuẩn dựa trên vai trò tác giả (chính/thường)
            if (authorRole.IsMainAuthor)
            {
                // Công thức cho tác giả chính
                authorHour = (decimal)(MAIN_AUTHOR_RATIO * (workHour / totalMainAuthors) + 
                                      REGULAR_RATIO * (workHour / totalAuthors));
            }
            else
            {
                // Công thức cho tác giả thường
                authorHour = (decimal)(REGULAR_RATIO * (workHour / totalAuthors));
            }

            // Làm tròn đến 1 chữ số thập phân để đảm bảo tính nhất quán
            return Math.Round(authorHour, 1);
        }

        public async Task DeleteWorkAsync(Guid workId, Guid userId, CancellationToken cancellationToken = default)
        {
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(workId);
            if (work is null)
                throw new Exception(ErrorMessages.WorkNotFound);

            if (!await _systemConfigService.IsSystemOpenAsync(DateTime.Now, cancellationToken))
                throw new Exception(ErrorMessages.SystemClosedDeleteWork);

            // Xóa các tác giả liên quan
            var authors = await _unitOfWork.Repository<Author>().FindAsync(a => a.WorkId == workId);
            foreach (var author in authors)
            {
                await _unitOfWork.Repository<Author>().DeleteAsync(author.Id);
            }

            await _unitOfWork.Repository<Work>().DeleteAsync(work.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await SafeInvalidateCacheAsync(workId);
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

        public async Task<byte[]> ExportWorksByUserAsync(List<ExportExcelDto> exportData, CancellationToken cancellationToken = default)
        {
            // Thử tìm file template ở các vị trí khác nhau
            string[] possiblePaths = new[]
            {
                // Đường dẫn từ thư mục bin của WebApi
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Infrastructure", "Templates", "ExportByUserTemplate.xlsx"),
                
                // Đường dẫn từ thư mục gốc của Server project
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Server", "Infrastructure", "Templates", "ExportByUserTemplate.xlsx")),
                
                // Đường dẫn từ thư mục gốc của solution
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Infrastructure", "Templates", "ExportByUserTemplate.xlsx"))
            };

            string? templatePath = null;
            foreach (var path in possiblePaths)
            {
                var normalizedPath = Path.GetFullPath(path);
                _logger.LogInformation("Đang thử tìm file template tại: {Path}", normalizedPath);
                
                if (File.Exists(normalizedPath))
                {
                    templatePath = normalizedPath;
                    _logger.LogInformation("Đã tìm thấy file template tại: {Path}", normalizedPath);
                    break;
                }
            }

            if (templatePath == null)
            {
                var errorMessage = $"Không tìm thấy file template Excel. Đã thử các đường dẫn sau:\n{string.Join("\n", possiblePaths.Select(p => Path.GetFullPath(p)))}";
                _logger.LogError(errorMessage);
                throw new FileNotFoundException(errorMessage);
            }

            using (var package = new ExcelPackage(new FileInfo(templatePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Lấy sheet đầu tiên

                // Điền thông tin cá nhân từ bản ghi đầu tiên
                var userInfo = exportData.FirstOrDefault();
                if (userInfo is null)
                    throw new Exception(ErrorMessages.NoDataToExport);

                // Điền thông tin cá nhân vào các ô tương ứng
                worksheet.Cells["C2"].Value = userInfo.DepartmentName;

                worksheet.Cells["B8"].Value = userInfo.FullName;
                worksheet.Cells["B9"].Value = userInfo.FieldName;
                worksheet.Cells["B10"].Value = userInfo.PhoneNumber;

                worksheet.Cells["E8"].Value = userInfo.AcademicTitle;
                worksheet.Cells["E9"].Value = userInfo.Specialization;
                worksheet.Cells["E10"].Value = userInfo.Email;

                worksheet.Cells["H8"].Value = userInfo.UserName;
                worksheet.Cells["H9"].Value = userInfo.OfficerRank;

                // Điền dữ liệu công trình vào bảng
                int startRow = 15; // Dòng bắt đầu của dữ liệu
                int currentRow = startRow;

                // Điền thông tin từng công trình
                for (int i = 0; i < exportData.Count; i++)
                {
                    var work = exportData[i];
                    
                    worksheet.Cells[currentRow, 1].Value = i + 1;
                    worksheet.Cells[currentRow, 2].Value = work.Title;
                    worksheet.Cells[currentRow, 3].Value = work.WorkTypeName;
                    worksheet.Cells[currentRow, 4].Value = work.WorkLevelName ?? "";
                    worksheet.Cells[currentRow, 5].Value = work.TimePublished.HasValue ? 
                        $"{work.TimePublished.Value.Month:D2}/{work.TimePublished.Value.Year}" : 
                        "";
                    worksheet.Cells[currentRow, 6].Value = work.TotalAuthors.HasValue ? work.TotalAuthors.Value : "";
                    worksheet.Cells[currentRow, 7].Value = work.TotalMainAuthors.HasValue ? work.TotalMainAuthors.Value : "";
                    worksheet.Cells[currentRow, 8].Value = work.Position.HasValue ? work.Position.Value : "";
                    worksheet.Cells[currentRow, 9].Value = work.AuthorRoleName ?? "";
                    worksheet.Cells[currentRow, 10].Value = work.CoAuthorNames ?? "";
                    worksheet.Cells[currentRow, 11].Value = work.Details != null && work.Details.Any() ? 
                        string.Join("; ", work.Details.Select(kv => $"{kv.Key}: {kv.Value}")) : 
                        "";
                    worksheet.Cells[currentRow, 12].Value = work.PurposeName ?? "";

                    // Xử lý hiển thị mức điểm
                    string scoreLevelText = work.ScoreLevel switch
                    {
                        ScoreLevel.BaiBaoMotDiem => "Bài báo khoa học 1 điểm",
                        ScoreLevel.BaiBaoNuaDiem => "Bài báo khoa học 0.5 điểm",
                        ScoreLevel.BaiBaoKhongBayNamDiem => "Bài báo khoa học 0.75 điểm",
                        ScoreLevel.BaiBaoTopMuoi => "Bài báo khoa học Top 10%",
                        ScoreLevel.BaiBaoTopBaMuoi => "Bài báo khoa học Top 30%",
                        ScoreLevel.BaiBaoTopNamMuoi => "Bài báo khoa học Top 50%",
                        ScoreLevel.BaiBaoTopConLai => "Bài báo khoa học Top còn lại",

                        ScoreLevel.HDSVDatGiaiNhat => "Hướng dẫn sinh viên đạt giải Nhất",
                        ScoreLevel.HDSVDatGiaiNhi => "Hướng dẫn sinh viên đạt giải Nhì",
                        ScoreLevel.HDSVDatGiaiBa => "Hướng dẫn sinh viên đạt giải Ba",
                        ScoreLevel.HDSVDatGiaiKK => "Hướng dẫn sinh viên đạt giải Khuyến khích",
                        ScoreLevel.HDSVConLai => "Hướng dẫn sinh viên đạt các giải còn lại",

                        ScoreLevel.TacPhamNgheThuatCapTruong => "Tác phẩm nghệ thuật cấp Trường",
                        ScoreLevel.TacPhamNgheThuatCapQuocGia => "Tác phẩm nghệ thuật cấp Quốc gia",
                        ScoreLevel.TacPhamNgheThuatCapQuocTe => "Tác phẩm nghệ thuật cấp Quốc tế",
                        ScoreLevel.TacPhamNgheThuatCapTinhThanhPho => "Tác phẩm nghệ thuật cấp Tỉnh/Thành phố",

                        ScoreLevel.ThanhTichHuanLuyenCapQuocGia => "Thành tích huấn luyện cấp Quốc gia",
                        ScoreLevel.ThanhTichHuanLuyenCapQuocTe => "Thành tích huấn luyện cấp Quốc tế",

                        ScoreLevel.GiaiPhapHuuIchCapQuocGia => "Giải pháp hữu ích cấp Quốc gia",
                        ScoreLevel.GiaiPhapHuuIchCapQuocTe => "Giải pháp hữu ích cấp Quốc tế",
                        ScoreLevel.GiaiPhapHuuIchCapTinhThanhPho => "Giải pháp hữu ích cấp Tỉnh/Thành phố",

                        ScoreLevel.KetQuaNghienCuu => "Kết quả nghiên cứu",

                        ScoreLevel.Sach => "Sách",
                        _ => ""
                    };

                    worksheet.Cells[currentRow, 13].Value = scoreLevelText;
                    worksheet.Cells[currentRow, 14].Value = work.SCImagoFieldName ?? "";
                    worksheet.Cells[currentRow, 15].Value = work.ScoringFieldName ?? "";
                    
                    // Định dạng giờ quy đổi với 1 chữ số thập phân
                    if (work.AuthorHour.HasValue)
                    {
                        var authorHourValue = (decimal)work.AuthorHour.Value;
                        worksheet.Cells[currentRow, 16].Value = Math.Round(authorHourValue, 1, MidpointRounding.AwayFromZero);
                        worksheet.Cells[currentRow, 16].Style.Numberformat.Format = "#,##0.0";
                    }
                    else
                    {
                        worksheet.Cells[currentRow, 16].Value = "";
                    }

                    // Xử lý hiển thị trạng thái
                    string proofStatusText = work.ProofStatus switch
                    {
                        ProofStatus.ChuaXuLy => "Chưa xử lý",
                        ProofStatus.HopLe => "Hợp lệ",
                        ProofStatus.KhongHopLe => "Không hợp lệ",
                        _ => ""
                    };

                    worksheet.Cells[currentRow, 17].Value = proofStatusText;
                    worksheet.Cells[currentRow, 18].Value = work.Note ?? "";

                    currentRow++;
                }

                // Tính tổng số công trình theo loại
                // Để trống 2 dòng sau danh sách công trình
                var summaryStartRow = currentRow + 2;

                // Chèn dòng trống cho phần tổng hợp
                worksheet.InsertRow(summaryStartRow, exportData.Count + 3); // +3 cho tiêu đề, dòng trống và tổng số

                // Thêm tiêu đề phần tổng hợp
                worksheet.Cells[summaryStartRow, 1, summaryStartRow, 3].Merge = true;
                worksheet.Cells[summaryStartRow, 1].Value = "III. TỔNG HỢP CÔNG TRÌNH THEO LOẠI";
                worksheet.Cells[summaryStartRow, 1].Style.Font.Bold = true;

                // Bắt đầu điền dữ liệu tổng hợp từ dòng tiếp theo
                summaryStartRow++;

                var workTypeCounts = exportData
                    .GroupBy(w => w.WorkTypeName ?? "Khác")
                    .ToDictionary(g => g.Key, g => g.Count());

                var workTypes = new[] { "Bài báo khoa học", "Báo cáo khoa học", "Đề tài", "Giáo trình", "Sách", "Hội thảo, hội nghị", "Hướng dẫn SV NCKH", "Khác" };
                for (int i = 0; i < workTypes.Length; i++)
                {
                    worksheet.Cells[summaryStartRow + i, 1].Value = i + 1;
                    worksheet.Cells[summaryStartRow + i, 2].Value = workTypes[i];
                    worksheet.Cells[summaryStartRow + i, 3].Value = workTypeCounts.ContainsKey(workTypes[i]) ? workTypeCounts[workTypes[i]] : 0;
                }

                // Tổng số sản phẩm
                worksheet.Cells[summaryStartRow + workTypes.Length + 1, 1].Value = "Tổng số sản phẩm:";
                worksheet.Cells[summaryStartRow + workTypes.Length + 1, 2, summaryStartRow + workTypes.Length + 1, 3].Merge = true;
                worksheet.Cells[summaryStartRow + workTypes.Length + 1, 2].Value = exportData.Count;

                return package.GetAsByteArray();
            }
        }

        public async Task<List<ExportExcelDto>> GetExportExcelDataAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            // Lấy thông tin cá nhân của user với đầy đủ details
            var user = await _userRepository.GetUserByIdWithDetailsAsync(userId);
            if (user is null)
                throw new Exception(ErrorMessages.UserNotFound);

            // Lấy danh sách công trình của user
            var works = await GetWorksByCurrentUserAsync(userId, cancellationToken);

            // Lấy thông tin đồng tác giả với đầy đủ details
            var allCoAuthorUserIds = works.SelectMany(w => w.CoAuthorUserIds).Distinct().ToList();
            var coAuthors = await _userRepository.GetUsersWithDetailsAsync();
            var filteredCoAuthors = coAuthors.Where(u => allCoAuthorUserIds.Contains(u.Id));

            // Ánh xạ dữ liệu vào ExportExcelDto
            var exportData = works.Select(w =>
            {
                var author = w.Authors?.FirstOrDefault();
                var coAuthorNames = string.Join(", ", w.CoAuthorUserIds
                    .Select(uid => filteredCoAuthors.FirstOrDefault(u => u.Id == uid)?.FullName ?? "Không xác định"));

                return new ExportExcelDto
                {
                    UserName = user.UserName ?? "Không xác định",
                    FullName = user.FullName ?? "Không xác định",
                    Email = user.Email ?? "Không xác định",
                    AcademicTitle = user.AcademicTitle.ToString() ?? "Không xác định", // Học hàm/học vị
                    OfficerRank = user.OfficerRank.ToString() ?? "Không xác định", // Ngạch công chức
                    DepartmentName = user.Department?.Name ?? "Không xác định",
                    FieldName = author?.FieldName ?? "Không xác định", // Ngành - Field
                    Specialization= user.Specialization ?? "Không xác định", // Chuyên ngành
                    PhoneNumber = user.PhoneNumber ?? "Không xác định",
                    Title = w.Title ?? "Không xác định",
                    WorkTypeName = w.WorkTypeName ?? "Không xác định",
                    WorkLevelName = w.WorkLevelName,
                    TimePublished = w.TimePublished,
                    TotalAuthors = w.TotalAuthors,
                    TotalMainAuthors = w.TotalMainAuthors,
                    Position = author?.Position,
                    AuthorRoleName = author?.AuthorRoleName,
                    CoAuthorUserIds = w.CoAuthorUserIds,
                    CoAuthorNames = coAuthorNames,
                    Details = w.Details ?? new Dictionary<string, string>(),
                    PurposeName = author?.PurposeName,
                    SCImagoFieldName = author?.SCImagoFieldName,
                    ScoringFieldName = author?.FieldName,
                    ScoreLevel = author?.ScoreLevel,
                    AuthorHour = (int?)author?.AuthorHour, // Chuyển đổi decimal sang int
                    ProofStatus = author?.ProofStatus,
                    Note = author?.Note
                };
            }).ToList();

            return exportData;
        }

        public async Task ImportAsync(IFormFile file)
        {
            if (file is null || file.Length == 0)
                throw new ArgumentException(ErrorMessages.InvalidFile);

            var importRows = new List<ImportExcelDto>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                // Cấp phép dùng EPPlus trong môi trường non-commercial
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.First();
                    int rowCount = worksheet.Dimension.Rows;

                    // Giả sử dòng đầu tiên là header
                    for (int row = 2; row <= rowCount; row++)
                    {
                        // Parse cột Details (giả sử cột 16)
                        string detailsText = worksheet.Cells[row, 16].Text.Trim();
                        Dictionary<string, string> details = new Dictionary<string, string>();
                        if (!string.IsNullOrEmpty(detailsText))
                        {
                            var lines = detailsText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var line in lines)
                            {
                                var parts = line.Split(new[] { ':' }, 2);
                                if (parts.Length == 2)
                                {
                                    string key = parts[0].Trim();
                                    string value = parts[1].Trim();
                                    if (!string.IsNullOrEmpty(key))
                                        details[key] = value;
                                }
                            }
                        }

                        var dto = new ImportExcelDto
                        {
                            Username = worksheet.Cells[row, 1].Text.Trim(),
                            FullName = worksheet.Cells[row, 2].Text.Trim(),
                            DepartmentName = worksheet.Cells[row, 3].Text.Trim(),
                            Title = worksheet.Cells[row, 4].Text.Trim(),
                            WorkTypeName = worksheet.Cells[row, 5].Text.Trim(),
                            WorkLevelName = worksheet.Cells[row, 6].Text.Trim(),
                            TimePublished = ParseMonthYear(worksheet.Cells[row, 7].Text.Trim()),
                            TotalAuthors = int.TryParse(worksheet.Cells[row, 8].Text.Trim(), out var ta) ? ta : (int?)null,
                            TotalMainAuthors = int.TryParse(worksheet.Cells[row, 9].Text.Trim(), out var tma) ? tma : (int?)null,
                            Position = int.TryParse(worksheet.Cells[row, 10].Text.Trim(), out var pos) ? pos : (int?)null,
                            AuthorRoleName = worksheet.Cells[row, 11].Text.Trim(),
                            PurposeName = worksheet.Cells[row, 12].Text.Trim(),
                            SCImagoFieldName = worksheet.Cells[row, 13].Text.Trim(),
                            ScoringFieldName = worksheet.Cells[row, 14].Text.Trim(),
                            ScoreLevel = Enum.TryParse<ScoreLevel>(worksheet.Cells[row, 15].Text.Trim(), out var scoreLevel)
                                                ? scoreLevel
                                                : (ScoreLevel?)null,
                            Details = details
                        };

                        importRows.Add(dto);
                    }
                }
            }

            await ProcessImportRows(importRows);
        }

        private async Task ProcessImportRows(List<ImportExcelDto> importRows)
        {
            var userRepo = _unitOfWork.Repository<User>();
            var authorRepo = _unitOfWork.Repository<Author>();
            var authorRoleRepo = _unitOfWork.Repository<AuthorRole>();
            var purposeRepo = _unitOfWork.Repository<Purpose>();
            var scImagoRepo = _unitOfWork.Repository<SCImagoField>();
            var fieldRepo = _unitOfWork.Repository<Field>();
            var workRepo = _unitOfWork.Repository<Work>();
            var workTypeRepo = _unitOfWork.Repository<WorkType>();
            var workLevelRepo = _unitOfWork.Repository<WorkLevel>();

            foreach (var dto in importRows)
            {
                // 1) Tìm User
                var user = await userRepo.FirstOrDefaultAsync(u =>
                    u.UserName.ToLower() == dto.Username.ToLower());
                if (user is null) continue;

                // 2) Tìm WorkType, WorkLevel
                var workType = await workTypeRepo.FirstOrDefaultAsync(wt =>
                    wt.Name.ToLower() == dto.WorkTypeName.ToLower());
                if (workType is null) continue;

                WorkLevel? workLevel = null;
                if (!string.IsNullOrWhiteSpace(dto.WorkLevelName))
                {
                    workLevel = await workLevelRepo.FirstOrDefaultAsync(wl =>
                        wl.Name.ToLower() == dto.WorkLevelName.ToLower());
                }

                // 3) Tìm công trình dựa trên 4 trường: Title, WorkTypeId, WorkLevelId, TimePublished
                //    Nếu 4 trường này trùng nhau thì xem là cùng 1 công trình
                // Lấy Id nếu workLevel != null
                Guid? workLevelId = workLevel?.Id;
                DateOnly? timePublished = null;
                if (dto.TimePublished.HasValue)
                {
                    timePublished = new DateOnly(dto.TimePublished.Value.Year, dto.TimePublished.Value.Month, 1);
                }

                var existingWork = await workRepo.FirstOrDefaultAsync(w =>
                    w.Title.ToLower() == dto.Title.ToLower() &&
                    w.WorkTypeId == workType.Id &&
                    (workLevelId == null ? w.WorkLevelId == null : w.WorkLevelId == workLevelId) &&
                    (timePublished == null ? w.TimePublished == null : 
                     w.TimePublished.Value.Year == timePublished.Value.Year && 
                     w.TimePublished.Value.Month == timePublished.Value.Month)
                );


                Work work;
                if (existingWork is null)
                {
                    // => Chưa có công trình này, tạo mới
                    work = new Work
                    {
                        Title = dto.Title,
                        TimePublished = timePublished,
                        TotalAuthors = dto.TotalAuthors,
                        TotalMainAuthors = dto.TotalMainAuthors,
                        Details = dto.Details,
                        WorkTypeId = workType.Id,
                        WorkLevelId = workLevel?.Id,
                        Source = WorkSource.QuanLyNhap
                    };
                    await workRepo.CreateAsync(work);
                }
                else
                {
                    work = existingWork;
                }

                // 4) Tìm AuthorRole, Purpose, SCImagoField, ScoringField
                var authorRole = await authorRoleRepo.FirstOrDefaultAsync(ar =>
                    ar.Name.ToLower() == dto.AuthorRoleName.ToLower());
                if (authorRole is null) continue;

                var purpose = await purposeRepo.FirstOrDefaultAsync(p =>
                    p.Name.ToLower() == dto.PurposeName.ToLower());
                if (purpose is null) continue;

                SCImagoField? scImagoField = null;
                if (!string.IsNullOrWhiteSpace(dto.SCImagoFieldName))
                {
                    scImagoField = await scImagoRepo.FirstOrDefaultAsync(s =>
                        s.Name.ToLower() == dto.SCImagoFieldName.ToLower());
                }

                Field? field = null;
                if (!string.IsNullOrWhiteSpace(dto.ScoringFieldName))
                {
                    field = await fieldRepo.FirstOrDefaultAsync(sf =>
                        sf.Name.ToLower() == dto.ScoringFieldName.ToLower());
                }

                // 5) Kiểm tra Author đã tồn tại chưa (dựa trên UserId + WorkId)
                var existingAuthor = await authorRepo.FirstOrDefaultAsync(a =>
                    a.UserId == user.Id && a.WorkId == work.Id);
                if (existingAuthor is not null)
                {
                    // => Tác giả đã tồn tại, bỏ qua hoặc update
                    continue;
                }

                // 6) Tạo mới Author
                var newAuthor = new Author
                {
                    UserId = user.Id,
                    WorkId = work.Id,
                    AuthorRoleId = authorRole.Id,
                    PurposeId = purpose.Id,
                    SCImagoFieldId = scImagoField?.Id,
                    FieldId = field?.Id,
                    Position = dto.Position,
                    ScoreLevel = dto.ScoreLevel,
                    ProofStatus = ProofStatus.ChuaXuLy
                };

                // 7) Tính workHour, authorHour (nếu cần)
                var factorRepo = _unitOfWork.Repository<Factor>();
                var factor = await factorRepo.FirstOrDefaultAsync(f =>
                    f.WorkTypeId == work.WorkTypeId &&
                    f.WorkLevelId == work.WorkLevelId &&
                    f.PurposeId == purpose.Id &&
                    (f.AuthorRoleId == null || f.AuthorRoleId == authorRole.Id) &&
                    f.ScoreLevel == newAuthor.ScoreLevel);

                if (factor is not null)
                {
                    int workHour = CalculateWorkHour(newAuthor.ScoreLevel, factor);
                    newAuthor.WorkHour = workHour;

                    int totalAuthors = work.TotalAuthors ?? 0;
                    int totalMainAuthors = work.TotalMainAuthors ?? 0;
                    decimal authorHour = await CalculateAuthorHour(workHour, totalAuthors, totalMainAuthors, authorRole.Id);
                    newAuthor.AuthorHour = authorHour;
                }
                else
                {
                    newAuthor.WorkHour = 0;
                    newAuthor.AuthorHour = 0;
                }

                await authorRepo.CreateAsync(newAuthor);
            }

            // Cuối cùng, lưu tất cả thay đổi
            await _unitOfWork.SaveChangesAsync();
        }

        private DateOnly? ParseMonthYear(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            // Thử parse các định dạng khác nhau
            string[] formats = { 
                "MM/yyyy", "M/yyyy", "yyyy-MM", "MM-yyyy", "M-yyyy",
                "d/M/yyyy", "dd/MM/yyyy", "d-M-yyyy", "dd-MM-yyyy",
                "yyyy/MM/dd", "yyyy-MM-dd"
            };

            foreach (var format in formats)
            {
                if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                {
                    // Luôn trả về ngày 1 của tháng đó
                    return new DateOnly(result.Year, result.Month, 1);
                }
            }

            // Thử parse bằng DateTime.Parse nếu các format trên không khớp
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return new DateOnly(parsedDate.Year, parsedDate.Month, 1);
            }

            return null;
        }

        private async Task CreateScientificArticleFromProjectAsync(Work project, Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                // Lấy nội dung sản phẩm từ details của đề tài
                var productDetails = project.Details?.GetValueOrDefault("Sản phẩm thuộc đề tài");
                if (string.IsNullOrEmpty(productDetails))
                {
                    _logger.LogWarning("Không tìm thấy thông tin sản phẩm trong details của đề tài {ProjectId}", project.Id);
                    return;
                }

                // Tạo công trình mới (Bài báo khoa học)
                var scientificArticle = new Work
                {
                    Id = Guid.NewGuid(),
                    Title = productDetails,
                    TimePublished = project.TimePublished,
                    TotalAuthors = project.TotalAuthors,
                    TotalMainAuthors = project.TotalMainAuthors,
                    Details = new Dictionary<string, string>
                    {
                        { "Tên tạp chí", "" },
                        { "Tập, số phát hành", "" },
                        { "Số trang", "" },
                        { "Chỉ số xuất bản", "" },
                        { "Cơ quản xuất bản", "" }
                    },
                    Source = WorkSource.NguoiDungKeKhai,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"), // ID của Bài báo khoa học
                    WorkLevelId = null,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.Repository<Work>().CreateAsync(scientificArticle);

                // Tạo thông tin tác giả
                var author = new Author
                {
                    Id = Guid.NewGuid(),
                    WorkId = scientificArticle.Id,
                    UserId = userId,
                    AuthorRoleId = null,
                    PurposeId = Guid.Parse("5cf30509-8632-4d62-ad14-55949b9b9336"), // ID của mục đích "Sản phẩm của đề tài NCKH"
                    Position = 1,
                    ScoreLevel = null,
                    WorkHour = 0,
                    SCImagoFieldId = null,
                    FieldId = null,
                    ProofStatus = ProofStatus.ChuaXuLy,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.Repository<Author>().CreateAsync(author);

                // Thêm tác giả vào WorkAuthor
                var workAuthor = new WorkAuthor
                {
                    Id = Guid.NewGuid(),
                    WorkId = scientificArticle.Id,
                    UserId = userId
                };
                await _unitOfWork.Repository<WorkAuthor>().CreateAsync(workAuthor);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Đã tạo bài báo khoa học tự động từ đề tài {ProjectId}", project.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo bài báo khoa học từ đề tài {ProjectId}", project.Id);
                throw;
            }
        }
    }
}