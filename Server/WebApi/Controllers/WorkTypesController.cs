using Application.WorkTypes;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Application.WorkLevels;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkTypesController : ControllerBase
    {
        private readonly IWorkTypeService workTypeService;
        private readonly ILogger<WorkTypesController> logger;

        public WorkTypesController(IWorkTypeService workTypeService, ILogger<WorkTypesController> logger)
        {
            this.workTypeService = workTypeService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkTypeDto>>>> GetWorkTypes()
        {
            var workTypes = await workTypeService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<WorkTypeDto>>(
                true,
                "Lấy dữ liệu loại công trình thành công",
                workTypes
            ));
        }

        [HttpGet("with-details")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkTypeWithDetailsCountDto>>>> GetWorkTypesWithDetailsCount()
        {
            var workTypes = await workTypeService.GetWorkTypesWithDetailsCountAsync();
            return Ok(new ApiResponse<IEnumerable<WorkTypeWithDetailsCountDto>>(
                true,
                "Lấy dữ liệu loại công trình với thông tin chi tiết thành công",
                workTypes
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WorkTypeDto>>> GetWorkType([FromRoute] Guid id)
        {
            var workType = await workTypeService.GetByIdAsync(id);
            if (workType is null)
            {
                return NotFound(new ApiResponse<WorkTypeDto>(false, "Không tìm thấy loại công trình"));
            }
            return Ok(new ApiResponse<WorkTypeDto>(
                true,
                "Lấy dữ liệu loại công trình thành công",
                workType
            ));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<WorkTypeDto>>> CreateWorkType([FromBody] CreateWorkTypeRequestDto requestDto)
        {
            try
            {
                var workTypeDto = new WorkTypeDto
                {
                    Name = requestDto.Name
                };

                var workType = await workTypeService.CreateAsync(workTypeDto);
                var response = new ApiResponse<WorkTypeDto>(
                    true,
                    "Tạo loại công trình thành công",
                    workType
                );

                return CreatedAtAction(nameof(GetWorkType), new { id = workType.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error creating work type");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateWorkType([FromRoute] Guid id, [FromBody] UpdateWorkTypeRequestDto request)
        {
            try
            {
                var existingWorkType = await workTypeService.GetByIdAsync(id);
                if (existingWorkType is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy loại công trình"));
                }

                existingWorkType.Name = request.Name;
                await workTypeService.UpdateAsync(existingWorkType);

                return Ok(new ApiResponse<object>(true, "Cập nhật loại công trình thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error updating work type");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteWorkType([FromRoute] Guid id)
        {
            try
            {
                var existingWorkType = await workTypeService.GetByIdAsync(id);
                if (existingWorkType is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy loại công trình"));
                }

                await workTypeService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa loại công trình thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting work type");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
