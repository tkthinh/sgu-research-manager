using Application.Caches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly ICacheManagementService _cacheService;

        public CacheController(ICacheManagementService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet("keys")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllKeys(CancellationToken cancellationToken)
        {
            var keys = await _cacheService.GetAllKeysAsync(cancellationToken);
            return Ok(new { success = true, message = "Lấy dữ liệu cache thành công", data = keys });
        }

        [HttpDelete("clear/{key}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ClearCache(string key, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return BadRequest(new { success = false, message = "Cache key không được để trống" });
            }

            await _cacheService.ClearCacheAsync(key, cancellationToken);
            return Ok(new { success = true, message = $"Đã xóa cache key: {key}" });
        }

        [HttpDelete("clear-all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ClearAllCache(CancellationToken cancellationToken)
        {
            await _cacheService.ClearAllCacheAsync(cancellationToken);
            return Ok(new { success = true, message = "Đã xóa tất cả cache key" });
        }
    }
}
