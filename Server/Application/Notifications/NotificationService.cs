﻿using System.Text.Json;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace Application.Notifications
{
    public class NotificationService : GenericCachedService<NotificationDto, Notification>, INotificationService
    {
        public NotificationService(
           IUnitOfWork unitOfWork,
           IGenericMapper<NotificationDto, Notification> mapper,
           IDistributedCache cache,
           ILogger<NotificationService> logger
        ) : base(unitOfWork, mapper, cache, logger)
        {
        }

        public async Task CreateGlobalNotificationAsync(string content)
        {
            var dto = new NotificationDto
            {
                Content = content,
                IsGlobal = true,
            };

            await base.CreateAsync(dto);
        }

        public async Task<NotificationDto> CreateNotificationForUserAsync(Guid userId, string content)
        {
            var dto = new NotificationDto
            {
                Content = content,
                UserId = userId,
                IsGlobal = false,
                IsRead = false,
            };

            var entity = mapper.MapToEntity(dto);
            entity.CreatedDate = DateTime.UtcNow;

            await unitOfWork.Repository<Notification>().CreateAsync(entity);
            await unitOfWork.SaveChangesAsync();

            if (isCacheAvailable)
            {
                await cache.RemoveAsync($"{cacheKeyPrefix}_user_{userId}");
            }

            return mapper.MapToDto(entity);
        }

        public async Task<IEnumerable<NotificationDto>> GetGlobalNotificationsAsync()
        {
            if (isCacheAvailable)
            {
                string cacheKey = $"{cacheKeyPrefix}_global";
                string? cachedData = null;

                try
                {
                    cachedData = await cache.GetStringAsync(cacheKey);
                }
                catch (Exception ex)
                {
                    HandleCacheException(ex, $"Error reading cache for key {cacheKey}");
                }

                if (!string.IsNullOrEmpty(cachedData))
                {
                    try
                    {
                        return JsonSerializer.Deserialize<IEnumerable<NotificationDto>>(cachedData)!;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error deserializing cached data for key {CacheKey}", cacheKey);
                    }
                }
            }

            var entities = (await unitOfWork.Repository<Notification>()
                        .FindAsync(e => e.IsGlobal == true))
                        .OrderByDescending(e => e.CreatedDate);

            var dtos = mapper.MapToDtos(entities);

            if (isCacheAvailable)
            {
                await SafeSetCacheAsync(
                    $"{cacheKeyPrefix}_global",
                    JsonSerializer.Serialize(dtos),
                    TimeSpan.FromMinutes(60));
            }

            return dtos;
        }

        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(Guid userId)
        {
            if (isCacheAvailable)
            {
                string cacheKey = $"{cacheKeyPrefix}_user_{userId}";
                string? cachedData = null;

                try
                {
                    cachedData = await cache.GetStringAsync(cacheKey);
                }
                catch (Exception ex)
                {
                    HandleCacheException(ex, $"Error reading cache for key {cacheKey}");
                }

                if (!string.IsNullOrEmpty(cachedData))
                {
                    try
                    {
                        return JsonSerializer.Deserialize<IEnumerable<NotificationDto>>(cachedData)!;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error deserializing cached data for key {CacheKey}", cacheKey);
                    }
                }
            }

            var entities = await unitOfWork.Repository<Notification>().
                                    FindAsync(e => e.UserId == userId);

            var dtos = mapper.MapToDtos(entities);

            if (isCacheAvailable)
            {
                await SafeSetCacheAsync(
                    $"{cacheKeyPrefix}_user_{userId}",
                    JsonSerializer.Serialize(dtos),
                    TimeSpan.FromMinutes(5));
            }

            return dtos;
        }
        public async Task<NotificationDto> MarkNotificationAsReadAsync(Guid id)
        {
            var notification = await unitOfWork.Repository<Notification>().GetByIdAsync(id);
            if (notification == null)
            {
                throw new Exception($"Không tìm thấy thông báo.");
            }

            notification.IsRead = true;

            await unitOfWork.Repository<Notification>().UpdateAsync(notification);
            await unitOfWork.SaveChangesAsync();

            if (isCacheAvailable && notification.UserId.HasValue)
            {
                await cache.RemoveAsync($"{cacheKeyPrefix}_user_{notification.UserId}");
            }

            return mapper.MapToDto(notification);
        }
    }

}
