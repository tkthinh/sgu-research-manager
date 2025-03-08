using Application.Authors;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Works
{
    public class WorkService : GenericCachedService<WorkDto, Work>, IWorkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper<WorkDto, Work> _mapper;
        private readonly IGenericMapper<AuthorDto, Author> _authorMapper;
        private readonly IGenericRepository<AuthorRole> _authorRoleRepository;
        private readonly IGenericRepository<Factor> _factorRepository;

        public WorkService(
            IUnitOfWork unitOfWork,
            IGenericMapper<WorkDto, Work> mapper,
            IGenericMapper<AuthorDto, Author> authorMapper,
            IDistributedCache cache,
            ILogger<WorkService> logger)
            : base(unitOfWork, mapper, cache, logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authorMapper = authorMapper;
            _authorRoleRepository = unitOfWork.Repository<AuthorRole>();
            _factorRepository = unitOfWork.Repository<Factor>();
        }

        // Tìm kiếm công trình theo tiêu đề
        public async Task<IEnumerable<WorkDto>> SearchWorksAsync(string title, CancellationToken cancellationToken = default)
        {
            var works = await _unitOfWork.Repository<Work>()
                .FindAsync(w => w.Title.Contains(title), cancellationToken);
            return _mapper.MapToDtos(works);
        }

        // Tạo mới công trình và tác giả
        public async Task<WorkDto> CreateWorkWithAuthorAsync(CreateWorkRequestDto request, CancellationToken cancellationToken = default)
        {
            // Kiểm tra công trình đã tồn tại
            var existingWork = (await _unitOfWork.Repository<Work>()
                .FindAsync(w => w.Title == request.Title, cancellationToken))
                .FirstOrDefault();
            if (existingWork != null)
                throw new Exception("Công trình đã tồn tại");

            // Tạo mới Work
            var work = new Work
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                TimePublished = request.TimePublished,
                TotalAuthors = request.TotalAuthors,
                TotalMainAuthors = request.TotalMainAuthors,
                FinalWorkHour = 0, // Admin sẽ cập nhật sau
                Note = request.Note,
                Details = request.Details,
                Source = request.Source,
                WorkTypeId = request.WorkTypeId,
                WorkLevelId = request.WorkLevelId,
                SCImagoFieldId = request.SCImagoFieldId,
                ScoringFieldId = request.ScoringFieldId,
                ProofStatusId = request.ProofStatusId,
                CreatedDate = DateTime.UtcNow
            };

            // Tính TempWorkHour từ Factor
            var factor = (await _factorRepository.FindAsync(f =>
                f.WorkTypeId == work.WorkTypeId &&
                f.WorkLevelId == work.WorkLevelId &&
                f.PurposeId == request.Author.PurposeId, cancellationToken))
                .FirstOrDefault();
            if (factor == null)
                throw new Exception("Không tìm thấy Factor phù hợp");

            var tempWorkHour = CalculateTempWorkHour(request.Author.DeclaredScore, factor);

            await _unitOfWork.Repository<Work>().CreateAsync(work, cancellationToken);

            // Tạo mới Author
            var author = new Author
            {
                Id = Guid.NewGuid(),
                WorkId = work.Id,
                UserId = request.Author.UserId,
                AuthorRoleId = request.Author.AuthorRoleId,
                PurposeId = request.Author.PurposeId,
                Position = request.Author.Position,
                DeclaredScore = request.Author.DeclaredScore,
                CoAuthors = request.Author.CoAuthors,
                TempWorkHour = tempWorkHour,
                CreatedDate = DateTime.UtcNow
            };

            // Tính TempAuthorHour
            var tempAuthorHour = await CalculateTempAuthorHour(tempWorkHour, request.TotalAuthors ?? 0,
                request.TotalMainAuthors ?? 0, request.Author.AuthorRoleId, cancellationToken);
            author.TempAuthorHour = tempAuthorHour;
            author.IsNotMatch = tempAuthorHour != 0;
            author.MarkedForScoring = false;

            await _unitOfWork.Repository<Author>().CreateAsync(author, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await SafeInvalidateCacheAsync(work.Id, cancellationToken);

            return _mapper.MapToDto(work);
        }

        // Thêm tác giả vào công trình
        public async Task<AuthorDto> AddAuthorToWorkAsync(Guid workId, CreateAuthorRequestDto request, CancellationToken cancellationToken = default)
        {
            var work = await _unitOfWork.Repository<Work>().GetByIdAsync(workId, cancellationToken);
            if (work == null)
                throw new Exception("Công trình không tồn tại");

            // Tính TempWorkHour từ Factor
            var factor = (await _factorRepository.FindAsync(f =>
                f.WorkTypeId == work.WorkTypeId &&
                f.WorkLevelId == work.WorkLevelId &&
                f.PurposeId == request.PurposeId, cancellationToken))
                .FirstOrDefault();
            if (factor == null)
                throw new Exception("Không tìm thấy Factor phù hợp");

            var tempWorkHour = CalculateTempWorkHour(request.DeclaredScore, factor);

            // Tạo mới Author
            var author = new Author
            {
                Id = Guid.NewGuid(),
                WorkId = workId,
                UserId = request.UserId,
                AuthorRoleId = request.AuthorRoleId,
                PurposeId = request.PurposeId,
                Position = request.Position,
                DeclaredScore = request.DeclaredScore,
                CoAuthors = request.CoAuthors,
                TempWorkHour = tempWorkHour,
                CreatedDate = DateTime.UtcNow
            };

            // Tính TempAuthorHour
            var tempAuthorHour = await CalculateTempAuthorHour(tempWorkHour, work.TotalAuthors ?? 0,
                work.TotalMainAuthors ?? 0, request.AuthorRoleId, cancellationToken);
            author.TempAuthorHour = tempAuthorHour;
            author.IsNotMatch = tempAuthorHour != 0;
            author.MarkedForScoring = false;

            await _unitOfWork.Repository<Author>().CreateAsync(author, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await SafeInvalidateCacheAsync(workId, cancellationToken);

            return _authorMapper.MapToDto(author);
        }

        private int CalculateTempWorkHour(float declaredScore, Factor factor)
        {
            return Math.Abs(declaredScore - factor.Score) < 0.1 ? factor.ConvertHour : 0;
        }

        private async Task<int> CalculateTempAuthorHour(int tempWorkHour, int totalAuthors, int totalMainAuthors, Guid authorRoleId, CancellationToken cancellationToken = default)
        {
            if (totalAuthors == 0 || totalMainAuthors == 0)
                return 0;

            var authorRole = await _authorRoleRepository.GetByIdAsync(authorRoleId, cancellationToken);
            if (authorRole == null)
                throw new Exception("Không tìm thấy vai trò tác giả");

            if (authorRole.IsMainAuthor)
                return (int)((1.0 / 3) * (tempWorkHour / totalMainAuthors) +
                             (2.0 / 3) * (tempWorkHour / totalAuthors));
            return (int)((2.0 / 3) * (tempWorkHour / totalAuthors));
        }

        public async Task UpdateAuthorAsync(Guid authorId, AuthorDto authorDto, CancellationToken cancellationToken = default)
        {
            var author = await _unitOfWork.Repository<Author>().GetByIdAsync(authorId, cancellationToken);
            if (author == null)
                throw new Exception("Không tìm thấy tác giả");

            // Cập nhật thông tin (không cho phép thay đổi FinalAuthorHour trực tiếp)
            author.UserId = authorDto.UserId;
            author.AuthorRoleId = authorDto.AuthorRoleId;
            author.PurposeId = authorDto.PurposeId;
            author.Position = authorDto.Position;
            author.DeclaredScore = authorDto.DeclaredScore;
            author.CoAuthors = authorDto.CoAuthors;
            author.MarkedForScoring = authorDto.MarkedForScoring;
            author.IsNotMatch = author.TempAuthorHour != author.FinalAuthorHour;
            author.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.Repository<Author>().UpdateAsync(author, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SafeInvalidateCacheAsync(author.WorkId, cancellationToken);
        }

        // Lấy danh sách công trình của user
        public async Task<IEnumerable<AuthorDto>> GetWorksByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var authors = await _unitOfWork.Repository<Author>()
                .FindAsync(a => a.UserId == userId, cancellationToken);
            return _authorMapper.MapToDtos(authors);
        }

        // Cập nhật FinalAuthorHour khi admin chấm
        public async Task UpdateWorkFinalHourAsync(Guid workId, int finalWorkHour, CancellationToken cancellationToken = default)
        {
            var work = await _unitOfWork.Repository<Work>().GetByIdAsync(workId, cancellationToken);
            if (work == null)
                throw new Exception("Công trình không tồn tại");

            work.FinalWorkHour = finalWorkHour;
            await _unitOfWork.Repository<Work>().UpdateAsync(work, cancellationToken);

            // Cập nhật FinalAuthorHour cho tất cả tác giả của công trình
            var authors = await _unitOfWork.Repository<Author>()
                .FindAsync(a => a.WorkId == workId, cancellationToken);
            foreach (var author in authors)
            {
                var finalAuthorHour = await CalculateFinalAuthorHour(finalWorkHour, work.TotalAuthors ?? 0,
                    work.TotalMainAuthors ?? 0, author.AuthorRoleId, cancellationToken);
                author.FinalAuthorHour = finalAuthorHour;
                author.IsNotMatch = author.TempAuthorHour != finalAuthorHour;
                author.ModifiedDate = DateTime.UtcNow;
                await _unitOfWork.Repository<Author>().UpdateAsync(author, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SafeInvalidateCacheAsync(workId, cancellationToken);
        }

        // Kiểm tra và cập nhật MarkedForScoring
        public async Task SetMarkedForScoringAsync(Guid authorId, bool marked, CancellationToken cancellationToken = default)
        {
            var author = await _unitOfWork.Repository<Author>().GetByIdAsync(authorId, cancellationToken);
            if (author == null)
                throw new Exception("Không tìm thấy tác giả");

            var work = await _unitOfWork.Repository<Work>().GetByIdAsync(author.WorkId, cancellationToken);
            if (work == null)
                throw new Exception("Công trình không tồn tại");

            // Kiểm tra giới hạn dựa trên WorkType, WorkLevel, và DeclaredScore
            var maxAllowed = await GetMaxAllowedForScoring(work.WorkTypeId, work.WorkLevelId, author.DeclaredScore, cancellationToken);
            var markedAuthors = await _unitOfWork.Repository<Author>()
                .FindAsync(a => a.UserId == author.UserId && a.MarkedForScoring && a.DeclaredScore == author.DeclaredScore, cancellationToken);
            if (marked && markedAuthors.Count() >= maxAllowed)
                throw new Exception($"Đã vượt quá giới hạn quy đổi ({maxAllowed} công trình)");

            author.MarkedForScoring = marked;
            author.ModifiedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<Author>().UpdateAsync(author, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SafeInvalidateCacheAsync(author.WorkId, cancellationToken);
        }

        private async Task<int> CalculateFinalAuthorHour(int finalWorkHour, int totalAuthors, int totalMainAuthors, Guid authorRoleId, CancellationToken cancellationToken = default)
        {
            if (totalAuthors == 0 || totalMainAuthors == 0)
                return 0;

            var authorRole = await _authorRoleRepository.GetByIdAsync(authorRoleId, cancellationToken);
            if (authorRole == null)
                throw new Exception("Không tìm thấy vai trò tác giả");

            if (authorRole.IsMainAuthor)
                return (int)((1.0 / 3) * (finalWorkHour / totalMainAuthors) +
                             (2.0 / 3) * (finalWorkHour / totalAuthors));
            return (int)((2.0 / 3) * (finalWorkHour / totalAuthors));
        }

        private async Task<int> GetMaxAllowedForScoring(Guid workTypeId, Guid workLevelId, float declaredScore, CancellationToken cancellationToken = default)
        {
            // Logic giới hạn dựa trên WorkType, WorkLevel, và DeclaredScore
            var workType = await _unitOfWork.Repository<WorkType>().GetByIdAsync(workTypeId, cancellationToken);
            var workLevel = await _unitOfWork.Repository<WorkLevel>().GetByIdAsync(workLevelId, cancellationToken);

            if (workType?.Name == "Bài báo" && declaredScore == 1.0f)
                return 4;
            if (workType?.Name == "Bài báo" && declaredScore == 0.75f)
                return 2;
            return 1; // Giá trị mặc định
        }

        public async Task DeleteAuthorAsync(Guid authorId, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.Repository<Author>().DeleteAsync(authorId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var author = await _unitOfWork.Repository<Author>().GetByIdAsync(authorId, cancellationToken);
            if (author != null)
                await SafeInvalidateCacheAsync(author.WorkId, cancellationToken);
        }
    }
}