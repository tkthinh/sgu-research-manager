using Application.Shared.Response;
using Application.SystemConfigs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")] // Chỉ admin có quyền truy cập
    public class SystemConfigsController : ControllerBase
    {
        private readonly ISystemConfigService _systemConfigService;
        private readonly ILogger<SystemConfigsController> _logger;

        public SystemConfigsController(
            ISystemConfigService systemConfigService,
            ILogger<SystemConfigsController> logger)
        {
            _systemConfigService = systemConfigService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<SystemConfigDto>>> GetSystemConfig()
        {
            try
            {
                var config = await _systemConfigService.GetSystemConfigAsync();
                return Ok(new ApiResponse<SystemConfigDto>(true, "Lấy cấu hình hệ thống thành công", config));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting system configuration");
                return BadRequest(new ApiResponse<SystemConfigDto>(false, "Có lỗi xảy ra khi lấy cấu hình hệ thống: " + ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<bool>>> CreateSystemConfig([FromBody] CreateSystemConfigRequestDto request)
        {
            try
            {
                await _systemConfigService.CreateSystemConfigAsync(request);
                return Ok(new ApiResponse<bool>(true, "Tạo cấu hình hệ thống thành công", true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating system configuration");
                return BadRequest(new ApiResponse<bool>(false, "Có lỗi xảy ra khi tạo cấu hình hệ thống: " + ex.Message));
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateSystemConfig([FromBody] UpdateSystemConfigRequestDto request)
        {
            try
            {
                await _systemConfigService.UpdateSystemConfigAsync(request);
                return Ok(new ApiResponse<bool>(true, "Cập nhật cấu hình hệ thống thành công", true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating system configuration");
                return BadRequest(new ApiResponse<bool>(false, "Có lỗi xảy ra khi cập nhật cấu hình hệ thống: " + ex.Message));
            }
        }
    }
}