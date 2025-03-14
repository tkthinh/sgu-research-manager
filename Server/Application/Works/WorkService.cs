using Application.Authors;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Domain.Enums;
using Application.SystemConfigs;
using Microsoft.AspNetCore.Http;

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

            return _mapper.MapToDtos(works);
        }

        public async Task<WorkDto?> GetWorkByIdWithAuthorsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(id);
            if (work == null)
                return null;
            return _mapper.MapToDto(work);
        }

        public async Task<IEnumerable<WorkDto>> SearchWorksAsync(string title, CancellationToken cancellationToken = default)
        {
            var works = await _workRepository.FindWorksWithAuthorsAsync(title);
            return _mapper.MapToDtos(works);
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
                Source = request.Source,
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
                ScoringFieldId = request.Author.ScoringFieldId,
                ProofStatus = ProofStatus.ChuaXuLy,
                CreatedDate = DateTime.UtcNow
            };

            var authorHour = await CalculateAuthorHour(workHour, request.TotalAuthors ?? 0,
                request.TotalMainAuthors ?? 0, request.Author.AuthorRoleId);
            author.AuthorHour = authorHour; 
            author.MarkedForScoring = false;

            // Lưu thông tin đồng tác giả vào WorkAuthor
            foreach (var coAuthorUserId in request.CoAuthorUserIds)
            {
                var workAuthor = new WorkAuthor
                {
                    Id = Guid.NewGuid(),
                    WorkId = work.Id,
                    UserId = coAuthorUserId
                };
                await _unitOfWork.Repository<WorkAuthor>().CreateAsync(workAuthor);
            }



            await _unitOfWork.Repository<Author>().CreateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await SafeInvalidateCacheAsync(work.Id);

            return _mapper.MapToDto(work);
        }

        public async Task<WorkDto> AddCoAuthorAsync(Guid workId, AddCoAuthorRequestDto request, Guid userId, CancellationToken cancellationToken = default)
        {
            // Kiểm tra công trình có tồn tại không
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(workId);
            if (work == null)
                throw new Exception("Công trình không tồn tại");

            // Kiểm tra người dùng hiện tại có phải là tác giả của công trình không
            var isAuthor = await _unitOfWork.Repository<Author>()
                .AnyAsync(a => a.WorkId == workId && a.UserId == userId, cancellationToken);
            if (!isAuthor)
                throw new UnauthorizedAccessException("Bạn không phải tác giả của công trình này");

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
                work.Source = request.WorkRequest.Source != default ? request.WorkRequest.Source : work.Source;
                work.WorkTypeId = request.WorkRequest.WorkTypeId ?? work.WorkTypeId;
                work.WorkLevelId = request.WorkRequest.WorkLevelId ?? work.WorkLevelId;
                work.ModifiedDate = DateTime.UtcNow;
            }

            // Tính toán giờ làm việc cho tác giả mới
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

            // Tạo mới tác giả
            var author = new Author
            {
                Id = Guid.NewGuid(),
                WorkId = workId,
                UserId = userId,
                AuthorRoleId = request.AuthorRequest.AuthorRoleId,
                PurposeId = request.AuthorRequest.PurposeId,
                SCImagoFieldId = request.AuthorRequest.SCImagoFieldId,
                ScoringFieldId = request.AuthorRequest.ScoringFieldId,
                Position = request.AuthorRequest.Position,
                ScoreLevel = request.AuthorRequest.ScoreLevel,
                WorkHour = workHour,
                AuthorHour = authorHour,
                MarkedForScoring = false,
                ProofStatus = ProofStatus.ChuaXuLy, // Mặc định khi tạo mới
                CreatedDate = DateTime.UtcNow
            };

            // Lưu thay đổi vào database
            await _unitOfWork.Repository<Work>().UpdateAsync(work);
            await _unitOfWork.Repository<Author>().CreateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Log và invalidate cache
            logger.LogInformation("User {UserId} added as co-author to work {WorkId}", userId, workId);
            await SafeInvalidateCacheAsync(workId);

            return _mapper.MapToDto(work);
        }

        public async Task<IEnumerable<AuthorDto>> GetWorksByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var authors = await _unitOfWork.Repository<Author>()
                .FindAsync(a => a.UserId == userId);
            return _authorMapper.MapToDtos(authors);
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
                work.Source = request.WorkRequest.Source != default ? request.WorkRequest.Source : work.Source;
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
                author.ScoringFieldId = request.AuthorRequest.ScoringFieldId ?? author.ScoringFieldId;

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

            var isAuthor = await _unitOfWork.Repository<Author>()
                .AnyAsync(a => a.WorkId == workId && a.UserId == userId, cancellationToken);
            if (!isAuthor)
                throw new UnauthorizedAccessException("Bạn không phải tác giả của công trình này");

            var author = await _unitOfWork.Repository<Author>()
                .FirstOrDefaultAsync(a => a.WorkId == workId && a.UserId == userId, cancellationToken);

            if (!await _systemConfigService.IsSystemOpenAsync(cancellationToken))
            {
                if (author == null || author.ProofStatus != ProofStatus.KhongHopLe)
                    throw new Exception("Hệ thống đã đóng. Chỉ được chỉnh sửa công trình không hợp lệ.");
            }

            if (author == null)
                throw new Exception("Không tìm thấy thông tin tác giả trong công trình");

            work.Title = request.WorkRequest?.Title ?? work.Title;
            work.TimePublished = request.WorkRequest?.TimePublished ?? work.TimePublished;
            work.TotalAuthors = request.WorkRequest?.TotalAuthors ?? work.TotalAuthors;
            work.TotalMainAuthors = request.WorkRequest?.TotalMainAuthors ?? work.TotalMainAuthors;
            work.Details = request.WorkRequest?.Details ?? work.Details;
            work.Source = request.WorkRequest?.Source ?? work.Source;
            work.WorkTypeId = request.WorkRequest?.WorkTypeId ?? work.WorkTypeId;
            work.WorkLevelId = request.WorkRequest?.WorkLevelId ?? work.WorkLevelId;
            work.ModifiedDate = DateTime.UtcNow;

            if (request.AuthorRequest != null)
            {
                author.AuthorRoleId = request.AuthorRequest.AuthorRoleId ?? author.AuthorRoleId;
                author.PurposeId = request.AuthorRequest.PurposeId ?? author.PurposeId;
                author.Position = request.AuthorRequest.Position ?? author.Position;
                author.ScoreLevel = request.AuthorRequest.ScoreLevel ?? author.ScoreLevel;
                author.SCImagoFieldId = request.AuthorRequest.SCImagoFieldId ?? author.SCImagoFieldId;
                author.ScoringFieldId = request.AuthorRequest.ScoringFieldId ?? author.ScoringFieldId;

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

            await _unitOfWork.Repository<Work>().UpdateAsync(work);
            await _unitOfWork.Repository<Author>().UpdateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("User {UserId} updated work {WorkId} by author", userId, workId);
            await SafeInvalidateCacheAsync(workId);

            return _mapper.MapToDto(work);
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            // Lấy danh sách các Author liên quan đến userId
            var authors = await _unitOfWork.Repository<Author>()
                .FindAsync(a => a.UserId == userId);

            if (!authors.Any())
            {
                return Enumerable.Empty<WorkDto>();
            }

            // Lấy danh sách WorkId từ các Author
            var workIds = authors.Select(a => a.WorkId).Distinct();

            // Lấy thông tin đầy đủ của các công trình
            var works = await _workRepository.GetWorksWithAuthorsByIdsAsync(workIds, cancellationToken);

            // Lọc để chỉ giữ thông tin tác giả của user hiện tại trong mỗi công trình
            var filteredWorks = works.Select(work =>
            {
                var userAuthor = authors.FirstOrDefault(a => a.WorkId == work.Id);
                if (userAuthor != null)
                {
                    // Tạo một bản sao của work với Authors chỉ chứa thông tin của user hiện tại
                    var filteredWork = _mapper.MapToDto(work);
                    filteredWork.Authors = new List<AuthorDto> { _authorMapper.MapToDto(userAuthor) };
                    return filteredWork;
                }
                return null;
            }).Where(w => w != null);

            return filteredWorks;
        }

        private int CalculateWorkHour(ScoreLevel? scoreLevel, Factor factor)
        {
            return scoreLevel == factor.ScoreLevel ? factor.ConvertHour : 0;
        }

        private async Task<int> CalculateAuthorHour(int workHour, int totalAuthors, int totalMainAuthors, Guid authorRoleId, CancellationToken cancellationToken = default)
        {
            if (totalAuthors == 0 || totalMainAuthors == 0)
                return 0;

            var authorRole = await _authorRoleRepository.GetByIdAsync(authorRoleId);
            if (authorRole == null)
                throw new Exception("Không tìm thấy vai trò tác giả");

            if (authorRole.IsMainAuthor)
                return (int)((1.0 / 3) * (workHour / totalMainAuthors) +
                             (2.0 / 3) * (workHour / totalAuthors));
            return (int)((2.0 / 3) * (workHour / totalAuthors));
        }

        public async Task DeleteWorkAsync(Guid workId, Guid userId, CancellationToken cancellationToken = default)
        {
            // Kiểm tra công trình có tồn tại không
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(workId);
            if (work == null)
                throw new Exception("Công trình không tồn tại");

            // Kiểm tra trạng thái hệ thống
            if (!await _systemConfigService.IsSystemOpenAsync(cancellationToken))
                throw new Exception("Hệ thống đã đóng. Không thể xóa công trình");

            // Xóa các tác giả liên quan
            var authors = await _unitOfWork.Repository<Author>().FindAsync(a => a.WorkId == workId);
            foreach (var author in authors)
            {
                await _unitOfWork.Repository<Author>().DeleteAsync(author.Id); // Truyền ID thay vì đối tượng
            }

            // Xóa công trình
            await _unitOfWork.Repository<Work>().DeleteAsync(work.Id); // Truyền ID thay vì đối tượng
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Invalidate cache
            await SafeInvalidateCacheAsync(workId);
        }
    }
}