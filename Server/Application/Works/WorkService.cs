using Application.Authors;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Domain.Enums;
using Application.SystemConfigs;

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

            var existingWork = (await _unitOfWork.Repository<Work>()
                .FindAsync(w => w.Title == request.Title))
                .FirstOrDefault();
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

        public async Task<WorkDto?> CoAuthorDeclaredAsync(Guid workId, UpdateWorkRequestDto workRequest, CreateAuthorRequestDto authorRequest, CancellationToken cancellationToken = default)
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
                    .FindAsync(a => a.WorkId == workId && a.UserId == authorRequest.UserId) != null;
                if (!isAuthor)
                    throw new Exception("Bạn không phải tác giả của công trình này.");
            }

            // Cập nhật thông tin công trình
            work.Title = workRequest.Title ?? work.Title;
            work.TimePublished = workRequest.TimePublished ?? work.TimePublished;
            work.TotalAuthors = workRequest.TotalAuthors ?? work.TotalAuthors;
            work.TotalMainAuthors = workRequest.TotalMainAuthors ?? work.TotalMainAuthors;
            work.Note = workRequest.Note ?? work.Note;
            work.Details = workRequest.Details ?? work.Details;
            work.Source = workRequest.Source;
            work.WorkTypeId = workRequest.WorkTypeId;
            work.WorkLevelId = workRequest.WorkLevelId ?? work.WorkLevelId;
            work.SCImagoFieldId = workRequest.SCImagoFieldId ?? work.SCImagoFieldId;
            work.ScoringFieldId = workRequest.ScoringFieldId ?? work.ScoringFieldId;
            work.ModifiedDate = DateTime.UtcNow;

            // Tính toán giờ làm việc tạm thời cho tác giả mới
            var factor = (await _factorRepository.FindAsync(f =>
                f.WorkTypeId == work.WorkTypeId &&
                f.WorkLevelId == work.WorkLevelId &&
                f.PurposeId == authorRequest.PurposeId &&
                f.ScoreLevel == authorRequest.ScoreLevel))
                .FirstOrDefault();
            if (factor == null)
                throw new Exception("Không tìm thấy Factor phù hợp");

            var tempWorkHour = CalculateTempWorkHour(authorRequest.ScoreLevel, factor);

            // Tạo mới tác giả
            var author = new Author
            {
                Id = Guid.NewGuid(),
                WorkId = workId,
                UserId = authorRequest.UserId,
                AuthorRoleId = authorRequest.AuthorRoleId,
                PurposeId = authorRequest.PurposeId,
                Position = authorRequest.Position,
                ScoreLevel = authorRequest.ScoreLevel,
                CoAuthors = authorRequest.CoAuthors,
                TempWorkHour = tempWorkHour,
                CreatedDate = DateTime.UtcNow
            };

            var tempAuthorHour = await CalculateTempAuthorHour(tempWorkHour, work.TotalAuthors ?? 0,
                work.TotalMainAuthors ?? 0, author.AuthorRoleId);
            author.TempAuthorHour = tempAuthorHour;
            author.MarkedForScoring = false;

            // Lưu thay đổi vào database
            await _unitOfWork.Repository<Work>().UpdateAsync(work);
            await _unitOfWork.Repository<Author>().CreateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Invalidate cache
            await SafeInvalidateCacheAsync(workId);

            // Trả về thông tin công trình đã cập nhật kèm tác giả
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
                throw new Exception("Hệ thống đã đóng. Không thể tạo công trình mới.");

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
    }
}