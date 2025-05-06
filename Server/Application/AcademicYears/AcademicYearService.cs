using System.Text.Json;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.AcademicYears
{
    public class AcademicYearService : GenericCachedService<AcademicYearDto, AcademicYear>, IAcademicYearService
    {
        protected override TimeSpan defaultCacheTime => TimeSpan.FromHours(24);

        public AcademicYearService(
            IUnitOfWork unitOfWork,
            IGenericMapper<AcademicYearDto, AcademicYear> mapper,
            IDistributedCache cache,
            ILogger<AcademicYearService> logger
        )
        : base(unitOfWork, mapper, cache, logger)
        {
        }

        public async Task<AcademicYearDto> GetCurrentAcademicYear(CancellationToken cancellationToken = default)
        {
            if (isCacheAvailable)
            {
                string cacheKey = $"{cacheKeyPrefix}_current";
                string? cachedData = null;

                try
                {
                    cachedData = await cache.GetStringAsync(cacheKey, cancellationToken);
                }
                catch (Exception ex)
                {
                    HandleCacheException(ex, $"Error reading cache for key {cacheKey}");
                }

                if (!string.IsNullOrEmpty(cachedData))
                {
                    try
                    {
                        return JsonSerializer.Deserialize<AcademicYearDto>(cachedData)!;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error deserializing cached data for key {CacheKey}", cacheKey);
                    }
                }
            }

            // If cache is unavailable or empty, get from database
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

            var currentAcademicYear = await unitOfWork.Repository<AcademicYear>().FirstOrDefaultAsync(
                x => x.StartDate <= today && x.EndDate >= today, cancellationToken);

            var dto = mapper.MapToDto(currentAcademicYear);

            // Only try to cache if it was available
            if (isCacheAvailable && dto != null)
            {
                await SafeSetCacheAsync(
                    $"{cacheKeyPrefix}_current",
                    JsonSerializer.Serialize(dto),
                    defaultCacheTime,
                    cancellationToken);
            }

            return dto;
        }

    }
}

