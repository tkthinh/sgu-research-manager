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
      }

      public virtual async Task<TDto> CreateAsync(TDto dto, CancellationToken cancellationToken = default)
      {
         var entity = mapper.MapToEntity(dto);
         entity.CreatedDate = DateTime.UtcNow;

         await unitOfWork.Repository<T>().CreateAsync(entity);
         await unitOfWork.SaveChangesAsync();

         // Clear cache after modification.
         await InvalidateCacheAsync();

         return mapper.MapToDto(entity);
      }

      public virtual async Task<IEnumerable<TDto>> GetAllAsync(CancellationToken cancellationToken = default)
      {
         string cacheKey = $"{cacheKeyPrefix}_all";
         string? cachedData = null;

         try
         {
            cachedData = await cache.GetStringAsync(cacheKey, cancellationToken);
         }
         catch (Exception ex)
         {
            logger.LogError(ex, "Error reading cache for key {CacheKey}", cacheKey);
         }

         if (!string.IsNullOrEmpty(cachedData))
         {
            return JsonSerializer.Deserialize<IEnumerable<TDto>>(cachedData)!;
         }

         var entities = await unitOfWork.Repository<T>().GetAllAsync();
         var dtos = mapper.MapToDtos(entities);

         try
         {
            await cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(dtos),
                new DistributedCacheEntryOptions
                {
                   AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                },
                cancellationToken);
         }
         catch (Exception ex)
         {
            logger.LogError(ex, "Error writing cache for key {CacheKey}", cacheKey);
         }

         return dtos;
      }

      public virtual async Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
      {
         string cacheKey = $"{cacheKeyPrefix}_{id}";
         string? cachedData = null;

         try
         {
            cachedData = await cache.GetStringAsync(cacheKey, cancellationToken);
         }
         catch (Exception ex)
         {
            logger.LogError(ex, "Error reading cache for key {CacheKey}", cacheKey);
         }

         if (!string.IsNullOrEmpty(cachedData))
         {
            return JsonSerializer.Deserialize<TDto>(cachedData);
         }

         var entity = await unitOfWork.Repository<T>().GetByIdAsync(id);
         if (entity == null)
            return default;

         var dto = mapper.MapToDto(entity);

         try
         {
            await cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(dto),
                new DistributedCacheEntryOptions
                {
                   AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                },
                cancellationToken);
         }
         catch (Exception ex)
         {
            logger.LogError(ex, "Error writing cache for key {CacheKey}", cacheKey);
         }

         return dto;
      }

      public virtual async Task UpdateAsync(TDto dto, CancellationToken cancellationToken = default)
      {
         var entity = mapper.MapToEntity(dto);
         entity.ModifiedDate = DateTime.UtcNow;

         await unitOfWork.Repository<T>().UpdateAsync(entity);
         await unitOfWork.SaveChangesAsync();

         await InvalidateCacheAsync(entity.Id);
      }

      public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
      {
         await unitOfWork.Repository<T>().DeleteAsync(id);
         await unitOfWork.SaveChangesAsync();

         await InvalidateCacheAsync(id);
      }

      private async Task InvalidateCacheAsync(Guid? id = null)
      {
         string allKey = $"{cacheKeyPrefix}_all";
         try
         {
            await cache.RemoveAsync(allKey);
         }
         catch (Exception ex)
         {
            logger.LogError(ex, "Error removing cache for key {CacheKey}", allKey);
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
               logger.LogError(ex, "Error removing cache for key {CacheKey}", idKey);
            }
         }
      }
   }
}
