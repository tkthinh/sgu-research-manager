using System.Text.Json;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.SystemConfigs
{
    public class SystemConfigService : GenericCachedService<SystemConfigDto, SystemConfig>, ISystemConfigService
    {
        public SystemConfigService(
            IUnitOfWork unitOfWork,
            IGenericMapper<SystemConfigDto, SystemConfig> mapper,
            IDistributedCache cache,
            ILogger<SystemConfigService> logger
        )
        : base(unitOfWork, mapper, cache, logger)
        {
        }

        public override async Task<IEnumerable<SystemConfigDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            if (isCacheAvailable)
            {
                string cacheKey = $"{cacheKeyPrefix}_all";
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
                        return JsonSerializer.Deserialize<IEnumerable<SystemConfigDto>>(cachedData)!;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error deserializing cached data for key {CacheKey}", cacheKey);
                    }
                }
            }

            // If cache is unavailable or empty, get from database
            var entities = await unitOfWork.Repository<SystemConfig>()
                .Include(sc => sc.AcademicYear)
                .ToListAsync(cancellationToken);

            var dtos = mapper.MapToDtos(entities);

            // Only try to cache if it was available
            if (isCacheAvailable)
            {
                await SafeSetCacheAsync(
                    $"{cacheKeyPrefix}_all",
                    JsonSerializer.Serialize(dtos),
                    TimeSpan.FromMinutes(30),
                    cancellationToken);
            }

            return dtos;
        }

        public async Task<IEnumerable<SystemConfigDto>> GetSystemConfigsOfYear(Guid academicYearId)
        {
            var configs = await unitOfWork.Repository<SystemConfig>()
                .Include(sc => sc.AcademicYear)
                .Where(sc => sc.AcademicYear.Id == academicYearId
                    && !sc.IsDeleted)
                .ToListAsync();
            return mapper.MapToDtos(configs);
        }

        public async Task<bool> IsSystemOpenAsync(DateTime time, CancellationToken cancellationToken = default)
        {
            var config = await unitOfWork.Repository<SystemConfig>()
                .FirstOrDefaultAsync(
                    c => c.OpenTime <= time && c.CloseTime >= time
                        && !c.IsDeleted,
                    cancellationToken
                    );

            return config != null;
        }

        public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var systemConfig = await unitOfWork.Repository<SystemConfig>().GetByIdAsync(id);
            if (systemConfig != null)
            {
                systemConfig.IsDeleted = true;
                await unitOfWork.Repository<SystemConfig>().UpdateAsync(systemConfig);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                await SafeInvalidateCacheAsync(id, cancellationToken);
            }
            else
            {
                throw new Exception("System config not found");
            }
        }
    }
}