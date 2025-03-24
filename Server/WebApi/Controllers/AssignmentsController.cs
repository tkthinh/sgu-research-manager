using Application.Assignments;
using Application.Shared.Response;
using Application.Users;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class AssignmentsController : ControllerBase
   {
      private readonly IAssignmentService assignmentService;
      private readonly IUserService userService;
      private readonly UserManager<ApplicationUser> userManager;
      private readonly ILogger<AssignmentsController> logger;

      public AssignmentsController(
         IAssignmentService assignmentService,
         IUserService userService,
         UserManager<ApplicationUser> userManager,
         ILogger<AssignmentsController> logger
         )
      {
         this.assignmentService = assignmentService;
         this.userService = userService;
         this.userManager = userManager;
         this.logger = logger;
      }

      [HttpGet]
      public async Task<ActionResult<ApiResponse<IEnumerable<AssignmentDto>>>> GetAssignments()
      {
         try
         {
            // Retrieve all manager users from Identity
            var managers = await userManager.GetUsersInRoleAsync("Manager");

            // Retrieve all assignment records
            var assignments = await assignmentService.GetAllAsync();

            var assignmentDtos = new List<AssignmentDto>();

            foreach (var manager in managers)
            {
               var managerInfo = await userService.GetUserByIdentityIdAsync(manager.Id);

               if (managerInfo is not null)
               {
                  var dto = new AssignmentDto
                  {
                     ManagerId = managerInfo.Id,
                     ManagerFullName = managerInfo.FullName,
                     ManagerDepartmentName = managerInfo.DepartmentName ?? "Unknown",
                     DepartmentId = Guid.Empty,
                     AssignedDepartmentName = "Chưa phân công"
                  };

                  var existingAssignment = assignments
                      .FirstOrDefault(a => a.ManagerId == managerInfo.Id);

                  if (existingAssignment != null)
                  {
                     // Map the assignment details if found
                     dto.Id = existingAssignment.Id;
                     dto.DepartmentId = existingAssignment.DepartmentId;
                     dto.AssignedDepartmentName = existingAssignment.AssignedDepartmentName ?? "Unknown";
                  }

                  assignmentDtos.Add(dto);
               }
            }
            // Return the DTO list instead of the raw assignments
            return Ok(new ApiResponse<IEnumerable<AssignmentDto>>(
                true,
                "Lấy dữ liệu phân công thành công",
                assignmentDtos
            ));
         }
         catch (Exception ex)
         {
            logger.LogError(ex, "Error fetching assignments");
            return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
         }
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
               ManagerId = request.ManagerId,
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
         catch (Exception ex)
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

            existingAssignment.ManagerId = request.ManagerId;
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
