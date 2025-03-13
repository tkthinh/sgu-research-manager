using Application.Authors;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Domain.Enums;
using Application.SystemConfigs;
using System.Security.Claims;

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

        public WorkService(
            IUnitOfWork unitOfWork,
            IGenericMapper<WorkDto, Work> mapper,
            IGenericMapper<AuthorDto, Author> authorMapper,
            IDistributedCache cache,
            ILogger<WorkService> logger,
            IWorkRepository workRepository,
            ISystemConfigService systemConfigService)
            : base(unitOfWork, mapper, cache, logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authorMapper = authorMapper;
            _authorRoleRepository = unitOfWork.Repository<AuthorRole>();
            _factorRepository = unitOfWork.Repository<Factor>();
            _workRepository = workRepository;
            _systemConfigService = systemConfigService;
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
                FinalWorkHour = 0,
                ProofStatus = ProofStatus.ChuaXuLy,
                Note = request.Note,
                Details = request.Details,
                Source = request.Source,
                WorkTypeId = request.WorkTypeId,
                WorkLevelId = request.WorkLevelId,
                SCImagoFieldId = request.SCImagoFieldId,
                ScoringFieldId = request.ScoringFieldId,
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

            var tempWorkHour = CalculateTempWorkHour(request.Author.ScoreLevel, factor);

            await _unitOfWork.Repository<Work>().CreateAsync(work);

            var author = new Author
            {
                Id = Guid.NewGuid(),
                WorkId = work.Id,
                UserId = request.Author.UserId,
                AuthorRoleId = request.Author.AuthorRoleId,
                PurposeId = request.Author.PurposeId,
                Position = request.Author.Position,
                ScoreLevel = request.Author.ScoreLevel,
                CoAuthors = request.Author.CoAuthors,
                TempWorkHour = tempWorkHour,
                CreatedDate = DateTime.UtcNow
            };

            var tempAuthorHour = await CalculateTempAuthorHour(tempWorkHour, request.TotalAuthors ?? 0,
                request.TotalMainAuthors ?? 0, request.Author.AuthorRoleId);
            author.TempAuthorHour = tempAuthorHour;
            author.MarkedForScoring = false;

            await _unitOfWork.Repository<Author>().CreateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await SafeInvalidateCacheAsync(work.Id);

            return _mapper.MapToDto(work);
        }

        public async Task<WorkDto> AddCoAuthorAsync(Guid workId, AddCoAuthorRequestDto request, CancellationToken cancellationToken = default)
        {
            // Kiểm tra công trình có tồn tại không
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(workId);
            if (work == null)
                throw new Exception("Công trình không tồn tại");

            // Kiểm tra trạng thái hệ thống
            if (!await _systemConfigService.IsSystemOpenAsync(cancellationToken))
            {
                // Nếu hệ thống đóng, chỉ cho phép chỉnh sửa nếu ProofStatus là KhongHopLe
                if (work.ProofStatus != ProofStatus.KhongHopLe)
                    throw new Exception("Hệ thống đã đóng. Chỉ được chỉnh sửa công trình không hợp lệ.");

                // Xác minh userId là tác giả của công trình
                var isAuthor = await _unitOfWork.Repository<Author>()
                    .AnyAsync(a => a.WorkId == workId && a.UserId == request.AuthorRequest.UserId, cancellationToken);
                if (!isAuthor)
                    throw new Exception("Bạn không phải tác giả của công trình này.");
            }

            // Cập nhật thông tin công trình
            work.Title = request.WorkRequest.Title ?? work.Title;
            work.TimePublished = request.WorkRequest.TimePublished ?? work.TimePublished;
            work.TotalAuthors = request.WorkRequest.TotalAuthors ?? work.TotalAuthors;
            work.TotalMainAuthors = request.WorkRequest.TotalMainAuthors ?? work.TotalMainAuthors;
            work.Note = request.WorkRequest.Note ?? work.Note;
            work.Details = request.WorkRequest.Details ?? work.Details;
            work.Source = request.WorkRequest.Source;
            work.WorkTypeId = request.WorkRequest.WorkTypeId;
            work.WorkLevelId = request.WorkRequest.WorkLevelId ?? work.WorkLevelId;
            work.SCImagoFieldId = request.WorkRequest.SCImagoFieldId ?? work.SCImagoFieldId;
            work.ScoringFieldId = request.WorkRequest.ScoringFieldId ?? work.ScoringFieldId;
            work.ModifiedDate = DateTime.UtcNow;

            // Tính toán giờ làm việc tạm thời cho tác giả mới
            var factor = await _factorRepository.FirstOrDefaultAsync(f =>
                f.WorkTypeId == work.WorkTypeId &&
                f.WorkLevelId == work.WorkLevelId &&
                f.PurposeId == request.AuthorRequest.PurposeId &&
                f.ScoreLevel == request.AuthorRequest.ScoreLevel, cancellationToken);
            if (factor == null)
                throw new Exception("Không tìm thấy Factor phù hợp");

            var tempWorkHour = CalculateTempWorkHour(request.AuthorRequest.ScoreLevel, factor);

            // Tạo mới tác giả
            var author = new Author
            {
                Id = Guid.NewGuid(),
                WorkId = workId,
                UserId = request.AuthorRequest.UserId,
                AuthorRoleId = request.AuthorRequest.AuthorRoleId,
                PurposeId = request.AuthorRequest.PurposeId,
                Position = request.AuthorRequest.Position,
                ScoreLevel = request.AuthorRequest.ScoreLevel,
                CoAuthors = request.AuthorRequest.CoAuthors,
                TempWorkHour = tempWorkHour,
                CreatedDate = DateTime.UtcNow,
                TempAuthorHour = await CalculateTempAuthorHour(tempWorkHour, work.TotalAuthors ?? 0,
                    work.TotalMainAuthors ?? 0, request.AuthorRequest.AuthorRoleId),
                MarkedForScoring = false
            };

            // Lưu thay đổi vào database
            await _unitOfWork.Repository<Work>().UpdateAsync(work);
            await _unitOfWork.Repository<Author>().CreateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Added co-author {UserId} to work {WorkId}", request.AuthorRequest.UserId, workId);

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


        public async Task<WorkDto> UpdateWorkAdminAsync(Guid workId, AdminUpdateWorkRequestDto request, CancellationToken cancellationToken = default)
        {
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(workId);
            if (work == null)
                throw new Exception("Công trình không tồn tại");

            // Cập nhật finalWorkHour và proofStatus
            work.FinalWorkHour = request.FinalWorkHour;
            work.ProofStatus = request.ProofStatus; // Giữ nguyên nếu không cung cấp
            work.ModifiedDate = DateTime.UtcNow;

            // Tính toán lại finalAuthorHour cho tất cả tác giả
            var authors = await _unitOfWork.Repository<Author>().FindAsync(a => a.WorkId == workId);
            foreach (var author in authors)
            {
                var finalAuthorHour = await CalculateFinalAuthorHour(work.FinalWorkHour, work.TotalAuthors ?? 0,
                    work.TotalMainAuthors ?? 0, author.AuthorRoleId);
                author.FinalAuthorHour = finalAuthorHour;
                author.ModifiedDate = DateTime.UtcNow;
                await _unitOfWork.Repository<Author>().UpdateAsync(author);
            }

            await _unitOfWork.Repository<Work>().UpdateAsync(work);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await SafeInvalidateCacheAsync(workId);

            return _mapper.MapToDto(work);
        }

        public async Task DeleteWorkAsync(Guid workId, CancellationToken cancellationToken = default)
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

        private int CalculateTempWorkHour(ScoreLevel? scoreLevel, Factor factor)
        {
            return scoreLevel == factor.ScoreLevel ? factor.ConvertHour : 0;
        }

        private async Task<int> CalculateFinalAuthorHour(int finalWorkHour, int totalAuthors, int totalMainAuthors, Guid authorRoleId, CancellationToken cancellationToken = default)
        {
            if (totalAuthors == 0 || totalMainAuthors == 0)
                return 0;

            var authorRole = await _authorRoleRepository.GetByIdAsync(authorRoleId);
            if (authorRole == null)
                throw new Exception("Không tìm thấy vai trò tác giả");

            if (authorRole.IsMainAuthor)
                return (int)((1.0 / 3) * (finalWorkHour / totalMainAuthors) +
                             (2.0 / 3) * (finalWorkHour / totalAuthors));
            return (int)((2.0 / 3) * (finalWorkHour / totalAuthors));
        }

        private async Task<int> CalculateTempAuthorHour(int tempWorkHour, int totalAuthors, int totalMainAuthors, Guid authorRoleId, CancellationToken cancellationToken = default)
        {
            if (totalAuthors == 0 || totalMainAuthors == 0)
                return 0;

            var authorRole = await _authorRoleRepository.GetByIdAsync(authorRoleId);
            if (authorRole == null)
                throw new Exception("Không tìm thấy vai trò tác giả");

            if (authorRole.IsMainAuthor)
                return (int)((1.0 / 3) * (tempWorkHour / totalMainAuthors) +
                             (2.0 / 3) * (tempWorkHour / totalAuthors));
            return (int)((2.0 / 3) * (tempWorkHour / totalAuthors));
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

            if (!await _systemConfigService.IsSystemOpenAsync(cancellationToken))
            {
                if (work.ProofStatus != ProofStatus.KhongHopLe)
                    throw new Exception("Hệ thống đã đóng. Chỉ được chỉnh sửa công trình không hợp lệ.");
            }

            work.Title = request.WorkRequest.Title ?? work.Title;
            work.TimePublished = request.WorkRequest.TimePublished ?? work.TimePublished;
            work.TotalAuthors = request.WorkRequest.TotalAuthors ?? work.TotalAuthors;
            work.TotalMainAuthors = request.WorkRequest.TotalMainAuthors ?? work.TotalMainAuthors;
            work.Note = request.WorkRequest.Note ?? work.Note;
            work.Details = request.WorkRequest.Details ?? work.Details;
            work.Source = request.WorkRequest.Source;
            work.WorkTypeId = request.WorkRequest.WorkTypeId;
            work.WorkLevelId = request.WorkRequest.WorkLevelId ?? work.WorkLevelId;
            work.SCImagoFieldId = request.WorkRequest.SCImagoFieldId ?? work.SCImagoFieldId;
            work.ScoringFieldId = request.WorkRequest.ScoringFieldId ?? work.ScoringFieldId;
            work.ModifiedDate = DateTime.UtcNow;

            var author = await _unitOfWork.Repository<Author>()
                .FirstOrDefaultAsync(a => a.WorkId == workId && a.UserId == userId, cancellationToken);
            if (author == null)
                throw new Exception("Không tìm thấy thông tin tác giả trong công trình");

            author.Position = request.AuthorRequest.Position ?? author.Position;
            author.PurposeId = request.AuthorRequest.PurposeId ?? author.PurposeId;
            author.ScoreLevel = request.AuthorRequest.ScoreLevel ?? author.ScoreLevel;
            author.CoAuthors = request.AuthorRequest.CoAuthors ?? author.CoAuthors;

            await _unitOfWork.Repository<Work>().UpdateAsync(work);
            await _unitOfWork.Repository<Author>().UpdateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("User {UserId} updated work {WorkId} and author info", userId, workId);

            return _mapper.MapToDto(work);
        }

        public async Task<Guid> GetUserIdFromUserNameAsync(string userName)
        {
            var user = await _unitOfWork.Repository<User>()
                .FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                throw new Exception($"Không tìm thấy người dùng với UserName: {userName}");
            }
            return user.Id; // Giả định User có thuộc tính Id (Guid)
        }
    }
}