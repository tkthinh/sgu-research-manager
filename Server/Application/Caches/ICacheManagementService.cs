namespace Application.Caches
{
   public interface ICacheManagementService
   {
      Task<IEnumerable<string>> GetAllKeysAsync(CancellationToken cancellationToken = default);
      Task ClearCacheAsync(string key, CancellationToken cancellationToken = default);
      Task ClearAllCacheAsync(CancellationToken cancellationToken = default);
   }
}
