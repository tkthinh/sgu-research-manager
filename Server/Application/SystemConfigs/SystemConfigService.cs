using System.Data;
using System.Globalization;
using System.Text.Json;
using Application.Notifications;
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
        private readonly INotificationService notificationService;
        public SystemConfigService(
            IUnitOfWork unitOfWork,
            IGenericMapper<SystemConfigDto, SystemConfig> mapper,
            IDistributedCache cache,
            ILogger<SystemConfigService> logger,
            INotificationService notificationService
        )
        : base(unitOfWork, mapper, cache, logger)
        {
            this.notificationService = notificationService;
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

        public async Task<SystemConfigDto?> GetSystemState()
        {
            DateTime currentTime = DateTime.UtcNow;
            var isOpen = await IsSystemOpenAsync(currentTime);

            if (!isOpen)
            {
                return null;
            }
            else
            {
                var config = await unitOfWork.Repository<SystemConfig>()
                    .Include(sc => sc.AcademicYear)
                    .Where(sc => sc.OpenTime <= currentTime && sc.CloseTime >= currentTime
                        && !sc.IsDeleted)
                    .FirstOrDefaultAsync();

                return mapper.MapToDto(config);
            }
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

        public override Task UpdateAsync(SystemConfigDto dto, CancellationToken cancellationToken = default)
        {
            if(dto.IsNotified == true)
            {
                throw new Exception("Không thể cập nhật cấu hình hệ thống đã thông báo");
            }

            return base.UpdateAsync(dto, cancellationToken);
        }

        public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var systemConfig = await unitOfWork.Repository<SystemConfig>().GetByIdAsync(id);
            if (systemConfig != null)
            {
                if(systemConfig.IsNotified == true)
                {
                    throw new Exception("Không thể xóa cấu hình hệ thống đã thông báo");
                }
                systemConfig.IsDeleted = true;
                await unitOfWork.Repository<SystemConfig>().UpdateAsync(systemConfig);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                await SafeInvalidateCacheAsync(id, cancellationToken);
            }
            else
            {
                throw new Exception("Không tìm thấy cấu hình hệ thống");
            }
        }

        public async Task NotifySystemOpening(SystemConfigDto dto)
        {
            if (dto != null)
            {
                var notificationContent = $"{dto.Name}: Hệ thống sẽ mở vào lúc {dto.OpenTime.AddHours(7).ToString(new CultureInfo("vi-VN"))}" +
                                          $" và sẽ đóng vào lúc {dto.CloseTime.AddHours(7).ToString(new CultureInfo("vi-VN"))}.";
                await notificationService.CreateGlobalNotificationAsync(notificationContent);

                dto.IsNotified = true;
                await unitOfWork.Repository<SystemConfig>().UpdateAsync(mapper.MapToEntity(dto));
                await unitOfWork.SaveChangesAsync();
            }

        }
    }
}