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
            // Retrieve users in the "Manager" role
            var managers = await userManager.GetUsersInRoleAsync("Manager");
            var assignments = await assignmentService.GetAllAsync();

            var assignmentDtos = new List<AssignmentDto>();

            foreach (var manager in managers)
            {
               var managerInfo = await userService.GetUserByIdentityIdAsync(manager.Id);
               if (managerInfo is null)
                  continue;

               // Filter assignments for the current user (manager)
               var userAssignments = assignments.Where(a => a.ManagerId == managerInfo.Id).ToList();
               if (userAssignments.Count > 0)
               {
                  foreach (var assign in userAssignments)
                  {
                     assignmentDtos.Add(new AssignmentDto
                     {
                        Id = assign.Id,
                        ManagerId = managerInfo.Id,
                        ManagerFullName = managerInfo.FullName,
                        ManagerDepartmentName = managerInfo.DepartmentName ?? "Unknown",
                        DepartmentId = assign.DepartmentId,
                        AssignedDepartmentName = assign.AssignedDepartmentName ?? "Unknown"
                     });
                  }
               }
               else
               {
                  // If no assignments exist, add a placeholder
                  assignmentDtos.Add(new AssignmentDto
                  {
                     ManagerId = managerInfo.Id,
                     ManagerFullName = managerInfo.FullName,
                     ManagerDepartmentName = managerInfo.DepartmentName ?? "Unknown",
                     DepartmentId = Guid.Empty,
                     AssignedDepartmentName = "Chưa phân công"
                  });
               }
            }

            return Ok(new ApiResponse<IEnumerable<AssignmentDto>>(
                true,
                "Lấy dữ liệu danh sách phân công thành công",
                assignmentDtos
            ));
         }
         catch (Exception ex)
         {
            logger.LogError(ex, "Error fetching assignments");
            return BadRequest(new ApiResponse<object>(
                false,
                "Có lỗi xảy ra trong quá trình xử lý"
            ));
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
      public async Task<ActionResult<ApiResponse<object>>> CreateAssignment([FromBody] CreateAssignmentRequestDto request)
      {
         try
         {
            var assignment = await assignmentService.CreateAsync(request);
            var response = new ApiResponse<object>(
                true,
                "Đã tạo phân công",
                assignment
            );

            return CreatedAtAction(nameof(GetAssignment), new { id = assignment.First().ManagerId }, response);
         }
         catch (Exception ex)
         {
            logger.LogError(ex, "Error creating assignment");
            return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình xử lý"));
         }
      }

      [HttpPut()]
      public async Task<ActionResult<ApiResponse<object>>> UpdateAssignment([FromBody] UpdateAssignmentRequestDto request)
      {
         try
         {
            var result = await assignmentService.UpdateAsync(request);
            return Ok(new ApiResponse<object>(true, "Đã cập nhật phân công"));
         }
         catch (Exception ex)
         {
            logger.LogError(ex, "Error updating assignment");
            return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình xử lý"));
         }
      }

      [HttpGet("user/{userId}")]
      public async Task<ActionResult<ApiResponse<IEnumerable<AssignmentDto>>>> GetAllAssignmentsByUser(Guid userId)
      {
         try
         {
            var assignments = await assignmentService.GetAllAssignmentByUserAsync(userId);

            if (!assignments.Any())
            {
               return NoContent();
            }

            return Ok(new ApiResponse<IEnumerable<AssignmentDto>>(
                true,
                "Lấy dữ liệu phân công của quản lý thành công",
                assignments
            ));
         }
         catch (Exception ex)
         {
            logger.LogError(ex, "Error fetching assignments for user");
            return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình xử lý"));
         }
      }

      [HttpDelete("user/{userId}")]
      public async Task<ActionResult<ApiResponse<object>>> DeleteAllAssignmentsForUser([FromRoute] Guid userId)
      {
         try
         {
            await assignmentService.DeleteAllAssignmentsByUserIdAsync(userId);
            
            return Ok(new ApiResponse<object>(true, "Xóa phân công của quản lý thành công"));
         }
         catch (Exception ex)
         {
            logger.LogError(ex, "Error deleting assignments for user");
            return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình xử lý"));
         }
      }
   }
}
