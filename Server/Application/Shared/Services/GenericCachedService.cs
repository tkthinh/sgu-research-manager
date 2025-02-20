using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;

namespace Application.Shared.Services
{
   public abstract class GenericCachedService<TDto, T> : IGenericService<TDto> where T : BaseEntity
   {
      private readonly IUnitOfWork unitOfWork;
      private readonly IGenericMapper<TDto, T> mapper;
      private readonly IDistributedCache cache;
      private readonly string cacheKeyPrefix;

      public GenericCachedService(
         IUnitOfWork unitOfWork,
         IGenericMapper<TDto, T> mapper,
         IDistributedCache cache
         )
      {
         this.unitOfWork = unitOfWork;
         this.mapper = mapper;
         this.cache = cache;
         cacheKeyPrefix = typeof(T).Name.ToLower(); // Use entity name as cache key prefix
      }

      public virtual async Task<TDto> CreateAsync(TDto dto, CancellationToken cancellationToken = default)
      {
         var entity = mapper.MapToEntity(dto);
         entity.CreatedDate = DateTime.UtcNow;

         await unitOfWork.Repository<T>().CreateAsync(entity);
         await unitOfWork.SaveChangesAsync();

         // Clear cache after modification
         await InvalidateCacheAsync(); 

         return mapper.MapToDto(entity);
      }

      public virtual async Task<IEnumerable<TDto>> GetAllAsync(CancellationToken cancellationToken = default)
      {
         string cacheKey = $"{cacheKeyPrefix}_all";

         var cachedData = await cache.GetStringAsync(cacheKey, cancellationToken);
         if (cachedData != null)
         {
            return JsonSerializer.Deserialize<IEnumerable<TDto>>(cachedData)!;
         }

         var entities = await unitOfWork.Repository<T>().GetAllAsync();
         var dtos = mapper.MapToDtos(entities);

         await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dtos),
            new DistributedCacheEntryOptions
            {
               AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            },
            cancellationToken);

         return dtos;
      }

      public virtual async Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
      {
         string cacheKey = $"{cacheKeyPrefix}_{id}";

         var cachedData = await cache.GetStringAsync(cacheKey, cancellationToken);
         if (cachedData != null)
         {
            return JsonSerializer.Deserialize<TDto>(cachedData);
         }

         var entity = await unitOfWork.Repository<T>().GetByIdAsync(id);
         if (entity == null) return default;

         var dto = mapper.MapToDto(entity);
         await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dto),
            new DistributedCacheEntryOptions
            {
               AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            },
            cancellationToken);

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
         await cache.RemoveAsync($"{cacheKeyPrefix}_all");
         if (id != null)
         {
            await cache.RemoveAsync($"{cacheKeyPrefix}_{id}");
         }
      }
   }
}
