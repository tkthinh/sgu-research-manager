using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.AcademicYears
{
    public class AcademicYearService : GenericCachedService<AcademicYearDto, AcademicYear>, IAcademicYearService
    {
        public AcademicYearService(
            IUnitOfWork unitOfWork,
            IGenericMapper<AcademicYearDto, AcademicYear> mapper,
            IDistributedCache cache,
            ILogger<AcademicYearService> logger
        )
        : base(unitOfWork, mapper, cache, logger)
        {
        }

        public async Task<AcademicYearDto> GetCurrentAcademicYear()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

            var currentAcademicYear = await unitOfWork.Repository<AcademicYear>().FirstOrDefaultAsync(
                x => x.StartDate <= today && x.EndDate >= today);

            return mapper.MapToDto(currentAcademicYear);
        }

    }
}

