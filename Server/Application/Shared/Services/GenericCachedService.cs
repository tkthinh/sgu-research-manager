using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Shared.Services
{
   public abstract class GenericCachedService<TDto, T> : IGenericService<TDto> where T : BaseEntity
   {
      protected readonly IUnitOfWork unitOfWork;
      protected readonly IGenericMapper<TDto, T> mapper;
      protected readonly IDistributedCache cache;
      protected readonly string cacheKeyPrefix;
      protected readonly ILogger logger;
      protected bool isCacheAvailable = true;

      public GenericCachedService(
          IUnitOfWork unitOfWork,
          IGenericMapper<TDto, T> mapper,
          IDistributedCache cache,
          ILogger logger
          )
      {
         this.unitOfWork = unitOfWork;
         this.mapper = mapper;
         this.cache = cache;
         this.logger = logger;
         cacheKeyPrefix = typeof(T).Name.ToLower();

         // Perform initial cache availability check
         CheckCacheAvailability();
      }

      public virtual async Task<TDto> CreateAsync(TDto dto, CancellationToken cancellationToken = default)
      {
         var entity = mapper.MapToEntity(dto);
         entity.CreatedDate = DateTime.UtcNow;

         await unitOfWork.Repository<T>().CreateAsync(entity);
         await unitOfWork.SaveChangesAsync();

         // Only try to clear cache if it was available
         await SafeInvalidateCacheAsync();

         return mapper.MapToDto(entity);
      }

      public virtual async Task<IEnumerable<TDto>> GetAllAsync(CancellationToken cancellationToken = default)
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
                  return JsonSerializer.Deserialize<IEnumerable<TDto>>(cachedData)!;
               }
               catch (Exception ex)
               {
                  logger.LogError(ex, "Error deserializing cached data for key {CacheKey}", cacheKey);
               }
            }
         }

         // If cache is unavailable or empty, get from database
         var entities = await unitOfWork.Repository<T>().GetAllAsync();
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

      public virtual async Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
      {
         if (isCacheAvailable)
         {
            string cacheKey = $"{cacheKeyPrefix}_{id}";
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
                  return JsonSerializer.Deserialize<TDto>(cachedData);
               }
               catch (Exception ex)
               {
                  logger.LogError(ex, "Error deserializing cached data for key {CacheKey}", cacheKey);
               }
            }
         }

         // If cache is unavailable or empty, get from database
         var entity = await unitOfWork.Repository<T>().GetByIdAsync(id);
         if (entity == null)
            return default;

         var dto = mapper.MapToDto(entity);

         // Only try to cache if it was available
         if (isCacheAvailable)
         {
            await SafeSetCacheAsync(
                $"{cacheKeyPrefix}_{id}",
                JsonSerializer.Serialize(dto),
                TimeSpan.FromMinutes(30),
                cancellationToken);
         }

         return dto;
      }

      public virtual async Task UpdateAsync(TDto dto, CancellationToken cancellationToken = default)
      {
         var entity = mapper.MapToEntity(dto);
         entity.ModifiedDate = DateTime.UtcNow;

         await unitOfWork.Repository<T>().UpdateAsync(entity);
         await unitOfWork.SaveChangesAsync();

         await SafeInvalidateCacheAsync(entity.Id);
      }

      public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
      {
         await unitOfWork.Repository<T>().DeleteAsync(id);
         await unitOfWork.SaveChangesAsync();

         await SafeInvalidateCacheAsync(id);
      }

      private void HandleCacheException(Exception ex, string message)
      {
         isCacheAvailable = false;
         logger.LogWarning(ex, $"{message} - Cache appears to be unavailable");
      }

      private async Task SafeSetCacheAsync(string key, string value, TimeSpan expiration, CancellationToken cancellationToken = default)
      {
         if (!isCacheAvailable) return;

         try
         {
            await cache.SetStringAsync(
                key,
                value,
                new DistributedCacheEntryOptions
                {
                   AbsoluteExpirationRelativeToNow = expiration
                },
                cancellationToken);
         }
         catch (Exception ex)
         {
            HandleCacheException(ex, $"Error writing cache for key {key}");
         }
      }

      protected async Task SafeInvalidateCacheAsync(Guid? id = null, CancellationToken cancellationToken = default)
      {
         if (!isCacheAvailable) return;

         string allKey = $"{cacheKeyPrefix}_all";
         try
         {
            await cache.RemoveAsync(allKey);
         }
         catch (Exception ex)
         {
            HandleCacheException(ex, $"Error removing cache for key {allKey}");
            return; // Don't try the second operation if the first fails
         }

         if (id != null)
         {
            string idKey = $"{cacheKeyPrefix}_{id}";
            try
            {
               await cache.RemoveAsync(idKey);
            }
            catch (Exception ex)
            {
               HandleCacheException(ex, $"Error removing cache for key {idKey}");
            }
         }
      }

      private async void CheckCacheAvailability()
      {
         try
         {
            await cache.GetStringAsync("__ping__");
            isCacheAvailable = true;
         }
         catch (Exception ex)
         {
            isCacheAvailable = false;
            logger.LogWarning(ex, "Cache unavailable - application will run without caching");
         }
      }
   }
}