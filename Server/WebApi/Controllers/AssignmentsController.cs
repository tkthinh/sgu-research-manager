using Application.Assignments;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService assignmentService;
        private readonly ILogger<AssignmentsController> logger;

        public AssignmentsController(IAssignmentService assignmentService, ILogger<AssignmentsController> logger)
        {
            this.assignmentService = assignmentService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AssignmentDto>>>> GetAssignments()
        {
            var assignments = await assignmentService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<AssignmentDto>>(
                true,
                "Lấy dữ liệu phân công thành công",
                assignments
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AssignmentDto>>> GetAssignment([FromRoute] Guid id)
        {
            var assignment = await assignmentService.GetByIdAsync(id);
            if (assignment == null)
            {
                return NotFound(new ApiResponse<AssignmentDto>(false, "Không tìm thấy phân công"));
            }
            return Ok(new ApiResponse<AssignmentDto>(
                true,
                "Lấy dữ liệu phân công thành công",
                assignment
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<AssignmentDto>>> CreateAssignment([FromBody] CreateAssignmentRequestDto request)
        {
            try
            {
                var dto = new AssignmentDto
                {
                    UserId = request.UserId,
                    DepartmentId = request.DepartmentId
                };

                var assignment = await assignmentService.CreateAsync(dto);
                var response = new ApiResponse<AssignmentDto>(
                    true,
                    "Tạo phân công thành công",
                    assignment
                );

                return CreatedAtAction(nameof(GetAssignment), new { id = assignment.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error creating assignment");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateAssignment([FromRoute] Guid id, [FromBody] UpdateAssignmentRequestDto request)
        {
            try
            {
                var existingAssignment = await assignmentService.GetByIdAsync(id);
                if (existingAssignment == null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy phân công"));
                }

                existingAssignment.UserId = request.UserId;
                existingAssignment.DepartmentId = request.DepartmentId;
                await assignmentService.UpdateAsync(existingAssignment);

                return Ok(new ApiResponse<object>(true, "Cập nhật phân công thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error updating assignment");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteAssignment([FromRoute] Guid id)
        {
            try
            {
                var existingAssignment = await assignmentService.GetByIdAsync(id);
                if (existingAssignment == null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy phân công"));
                }

                await assignmentService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa phân công thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting assignment");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
