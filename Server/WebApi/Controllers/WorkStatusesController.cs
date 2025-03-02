using Application.WorkStatuses;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkStatusesController : ControllerBase
    {
        private readonly IWorkStatusService workStatusService;
        private readonly ILogger<WorkStatusesController> logger;

        public WorkStatusesController(IWorkStatusService workStatusService, ILogger<WorkStatusesController> logger)
        {
            this.workStatusService = workStatusService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkStatusDto>>>> GetWorkStatuses()
        {
            var statuses = await workStatusService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<WorkStatusDto>>(
                true,
                "Lấy dữ liệu trạng thái công trình thành công",
                statuses
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WorkStatusDto>>> GetWorkStatus([FromRoute] Guid id)
        {
            var status = await workStatusService.GetByIdAsync(id);
            if (status is null)
            {
                return NotFound(new ApiResponse<WorkStatusDto>(false, "Không tìm thấy trạng thái công trình"));
            }
            return Ok(new ApiResponse<WorkStatusDto>(
                true,
                "Lấy dữ liệu trạng thái công trình thành công",
                status
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<WorkStatusDto>>> CreateWorkStatus([FromBody] CreateWorkStatusRequestDto requestDto)
        {
            try
            {
                var statusDto = new WorkStatusDto
                {
                    Name = requestDto.Name,
                    WorkTypeId = requestDto.WorkTypeId, 
                };

                var status = await workStatusService.CreateAsync(statusDto);
                var response = new ApiResponse<WorkStatusDto>(
                    true,
                    "Tạo trạng thái công việc thành công",
                    status
                );

                return CreatedAtAction(nameof(GetWorkStatus), new { id = status.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error creating work status");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateWorkStatus([FromRoute] Guid id, [FromBody] UpdateWorkStatusRequestDto request)
        {
            try
            {
                var existingStatus = await workStatusService.GetByIdAsync(id);
                if (existingStatus is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy trạng thái công trình"));
                }

                existingStatus.Name = request.Name;
                await workStatusService.UpdateAsync(existingStatus);

                return Ok(new ApiResponse<object>(true, "Cập nhật trạng thái công trình thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error updating work status");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteWorkStatus([FromRoute] Guid id)
        {
            try
            {
                var existingStatus = await workStatusService.GetByIdAsync(id);
                if (existingStatus is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy trạng thái công trình"));
                }

                await workStatusService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa trạng thái công trình thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting work status");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
