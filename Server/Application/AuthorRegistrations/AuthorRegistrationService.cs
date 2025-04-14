using Application.AcademicYears;
using Application.Shared.Services;
using Application.SystemConfigs;
using Application.Works;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.AuthorRegistrations
{
    public class AuthorRegistrationService : GenericCachedService<AuthorRegistrationDto, AuthorRegistration>, IAuthorRegistrationService
    {
        private readonly ISystemConfigService systemConfigService;
        private readonly IAcademicYearService academicYearService;
        private readonly IWorkService workService;

        public AuthorRegistrationService(
            IUnitOfWork unitOfWork,
            IGenericMapper<AuthorRegistrationDto, AuthorRegistration> mapper,
            IDistributedCache cache,
            ILogger<AuthorRegistrationService> logger,
            ISystemConfigService systemConfigService,
            IAcademicYearService academicYearService,
            IWorkService workService
        ) : base(unitOfWork, mapper, cache, logger)
        {
            this.systemConfigService = systemConfigService;
            this.academicYearService = academicYearService;
            this.workService = workService;
        }

        public async Task<IEnumerable<AuthorRegistrationDto>> RegisterAuthorsForCurrentYear(List<Guid> authorIds, CancellationToken cancellationToken = default)
        {
            var currentAcademicYear = await academicYearService.GetCurrentAcademicYear(cancellationToken);
            var authors = await unitOfWork.Repository<Author>()
                .FindAsync(a => authorIds.Contains(a.Id));

            var authorRegistrations = new List<AuthorRegistration>();
            foreach (var author in authors)
            {
                bool isRegistable = await IsAuthorRegistrable(author.Id, currentAcademicYear.Id);
                if (!isRegistable)
                {
                    continue;
                }

                var authorRegistration = new AuthorRegistration
                {
                    AuthorId = author.Id,
                    AcademicYearId = currentAcademicYear.Id,
                };
                authorRegistrations.Add(authorRegistration);
                await unitOfWork.Repository<AuthorRegistration>().CreateAsync(authorRegistration);
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return mapper.MapToDtos(authorRegistrations);
        }

        private async Task<bool> IsAuthorRegistrable(Guid authorId, Guid academicYearId)
        {
            // Check if registration record exists for the given academic year.
            var authorRegistration = await unitOfWork.Repository<AuthorRegistration>()
                .FirstOrDefaultAsync(a => a.AuthorId == authorId && a.AcademicYearId == academicYearId);

            // If there's already a registration record for this academic year, they're NOT registrable.
            if (authorRegistration != null)
            {
                return false;
            }

            // Since no registration record exists, get the Author and check the work's deadline.
            var author = await unitOfWork.Repository<Author>()
                .FirstOrDefaultAsync(a => a.Id == authorId);

            // Not registrable if Work or its ExchangeDeadline are null.
            if (author?.Work?.ExchangeDeadline == null)
            {
                return false;
            }

            // Compare directly as DateOnly to avoid conversion issues.
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            return author.Work.ExchangeDeadline.Value <= currentDate;
        }
    }
}
