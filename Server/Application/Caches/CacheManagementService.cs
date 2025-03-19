using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Application.Caches
{
   public class CacheManagementService : ICacheManagementService
   {
      private readonly IDistributedCache cache;
      private readonly ILogger<CacheManagementService> logger;
      private readonly ConnectionMultiplexer redis;

      public CacheManagementService(IDistributedCache cache, ILogger<CacheManagementService> logger, ConnectionMultiplexer redis)
      {
         this.cache = cache;
         this.logger = logger;
         this.redis = redis;
      }

      public async Task<IEnumerable<string>> GetAllKeysAsync(CancellationToken cancellationToken = default)
      {
         try
         {
            var server = redis.GetServer(redis.GetEndPoints().First());
            var keys = server.Keys(pattern: "*").Select(k => k.ToString());

            return await Task.FromResult(keys);
         }
         catch (Exception ex)
         {
            logger.LogError(ex, "Failed to retrieve cache keys");
            return Enumerable.Empty<string>();
         }
      }

      public async Task ClearCacheAsync(string key, CancellationToken cancellationToken = default)
      {
         if (string.IsNullOrEmpty(key))
         {
            logger.LogWarning("Attempted to clear cache with a null or empty key.");
            return;
         }

         try
         {
            await cache.RemoveAsync(key, cancellationToken);
            logger.LogInformation("Cache cleared for key: {Key}", key);
         }
         catch (Exception ex)
         {
            logger.LogError(ex, "Error clearing cache for key: {Key}", key);
         }
      }

      public async Task ClearAllCacheAsync(CancellationToken cancellationToken = default)
      {
         try
         {
            var server = redis.GetServer(redis.GetEndPoints().First());
            var keys = server.Keys(pattern: "*").Select(k => k.ToString()).ToList();

            foreach (var key in keys)
            {
               await cache.RemoveAsync(key, cancellationToken);
            }
            logger.LogInformation("All cache keys cleared.");
         }
         catch (Exception ex)
         {
            logger.LogError(ex, "Error clearing all cache keys.");
         }
      }
   }
}

