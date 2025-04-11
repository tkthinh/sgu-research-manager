using Application.AcademicYears;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Authors
{
    public class AuthorService : GenericCachedService<AuthorDto, Author>, IAuthorService
    {
        public AuthorService(
            IUnitOfWork unitOfWork,
            IGenericMapper<AuthorDto, Author> mapper,
            IDistributedCache cache,
            ILogger<AuthorService> logger
            )
            : base(unitOfWork, mapper, cache, logger)
        {
        }

        public async Task<IEnumerable<AuthorDto>> GetAllRegistableAuthorsOfUser(Guid userId, CancellationToken cancellationToken = default)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

            var authors = await unitOfWork.Repository<Author>()
                .Include(a => a.Work)
                .Include(a => a.AuthorRole)
                .Include(a => a.Purpose)
                .Include(a => a.SCImagoField)
                .Include(a => a.Field)
                .Where(a => a.UserId == userId &&
                    a.Work != null &&
                    a.Work.ExchangeDeadline.HasValue &&
                    currentDate <= a.Work.ExchangeDeadline.Value &&
                    (a.AuthorRegistration == null || a.AuthorRegistration.AcademicYearId == Guid.Empty)
                )
                .ToListAsync(cancellationToken);

            return mapper.MapToDtos(authors);
        }
    }
}
