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

        public WorkService(
            IUnitOfWork unitOfWork,
            IGenericMapper<WorkDto, Work> mapper,
            IGenericMapper<AuthorDto, Author> authorMapper,
            IDistributedCache cache,
            ILogger<WorkService> logger,
            IWorkRepository workRepository,
            ISystemConfigService systemConfigService,
            IHttpContextAccessor httpContextAccessor)
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
        }

        public async Task<IEnumerable<WorkDto>> GetAllWorksWithAuthorsAsync(CancellationToken cancellationToken = default)
        {
            var works = await _workRepository.GetWorksWithAuthorsAsync();
            var workDtos = _mapper.MapToDtos(works).ToList();

            foreach (var workDto in workDtos)
            {
                await FillCoAuthorUserIdsAsync(workDto, cancellationToken);
            }

            return workDtos;
        }

        public async Task<WorkDto?> GetWorkByIdWithAuthorsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(id);
            if (work == null)
                return null;

            var dto = _mapper.MapToDto(work);
            await FillCoAuthorUserIdsAsync(dto, cancellationToken); // Điền CoAuthorUserIds
            return dto;
        }

        public async Task<WorkDto> CreateWorkWithAuthorAsync(CreateWorkRequestDto request, CancellationToken cancellationToken = default)
        {
            // Kiểm tra trạng thái hệ thống
            if (!await _systemConfigService.IsSystemOpenAsync(cancellationToken))
                throw new Exception("Hệ thống đã đóng. Không thể tạo công trình mới.");

            var existingWork = await _unitOfWork.Repository<Work>()
                .FirstOrDefaultAsync(w => w.Title == request.Title, cancellationToken);
            if (existingWork != null)
                throw new Exception("Công trình đã tồn tại");

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
                CreatedDate = DateTime.UtcNow
            };

            var factor = (await _factorRepository.FindAsync(f =>
                f.WorkTypeId == work.WorkTypeId &&
                f.WorkLevelId == work.WorkLevelId &&
                f.PurposeId == request.Author.PurposeId &&
                f.ScoreLevel == request.Author.ScoreLevel))
                .FirstOrDefault();
            if (factor == null)
                throw new Exception("Không tìm thấy Factor phù hợp");

            var workHour = CalculateWorkHour(request.Author.ScoreLevel, factor);

            await _unitOfWork.Repository<Work>().CreateAsync(work);

            // Lấy UserId
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new Exception("Không thể xác định UserId của người dùng hiện tại.");
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

            // Tạo danh sách đầy đủ tất cả UserId (bao gồm cả tác giả chính)
            var allAuthorUserIds = new List<Guid> { userId };
            allAuthorUserIds.AddRange(coAuthorUserIds);
            allAuthorUserIds = allAuthorUserIds.Distinct().ToList(); // Đảm bảo không trùng lặp

            var workDto = _mapper.MapToDto(work);
            workDto.CoAuthorUserIds = allAuthorUserIds; // Gán danh sách đầy đủ
            await SafeInvalidateCacheAsync(work.Id);

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
               if (userAuthor == null)
               {
                  return null;
               }
               if (userAuthor != null)
                {
                    var filteredWork = _mapper.MapToDto(work);
                    filteredWork.Authors = new List<AuthorDto> { _authorMapper.MapToDto(userAuthor) };
                    return filteredWork;
                }
                return null;
            }).Where(w => w != null).ToList();

            foreach (var workDto in filteredWorks)
            {
                await FillCoAuthorUserIdsAsync(workDto!, cancellationToken);
            }

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
            if (!await _systemConfigService.IsSystemOpenAsync(cancellationToken))
                throw new Exception("Hệ thống đã đóng. Không thể đánh dấu công trình.");

            var author = await _unitOfWork.Repository<Author>().GetByIdAsync(authorId);
            if (author == null)
                throw new Exception("Không tìm thấy tác giả");

            var work = await _unitOfWork.Repository<Work>().GetByIdAsync(author.WorkId);
            if (work == null)
                throw new Exception("Công trình không tồn tại");

            // Nếu đánh dấu công trình, kiểm tra giới hạn
            if (marked)
            {
                // Lấy giới hạn số lượng công trình được phép đánh dấu từ bảng Factor
                var maxAllowed = await GetMaxAllowedForScoring(work.WorkTypeId, work.WorkLevelId, author.ScoreLevel, cancellationToken);

                // Đếm số lượng công trình đã được đánh dấu của người dùng với cùng ScoreLevel
                var markedAuthors = await _unitOfWork.Repository<Author>()
                        .FindAsync(a => a.UserId == author.UserId &&
                                       a.MarkedForScoring &&
                                       a.ScoreLevel == author.ScoreLevel);

                if (markedAuthors.Count() >= maxAllowed)
                {
                    var workType = await _unitOfWork.Repository<WorkType>().GetByIdAsync(work.WorkTypeId);
                    var workLevel = work.WorkLevelId.HasValue
                        ? await _unitOfWork.Repository<WorkLevel>().GetByIdAsync(work.WorkLevelId.Value)
                        : null;

                    var workTypeInfo = workType?.Name ?? "Không xác định";
                    var workLevelInfo = workLevel?.Name ?? "Không xác định";
                    var scoreLevelInfo = author.ScoreLevel.HasValue ? author.ScoreLevel.ToString() : "Không xác định";

                    throw new Exception($"Đã vượt quá giới hạn quy đổi ({maxAllowed} công trình) cho loại công trình '{workTypeInfo}', cấp '{workLevelInfo}' với mức điểm {scoreLevelInfo}");
                }
            }

            // Cập nhật trạng thái đánh dấu
            author.MarkedForScoring = marked;
            author.ModifiedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<Author>().UpdateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SafeInvalidateCacheAsync(author.WorkId);
        }

        private async Task<int> GetMaxAllowedForScoring(Guid workTypeId, Guid? workLevelId, ScoreLevel? scoreLevel, CancellationToken cancellationToken = default)
        {
            // Tìm kiếm Factor phù hợp với các tiêu chí: WorkTypeId, WorkLevelId, ScoreLevel
            var factor = (await _factorRepository.FindAsync(f =>
                f.WorkTypeId == workTypeId &&
                f.WorkLevelId == workLevelId &&
                f.ScoreLevel == scoreLevel))
                .FirstOrDefault();

            // Nếu tìm thấy Factor và có giá trị MaxAllowed, sử dụng giá trị đó
            if (factor != null && factor.MaxAllowed.HasValue)
                return factor.MaxAllowed.Value;

            // Nếu không tìm thấy, có thể trả về giá trị mặc định (ví dụ: 1)
            return 1;
        }

        public async Task<WorkDto> UpdateWorkByAdminAsync(Guid workId, Guid userId, UpdateWorkByAdminRequestDto request, CancellationToken cancellationToken = default)
        {
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(workId);
            if (work == null) throw new Exception("Công trình không tồn tại");

            // Tìm Author tương ứng với UserId và WorkId
            var author = await _unitOfWork.Repository<Author>()
                .FirstOrDefaultAsync(a => a.WorkId == workId && a.UserId == userId, cancellationToken);
            if (author == null) throw new Exception("Không tìm thấy thông tin tác giả trong công trình này");

            // Cập nhật thông tin công trình (Work) nếu có WorkRequest
            if (request.WorkRequest != null)
            {
                work.Title = request.WorkRequest.Title ?? work.Title;
                work.TimePublished = request.WorkRequest.TimePublished ?? work.TimePublished;
                work.TotalAuthors = request.WorkRequest.TotalAuthors ?? work.TotalAuthors;
                work.TotalMainAuthors = request.WorkRequest.TotalMainAuthors ?? work.TotalMainAuthors;
                work.Details = request.WorkRequest.Details ?? work.Details;
                work.Source = request.WorkRequest.Source;
                work.WorkTypeId = request.WorkRequest.WorkTypeId ?? work.WorkTypeId;
                work.WorkLevelId = request.WorkRequest.WorkLevelId ?? work.WorkLevelId;
                work.ModifiedDate = DateTime.UtcNow;
            }

            // Cập nhật thông tin tác giả (Author) nếu có AuthorRequest
            if (request.AuthorRequest != null)
            {
                author.AuthorRoleId = request.AuthorRequest.AuthorRoleId ?? author.AuthorRoleId;
                author.PurposeId = request.AuthorRequest.PurposeId ?? author.PurposeId;
                author.Position = request.AuthorRequest.Position ?? author.Position;
                author.ScoreLevel = request.AuthorRequest.ScoreLevel ?? author.ScoreLevel;
                author.SCImagoFieldId = request.AuthorRequest.SCImagoFieldId ?? author.SCImagoFieldId;
                author.FieldId = request.AuthorRequest.FieldId ?? author.FieldId;

                // Tính lại WorkHour và AuthorHour nếu có thay đổi liên quan
                if (request.AuthorRequest.ScoreLevel.HasValue ||
                    (request.WorkRequest != null && (request.WorkRequest.TotalAuthors.HasValue || request.WorkRequest.TotalMainAuthors.HasValue)))
                {
                    var factor = (await _factorRepository.FindAsync(f =>
                        f.WorkTypeId == work.WorkTypeId &&
                        f.WorkLevelId == work.WorkLevelId &&
                        f.PurposeId == author.PurposeId &&
                        f.ScoreLevel == (request.AuthorRequest.ScoreLevel ?? author.ScoreLevel)))
                        .FirstOrDefault();
                    if (factor == null)
                        throw new Exception("Không tìm thấy Factor phù hợp");

                    author.WorkHour = CalculateWorkHour(request.AuthorRequest.ScoreLevel ?? author.ScoreLevel, factor);
                    author.AuthorHour = await CalculateAuthorHour(
                        author.WorkHour,
                        request.WorkRequest?.TotalAuthors ?? work.TotalAuthors ?? 0,
                        request.WorkRequest?.TotalMainAuthors ?? work.TotalMainAuthors ?? 0,
                        author.AuthorRoleId);
                }

                // Xử lý ProofStatus và Note
                if (request.AuthorRequest.ProofStatus.HasValue)
                {
                    if (request.AuthorRequest.ProofStatus == ProofStatus.HopLe)
                    {
                        author.ProofStatus = ProofStatus.HopLe;
                        author.Note = request.AuthorRequest.Note ?? author.Note;
                    }
                    else if (request.AuthorRequest.ProofStatus == ProofStatus.KhongHopLe)
                    {
                        author.ProofStatus = ProofStatus.KhongHopLe;
                        author.Note = request.AuthorRequest.Note ?? author.Note;
                    }
                    else
                    {
                        author.ProofStatus = request.AuthorRequest.ProofStatus.Value;
                        author.Note = request.AuthorRequest.Note ?? author.Note;
                    }
                }

                author.ModifiedDate = DateTime.UtcNow;
            }

            // Lưu thay đổi vào database
            await _unitOfWork.Repository<Work>().UpdateAsync(work);
            await _unitOfWork.Repository<Author>().UpdateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Invalidate cache
            await SafeInvalidateCacheAsync(workId);

            return _mapper.MapToDto(work);
        }

        public async Task<WorkDto> UpdateWorkByAuthorAsync(Guid workId, UpdateWorkByAuthorRequestDto request, Guid userId, CancellationToken cancellationToken = default)
        {
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(workId);
            if (work == null)
                throw new Exception("Công trình không tồn tại");

            // Kiểm tra trạng thái hệ thống
            var currentAuthor = await _unitOfWork.Repository<Author>()
                .FirstOrDefaultAsync(a => a.WorkId == workId && a.UserId == userId, cancellationToken);

            if (!await _systemConfigService.IsSystemOpenAsync(cancellationToken))
            {
                if (currentAuthor == null || currentAuthor.ProofStatus != ProofStatus.KhongHopLe)
                    throw new Exception("Hệ thống đã đóng. Chỉ được chỉnh sửa công trình không hợp lệ.");
            }

            // Cập nhật thông tin công trình
            if (request.WorkRequest != null)
            {
                work.Title = request.WorkRequest.Title ?? work.Title;
                work.TimePublished = request.WorkRequest.TimePublished ?? work.TimePublished;
                work.TotalAuthors = request.WorkRequest.TotalAuthors ?? work.TotalAuthors;
                work.TotalMainAuthors = request.WorkRequest.TotalMainAuthors ?? work.TotalMainAuthors;
                work.Details = request.WorkRequest.Details ?? work.Details;
                work.Source = WorkSource.NguoiDungKeKhai; // Luôn đặt nguồn là NguoiDungKeKhai
                work.WorkTypeId = request.WorkRequest.WorkTypeId ?? work.WorkTypeId;
                work.WorkLevelId = request.WorkRequest.WorkLevelId ?? work.WorkLevelId;
                work.ModifiedDate = DateTime.UtcNow;
            }

            // Kiểm tra xem user đã có thông tin tác giả chưa
            var author = await _unitOfWork.Repository<Author>()
                .FirstOrDefaultAsync(a => a.WorkId == workId && a.UserId == userId, cancellationToken);

            if (author == null)
            {
                // Kiểm tra AuthorRequest không null khi tạo mới
                if (request.AuthorRequest == null)
                {
                    throw new Exception("Thông tin tác giả là bắt buộc khi thêm mới");
                }

                // Tính toán WorkHour và AuthorHour cho tác giả mới
                var factor = await _factorRepository.FirstOrDefaultAsync(f =>
                    f.WorkTypeId == work.WorkTypeId &&
                    f.WorkLevelId == work.WorkLevelId &&
                    f.PurposeId == request.AuthorRequest.PurposeId &&
                    f.ScoreLevel == request.AuthorRequest.ScoreLevel, cancellationToken);
                if (factor == null)
                    throw new Exception("Không tìm thấy Factor phù hợp");

                var workHour = CalculateWorkHour(request.AuthorRequest.ScoreLevel, factor);
                var authorHour = await CalculateAuthorHour(
                    workHour,
                    work.TotalAuthors ?? 0,
                    work.TotalMainAuthors ?? 0,
                    request.AuthorRequest.AuthorRoleId);

                // Tạo mới thông tin tác giả
                author = new Author
                {
                    Id = Guid.NewGuid(),
                    WorkId = workId,
                    UserId = userId,
                    AuthorRoleId = request.AuthorRequest.AuthorRoleId,
                    PurposeId = request.AuthorRequest.PurposeId,
                    SCImagoFieldId = request.AuthorRequest.SCImagoFieldId,
                    FieldId = request.AuthorRequest.FieldId,
                    Position = request.AuthorRequest.Position,
                    ScoreLevel = request.AuthorRequest.ScoreLevel,
                    WorkHour = workHour,
                    AuthorHour = authorHour,
                    MarkedForScoring = false,
                    ProofStatus = ProofStatus.ChuaXuLy,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.Repository<Author>().CreateAsync(author);
            }
            else
            {
                // Cập nhật thông tin tác giả nếu có AuthorRequest
                if (request.AuthorRequest != null)
                {
                    author.AuthorRoleId = request.AuthorRequest.AuthorRoleId;
                    author.PurposeId = request.AuthorRequest.PurposeId;
                    author.Position = request.AuthorRequest.Position ?? author.Position;
                    author.ScoreLevel = request.AuthorRequest.ScoreLevel ?? author.ScoreLevel;
                    author.SCImagoFieldId = request.AuthorRequest.SCImagoFieldId ?? author.SCImagoFieldId;
                    author.FieldId = request.AuthorRequest.FieldId ?? author.FieldId;

                    // Tính lại WorkHour và AuthorHour nếu có thay đổi
                    if (request.AuthorRequest.ScoreLevel.HasValue ||
                        request.WorkRequest?.TotalAuthors.HasValue == true ||
                        request.WorkRequest?.TotalMainAuthors.HasValue == true)
                    {
                        var factor = (await _factorRepository.FindAsync(f =>
                            f.WorkTypeId == work.WorkTypeId &&
                            f.WorkLevelId == work.WorkLevelId &&
                            f.PurposeId == author.PurposeId &&
                            f.ScoreLevel == (request.AuthorRequest.ScoreLevel ?? author.ScoreLevel)))
                            .FirstOrDefault();
                        if (factor == null)
                            throw new Exception("Không tìm thấy Factor phù hợp");

                        author.WorkHour = CalculateWorkHour(request.AuthorRequest.ScoreLevel ?? author.ScoreLevel, factor);
                        author.AuthorHour = await CalculateAuthorHour(
                            author.WorkHour,
                            request.WorkRequest?.TotalAuthors ?? work.TotalAuthors ?? 0,
                            request.WorkRequest?.TotalMainAuthors ?? work.TotalMainAuthors ?? 0,
                            author.AuthorRoleId);
                    }

                author.ModifiedDate = DateTime.UtcNow;
                }

                await _unitOfWork.Repository<Author>().UpdateAsync(author);
            }

            // Lưu thay đổi vào database
            await _unitOfWork.Repository<Work>().UpdateAsync(work);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Log và invalidate cache
            logger.LogInformation("User {UserId} updated work {WorkId} by author", userId, workId);
            await SafeInvalidateCacheAsync(workId);

            var updatedWorkDto = _mapper.MapToDto(work);
            await FillCoAuthorUserIdsAsync(updatedWorkDto, cancellationToken); // Điền CoAuthorUserIds
            return updatedWorkDto;
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

            // Điền CoAuthorUserIds cho từng WorkDto
            foreach (var workDto in filteredWorks)
            {
                await FillCoAuthorUserIdsAsync(workDto, cancellationToken);
            }

            return filteredWorks;
        }

        private int CalculateWorkHour(ScoreLevel? scoreLevel, Factor factor)
        {
            return scoreLevel == factor.ScoreLevel ? factor.ConvertHour : 0;
        }

        private async Task<decimal> CalculateAuthorHour(int workHour, int totalAuthors, int totalMainAuthors, Guid authorRoleId, CancellationToken cancellationToken = default)
        {
            if (totalAuthors == 0 || totalMainAuthors == 0)
                return 0;

            var authorRole = await _authorRoleRepository.GetByIdAsync(authorRoleId);
            if (authorRole == null)
                throw new Exception("Không tìm thấy vai trò tác giả");

            decimal authorHour;
            if (authorRole.IsMainAuthor)
            {
                authorHour = (decimal)((1.0 / 3) * (workHour / totalMainAuthors) +
                                      (2.0 / 3) * (workHour / totalAuthors));
            }
            else
            {
                authorHour = (decimal)((2.0 / 3) * (workHour / totalAuthors));
            }

            // Làm tròn đến 1 chữ số thập phân
            return Math.Round(authorHour, 1);
        }

        public async Task DeleteWorkAsync(Guid workId, Guid userId, CancellationToken cancellationToken = default)
        {
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(workId);
            if (work == null)
                throw new Exception("Công trình không tồn tại");

            if (!await _systemConfigService.IsSystemOpenAsync(cancellationToken))
                throw new Exception("Hệ thống đã đóng. Không thể xóa công trình");

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
            // Lấy tất cả UserId từ Author
            var authors = await _unitOfWork.Repository<Author>()
                .FindAsync(a => a.WorkId == workDto.Id);
            var authorUserIds = authors.Select(a => a.UserId).ToList();

            // Lấy tất cả UserId từ WorkAuthor
            var workAuthors = await _unitOfWork.Repository<WorkAuthor>()
                .FindAsync(wa => wa.WorkId == workDto.Id);
            var coAuthorUserIds = workAuthors.Select(wa => wa.UserId).ToList();

            // Kết hợp và loại bỏ trùng lặp
            workDto.CoAuthorUserIds = authorUserIds.Union(coAuthorUserIds).Distinct().ToList();
        }

        public async Task<byte[]> ExportToExcelAsync(List<ExportExcelDto> exportData, CancellationToken cancellationToken = default)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Kê khai công trình");

                // Định dạng tiêu đề
                worksheet.Cells[1, 1].Value = "TRƯỜNG ĐẠI HỌC SÀI GÒN";
                worksheet.Cells[1, 1, 1, 10].Merge = true;
                worksheet.Cells[1, 12].Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                worksheet.Cells[1, 12, 1, 18].Merge = true;
                worksheet.Cells[2, 1].Value = "ĐƠN VỊ:";
                worksheet.Cells[2, 1, 2, 10].Merge = true;
                worksheet.Cells[2, 12].Value = "Độc lập - Tự do - Hạnh phúc";
                worksheet.Cells[2, 12, 2, 18].Merge = true;
                worksheet.Cells[3, 1].Value = "KÊ KHAI SẢN PHẨM, CÔNG TRÌNH CỦA HOẠT ĐỘNG KHOA HỌC CÔNG NGHỆ NĂM HỌC 2023-2024";
                worksheet.Cells[3, 1, 3, 18].Merge = true;

                // Phần I: Thông tin tác giả
                worksheet.Cells[5, 1].Value = "I. THÔNG TIN TÁC GIẢ";
                worksheet.Cells[5, 1, 5, 18].Merge = true;

                // Lấy thông tin cá nhân từ bản ghi đầu tiên (vì thông tin cá nhân giống nhau cho tất cả công trình)
                var userInfo = exportData.FirstOrDefault();
                if (userInfo == null)
                    throw new Exception("Không có dữ liệu để export");

                worksheet.Cells[6, 1].Value = "1. Họ và tên:";
                worksheet.Cells[6, 2].Value = userInfo.FullName;
                worksheet.Cells[6, 5].Value = "4. Học hàm/học vị:";
                worksheet.Cells[6, 6].Value = userInfo.AcademicTitle;
                worksheet.Cells[6, 9].Value = "7. Mã số viên chức:";
                worksheet.Cells[6, 10].Value = userInfo.UserName; // Dùng UserName làm mã số viên chức

                worksheet.Cells[7, 1].Value = "2. Ngành:";
                worksheet.Cells[7, 2].Value = userInfo.DepartmentName;
                worksheet.Cells[7, 5].Value = "5. Chuyên ngành:";
                //worksheet.Cells[7, 6].Value = userInfo.MajorName;
                worksheet.Cells[7, 9].Value = "8. Ngạch công chức:";
                worksheet.Cells[7, 10].Value = userInfo.OfficerRank;

                worksheet.Cells[8, 1].Value = "3. Số điện thoại:";
                //worksheet.Cells[8, 2].Value = userInfo.PhoneNumber;
                worksheet.Cells[8, 5].Value = "6. Email:";
                worksheet.Cells[8, 6].Value = userInfo.Email;

                // Phần II: Thông tin công trình
                worksheet.Cells[10, 1].Value = "II. THÔNG TIN CÔNG TRÌNH";
                worksheet.Cells[10, 1, 10, 18].Merge = true;

                // Tiêu đề bảng
                var headers = new[]
                {
            "STT", "Tên công trình", "Loại công trình", "Cấp công trình", "Thời điểm công bố/hoàn thành",
            "Tổng số tác giả", "Tổng số tác giả chính", "Vị trí của tác giả", "Vai trò tác giả", "Đồng tác giả",
            "Thông tin chi tiết của công trình", "Mục đích quy đổi", "Mức điểm của công trình", "Ngành theo SCImago",
            "Ngành tính điểm", "Giờ chuẩn quy đổi", "Trạng thái", "Ghi chú"
        };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[11, i + 1].Value = headers[i];
                }

                // Điền dữ liệu công trình
                for (int i = 0; i < exportData.Count; i++)
                {
                    var work = exportData[i];
                    worksheet.Cells[i + 12, 1].Value = i + 1;
                    worksheet.Cells[i + 12, 2].Value = work.Title;
                    worksheet.Cells[i + 12, 3].Value = work.WorkTypeName;
                    worksheet.Cells[i + 12, 4].Value = work.WorkLevelName;
                    worksheet.Cells[i + 12, 5].Value = work.TimePublished.HasValue ? work.TimePublished.Value.ToString("dd/MM/yyyy") : "Không xác định";
                    worksheet.Cells[i + 12, 6].Value = work.TotalAuthors;
                    worksheet.Cells[i + 12, 7].Value = work.TotalMainAuthors;
                    worksheet.Cells[i + 12, 8].Value = work.Position;
                    worksheet.Cells[i + 12, 9].Value = work.AuthorRoleName;
                    worksheet.Cells[i + 12, 10].Value = work.CoAuthorNames;
                    worksheet.Cells[i + 12, 11].Value = work.Details != null ? string.Join("; ", work.Details.Select(kv => $"{kv.Key}: {kv.Value}")) : "Không xác định";
                    worksheet.Cells[i + 12, 12].Value = work.PurposeName;
                    worksheet.Cells[i + 12, 13].Value = work.ScoreLevel;
                    worksheet.Cells[i + 12, 14].Value = work.SCImagoFieldName;
                    worksheet.Cells[i + 12, 15].Value = work.ScoringFieldName;
                    worksheet.Cells[i + 12, 16].Value = work.AuthorHour;
                    worksheet.Cells[i + 12, 17].Value = work.ProofStatus?.ToString();
                    worksheet.Cells[i + 12, 18].Value = work.Note;
                }

                // Phần III: Tổng số công trình
                int startRow = 12 + exportData.Count + 2;
                worksheet.Cells[startRow, 1].Value = "III. TỔNG SỐ CÔNG TRÌNH";
                worksheet.Cells[startRow, 1, startRow, 18].Merge = true;

                startRow += 1;
                worksheet.Cells[startRow, 1].Value = "Tôi xin chịu trách nhiệm về tính xác thực của những thông tin trong bản kê khai này.";
                worksheet.Cells[startRow, 1, startRow, 18].Merge = true;

                startRow += 1;
                worksheet.Cells[startRow, 1].Value = "TỔNG SỐ CÔNG TRÌNH";
                worksheet.Cells[startRow, 1, startRow, 18].Merge = true;

                startRow += 1;
                worksheet.Cells[startRow, 1].Value = "TT";
                worksheet.Cells[startRow, 2].Value = "Loại công trình";
                worksheet.Cells[startRow, 3].Value = "Số lượng";

                // Tính tổng số công trình theo loại
                var workTypeCounts = exportData
                    .GroupBy(w => w.WorkTypeName ?? "Khác")
                    .ToDictionary(g => g.Key, g => g.Count());

                var workTypes = new[] { "Bài báo khoa học", "Báo cáo khoa học", "Đề tài", "Giáo trình", "Sách", "Hội thảo, hội nghị", "Hướng dẫn SV NCKH", "Khác" };
                for (int i = 0; i < workTypes.Length; i++)
                {
                    worksheet.Cells[startRow + i + 1, 1].Value = i + 1;
                    worksheet.Cells[startRow + i + 1, 2].Value = workTypes[i];
                    worksheet.Cells[startRow + i + 1, 3].Value = workTypeCounts.ContainsKey(workTypes[i]) ? workTypeCounts[workTypes[i]] : 0;
                }

                startRow += workTypes.Length + 1;
                worksheet.Cells[startRow, 1].Value = "Tổng số sản phẩm:";
                worksheet.Cells[startRow, 2].Value = exportData.Count;

                // Định dạng cột
                worksheet.Cells.AutoFitColumns();

                // Trả về nội dung file Excel dưới dạng byte[]
                return package.GetAsByteArray();
            }
        }
        public async Task<List<ExportExcelDto>> GetExportExcelDataAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            // Lấy thông tin cá nhân của user
            var user = await _unitOfWork.Repository<User>()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
                throw new Exception("Không tìm thấy người dùng");

            // Lấy danh sách công trình của user
            var works = await GetWorksByCurrentUserAsync(userId, cancellationToken);

            // Lấy thông tin đồng tác giả
            var allCoAuthorUserIds = works.SelectMany(w => w.CoAuthorUserIds).Distinct().ToList();
            var coAuthors = await _unitOfWork.Repository<User>()
                .FindAsync(u => allCoAuthorUserIds.Contains(u.Id));

            // Ánh xạ dữ liệu vào ExportExcelDto
            var exportData = works.Select(w =>
            {
                var author = w.Authors?.FirstOrDefault();
                var coAuthorNames = string.Join(", ", w.CoAuthorUserIds
                    .Where(uid => uid != userId)
                    .Select(uid => coAuthors.FirstOrDefault(u => u.Id == uid)?.FullName ?? "Không xác định"));

                return new ExportExcelDto
                {
                    UserName = user.UserName ?? "Không xác định",
                    FullName = user.FullName ?? "Không xác định",
                    Email = user.Email ?? "Không xác định",
                    AcademicTitle = user.AcademicTitle.ToString() ?? "Không xác định", // Học hàm/học vị
                    OfficerRank = user.OfficerRank.ToString() ?? "Không xác định", // Ngạch công chức
                    DepartmentName = user.Department?.Name ?? "Không xác định",
                    FieldName = author?.FieldName ?? "Không xác định", // Ngành - Field
                    //MajorName = user.Major ?? "Không xác định", // Chuyên ngành
                    //PhoneNumber = user.PhoneNumber ?? "Không xác định",
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
            if (file == null || file.Length == 0)
                throw new ArgumentException("File không hợp lệ");

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
                            TimePublished = DateOnly.TryParseExact(worksheet.Cells[row, 7].Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date :
                           DateOnly.TryParseExact(worksheet.Cells[row, 7].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) ? date :
                           DateOnly.TryParseExact(worksheet.Cells[row, 7].Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) ? date : null,
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
                if (user == null) continue;

                // 2) Tìm WorkType, WorkLevel
                var workType = await workTypeRepo.FirstOrDefaultAsync(wt =>
                    wt.Name.ToLower() == dto.WorkTypeName.ToLower());
                if (workType == null) continue;

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
                DateOnly? timePublished = dto.TimePublished; // Có thể null

                var existingWork = await workRepo.FirstOrDefaultAsync(w =>
                    w.Title.ToLower() == dto.Title.ToLower() &&
                    w.WorkTypeId == workType.Id &&
                    // So sánh WorkLevelId
                    (workLevelId == null ? w.WorkLevelId == null : w.WorkLevelId == workLevelId) &&
                    // So sánh TimePublished
                    (timePublished == null ? w.TimePublished == null : w.TimePublished == timePublished)
                );


                Work work;
                if (existingWork == null)
                {
                    // => Chưa có công trình này, tạo mới
                    work = new Work
                    {
                        Title = dto.Title,
                        TimePublished = dto.TimePublished,
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
                if (authorRole == null) continue;

                var purpose = await purposeRepo.FirstOrDefaultAsync(p =>
                    p.Name.ToLower() == dto.PurposeName.ToLower());
                if (purpose == null) continue;

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
                if (existingAuthor != null)
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
                    MarkedForScoring = false,
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

                if (factor != null)
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
    }
}