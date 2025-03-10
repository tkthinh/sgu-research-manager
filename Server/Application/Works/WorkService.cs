using Application.Authors;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Domain.Enums;

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

        public async Task<IEnumerable<WorkDto>> SearchWorksAsync(string title, CancellationToken cancellationToken = default)
        {
            var works = await _unitOfWork.Repository<Work>()
                .FindAsync(w => w.Title.Contains(title));
            return _mapper.MapToDtos(works);
        }

        public async Task<WorkDto> CreateWorkWithAuthorAsync(CreateWorkRequestDto request, CancellationToken cancellationToken = default)
        {
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
                ProofStatus = request.ProofStatus,
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
            author.IsNotMatch = tempAuthorHour != 0;
            author.MarkedForScoring = false;

            await _unitOfWork.Repository<Author>().CreateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await SafeInvalidateCacheAsync(work.Id);

            return _mapper.MapToDto(work);
        }

        public async Task<AuthorDto> AddAuthorToWorkAsync(Guid workId, CreateAuthorRequestDto request, CancellationToken cancellationToken = default)
        {
            var work = await _unitOfWork.Repository<Work>().GetByIdAsync(workId);
            if (work == null)
                throw new Exception("Công trình không tồn tại");

            var factor = (await _factorRepository.FindAsync(f =>
                f.WorkTypeId == work.WorkTypeId &&
                f.WorkLevelId == work.WorkLevelId &&
                f.PurposeId == request.PurposeId &&
                f.ScoreLevel == request.ScoreLevel))
                .FirstOrDefault();
            if (factor == null)
                throw new Exception("Không tìm thấy Factor phù hợp");

            var tempWorkHour = CalculateTempWorkHour(request.ScoreLevel, factor);

            var author = new Author
            {
                Id = Guid.NewGuid(),
                WorkId = workId,
                UserId = request.UserId,
                AuthorRoleId = request.AuthorRoleId,
                PurposeId = request.PurposeId,
                Position = request.Position,
                ScoreLevel = request.ScoreLevel,
                CoAuthors = request.CoAuthors,
                TempWorkHour = tempWorkHour,
                CreatedDate = DateTime.UtcNow
            };

            var tempAuthorHour = await CalculateTempAuthorHour(tempWorkHour, work.TotalAuthors ?? 0,
                work.TotalMainAuthors ?? 0, request.AuthorRoleId);
            author.TempAuthorHour = tempAuthorHour;
            author.IsNotMatch = tempAuthorHour != 0;
            author.MarkedForScoring = false;

            await _unitOfWork.Repository<Author>().CreateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await SafeInvalidateCacheAsync(workId);

            return _authorMapper.MapToDto(author);
        }

        private int CalculateTempWorkHour(ScoreLevel? scoreLevel, Factor factor)
        {
            return scoreLevel == factor.ScoreLevel ? factor.ConvertHour : 0;
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

        public async Task UpdateAuthorAsync(Guid authorId, AuthorDto authorDto, CancellationToken cancellationToken = default)
        {
            var author = await _unitOfWork.Repository<Author>().GetByIdAsync(authorId);
            if (author == null)
                throw new Exception("Không tìm thấy tác giả");

            author.UserId = authorDto.UserId;
            author.AuthorRoleId = authorDto.AuthorRoleId;
            author.PurposeId = authorDto.PurposeId;
            author.Position = authorDto.Position;
            author.ScoreLevel = authorDto.ScoreLevel;
            author.CoAuthors = authorDto.CoAuthors;
            author.MarkedForScoring = authorDto.MarkedForScoring;
            author.IsNotMatch = author.TempAuthorHour != author.FinalAuthorHour;
            author.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.Repository<Author>().UpdateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SafeInvalidateCacheAsync(author.WorkId);
        }

        public async Task<IEnumerable<AuthorDto>> GetWorksByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var authors = await _unitOfWork.Repository<Author>()
                .FindAsync(a => a.UserId == userId);
            return _authorMapper.MapToDtos(authors);
        }

        public async Task UpdateWorkFinalHourAsync(Guid workId, int finalWorkHour, CancellationToken cancellationToken = default)
        {
            var work = await _unitOfWork.Repository<Work>().GetByIdAsync(workId);
            if (work == null)
                throw new Exception("Công trình không tồn tại");

            work.FinalWorkHour = finalWorkHour;
            await _unitOfWork.Repository<Work>().UpdateAsync(work);

            var authors = await _unitOfWork.Repository<Author>()
                .FindAsync(a => a.WorkId == workId);
            foreach (var author in authors)
            {
                var finalAuthorHour = await CalculateFinalAuthorHour(finalWorkHour, work.TotalAuthors ?? 0,
                    work.TotalMainAuthors ?? 0, author.AuthorRoleId);
                author.FinalAuthorHour = finalAuthorHour;
                author.IsNotMatch = author.TempAuthorHour != finalAuthorHour;
                author.ModifiedDate = DateTime.UtcNow;
                await _unitOfWork.Repository<Author>().UpdateAsync(author);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SafeInvalidateCacheAsync(workId);
        }

        public async Task SetMarkedForScoringAsync(Guid authorId, bool marked, CancellationToken cancellationToken = default)
        {
            var author = await _unitOfWork.Repository<Author>().GetByIdAsync(authorId);
            if (author == null)
                throw new Exception("Không tìm thấy tác giả");

            var work = await _unitOfWork.Repository<Work>().GetByIdAsync(author.WorkId);
            if (work == null)
                throw new Exception("Công trình không tồn tại");

            var maxAllowed = await GetMaxAllowedForScoring(work.WorkTypeId, work.WorkLevelId, author.ScoreLevel);
            var markedAuthors = await _unitOfWork.Repository<Author>()
                .FindAsync(a => a.UserId == author.UserId && a.MarkedForScoring && a.ScoreLevel == author.ScoreLevel);
            if (marked && markedAuthors.Count() >= maxAllowed)
                throw new Exception($"Đã vượt quá giới hạn quy đổi ({maxAllowed} công trình)");

            author.MarkedForScoring = marked;
            author.ModifiedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<Author>().UpdateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SafeInvalidateCacheAsync(author.WorkId);
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

        private async Task<int> GetMaxAllowedForScoring(Guid workTypeId, Guid? workLevelId, ScoreLevel? scoreLevel, CancellationToken cancellationToken = default)
        {
            var workType = await _unitOfWork.Repository<WorkType>().GetByIdAsync(workTypeId);
            if (workType == null)
                return 1; // Giá trị mặc định

            // Logic giới hạn dựa trên WorkType và ScoreLevel
            if (workType.Name == "Bài báo")
            {
                if (scoreLevel == ScoreLevel.One)
                    return 4;
                if (scoreLevel == ScoreLevel.ZeroPointSevenFive)
                    return 2;
            }
            return 1; // Giá trị mặc định
        }

        public async Task DeleteAuthorAsync(Guid authorId, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.Repository<Author>().DeleteAsync(authorId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var author = await _unitOfWork.Repository<Author>().GetByIdAsync(authorId);
            if (author != null)
                await SafeInvalidateCacheAsync(author.WorkId);
        }
    }
}