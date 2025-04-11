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
        private readonly ISystemConfigService systemConfigService;
        private readonly ILogger<SystemConfigsController> logger;

        public SystemConfigsController(
            ISystemConfigService systemConfigService,
            ILogger<SystemConfigsController> logger)
        {
            this.systemConfigService = systemConfigService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SystemConfigDto>>>> GetSystemConfigs()
        {
            try
            {
                var configs = await systemConfigService.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<SystemConfigDto>>(
                    true,
                    "Lấy cấu hình hệ thống thành công",
                    configs
                ));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting system configuration");
                return BadRequest(new ApiResponse<SystemConfigDto>(false, "Có lỗi xảy ra khi lấy cấu hình hệ thống: " + ex.Message));
            }
        }

        [HttpGet("check")]
        public async Task<ActionResult<ApiResponse<SystemConfigDto>>> CheckSystemConfig()
        {
            try
            {
                var systemState = await systemConfigService.GetSystemState();

                if (systemState == null)
                {
                    return Ok(new ApiResponse<object>(false, "Hệ thống đang đóng", null));
                }

                return Ok(new ApiResponse<SystemConfigDto>(
                    true,
                    "Hệ thống đang mở",
                    systemState
                ));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking system configuration");
                return BadRequest(new ApiResponse<bool>(false, "Có lỗi xảy ra khi lấy trạng thái cấu hình hệ thống: " + ex.Message));
            }
        }

        [HttpGet("year/{academicYearId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SystemConfigDto>>>> GetSystemConfigsOfYear([FromRoute] Guid academicYearId)
        {
            try
            {
                var configs = await systemConfigService.GetSystemConfigsOfYear(academicYearId);
                if(configs == null || !configs.Any())
                {
                    return NotFound(new ApiResponse<IEnumerable<SystemConfigDto>>(false, "Không tìm thấy cấu hình hệ thống cho năm học này"));
                }

                return Ok(new ApiResponse<IEnumerable<SystemConfigDto>>(
                    true,
                    "Lấy cấu hình hệ thống thành công",
                    configs
                ));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting system configuration");
                return BadRequest(new ApiResponse<SystemConfigDto>(false, "Có lỗi xảy ra khi lấy cấu hình hệ thống: " + ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<SystemConfigDto>>> CreateSystemConfig([FromBody] CreateSystemConfigRequestDto request)
        {
            try
            {
                var systemConfigDto = new SystemConfigDto
                {
                    Name = request.Name,
                    OpenTime = request.OpenTime,
                    CloseTime = request.CloseTime,
                    AcademicYearId = request.AcademicYearId
                };

                var systemConfig = await systemConfigService.CreateAsync(systemConfigDto);

                return Ok(new ApiResponse<SystemConfigDto>(
                    true,
                    "Tạo cấu hình hệ thống thành công",
                    systemConfig
                ));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating system configuration");
                return BadRequest(new ApiResponse<bool>(false, "Có lỗi xảy ra khi tạo cấu hình hệ thống: " + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateSystemConfig([FromRoute] Guid id, [FromBody] UpdateSystemConfigRequestDto request)
        {
            try
            {
                var existingSystemConfig = await systemConfigService.GetByIdAsync(id);
                if (existingSystemConfig is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy cấu hình hệ thống"));
                }

                existingSystemConfig.Name = request.Name;
                existingSystemConfig.OpenTime = request.OpenTime;
                existingSystemConfig.CloseTime = request.CloseTime;
                existingSystemConfig.AcademicYearId = request.AcademicYearId;

                await systemConfigService.UpdateAsync(existingSystemConfig);

                return Ok(new ApiResponse<object>(true, "Cập nhật cấu hình hệ thống thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating system configuration");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra khi cập nhật cấu hình hệ thống: " + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteSystemConfig([FromRoute] Guid id)
        {
            try
            {
                var existingSystemConfig = await systemConfigService.GetByIdAsync(id);
                if (existingSystemConfig is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy cấu hình hệ thống"));
                }

                await systemConfigService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa cấu hình hệ thống thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting system configuration");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra khi xóa cấu hình hệ thống: " + ex.Message));
            }
        }
    }
}