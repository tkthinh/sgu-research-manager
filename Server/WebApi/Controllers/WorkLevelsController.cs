using Application.Shared.Response;
using Application.WorkLevels;
using Application.WorkTypes;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkLevelsController : ControllerBase
    {
        private readonly IWorkLevelService workLevelService;
        private readonly ILogger<WorkLevelsController> logger;

        public WorkLevelsController(IWorkLevelService workLevelService, ILogger<WorkLevelsController> logger)
        {
            this.workLevelService = workLevelService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkLevelDto>>>> GetWorkLevels()
        {
            var workLevels = await workLevelService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<WorkLevelDto>>(
                true,
                "Lấy dữ liệu cấp công trình thành công",
                workLevels
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WorkLevelDto>>> GetWorkLevel([FromRoute] Guid id)
        {
            var workLevel = await workLevelService.GetByIdAsync(id);
            if (workLevel is null)
            {
                return NotFound(new ApiResponse<WorkLevelDto>(
                    false,
                    "Không tìm thấy cấp công trình"
                ));
            }
            return Ok(new ApiResponse<WorkLevelDto>(
                true,
                "Lấy dữ liệu cấp công trình thành công",
                workLevel
            ));
        }


        [HttpGet("by-worktype/{workTypeId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkLevelDto>>>> GetWorkLevelsByWorkTypeId([FromRoute] Guid workTypeId)
        {
            if (workTypeId == Guid.Empty)
            {
                return BadRequest(new ApiResponse<IEnumerable<WorkLevelDto>>(false, "WorkTypeId không hợp lệ"));
            }

            var workLevels = await workLevelService.GetWorkLevelsByWorkTypeIdAsync(workTypeId);
            if (workLevels == null || !workLevels.Any())
            {
                return Ok(new ApiResponse<IEnumerable<WorkLevelDto>>(
                    true,
                    "Không tìm thấy cấp công trình cho loại công trình này",
                    Enumerable.Empty<WorkLevelDto>() // Trả về danh sách rỗng thay vì null
                ));
            }

            return Ok(new ApiResponse<IEnumerable<WorkLevelDto>>(
                true,
                "Lấy dữ liệu cấp công trình theo loại công trình thành công",
                workLevels
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<WorkLevelDto>>> CreateWorkLevel([FromBody] CreateWorkLevelRequestDto requestDto)
        {
            try
            {
                var workLevelDto = new WorkLevelDto
                {
                    Name = requestDto.Name,
                    WorkTypeId = requestDto.WorkTypeId
                };

                var workLevel = await workLevelService.CreateAsync(workLevelDto);
                var response = new ApiResponse<WorkLevelDto>(
                    true,
                    "Tạo cấp công trình thành công",
                    workLevel
                );

                return CreatedAtAction(nameof(GetWorkLevel), new { id = workLevel.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Lỗi khi tạo cấp công trình");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateWorkLevel([FromRoute] Guid id, [FromBody] UpdateWorkLevelRequestDto request)
        {
            try
            {
                var existingWorkLevel = await workLevelService.GetByIdAsync(id);
                if (existingWorkLevel is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy cấp công trình"));
                }

                existingWorkLevel.Name = request.Name;
                existingWorkLevel.WorkTypeId = request.WorkTypeId;
                await workLevelService.UpdateAsync(existingWorkLevel);

                return Ok(new ApiResponse<object>(true, "Cập nhật cấp công trình"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Lỗi khi cập nhật cấp công trình");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteWorkLevel([FromRoute] Guid id)
        {
            try
            {
                var existingWorkLevel = await workLevelService.GetByIdAsync(id);
                if (existingWorkLevel is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy cấp công trình"));
                }

                await workLevelService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa cấp công trình thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Lỗi khi xóa cấp công trình");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
