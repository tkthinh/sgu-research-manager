using Application.Departments;
using Application.Shared.Response;
using Application.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService departmentService;
        private readonly ILogger<DepartmentsController> logger;
        private readonly ICurrentUserService currentUserService;

        public DepartmentsController(
           IDepartmentService departmentService,
           ILogger<DepartmentsController> logger,
           ICurrentUserService currentUserService)
        {
            this.departmentService = departmentService;
            this.logger = logger;
            this.currentUserService = currentUserService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<DepartmentDto>>>> GetDepartments()
        {
            var departments = await departmentService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<DepartmentDto>>(
               true,
               "Lấy dữ liệu đơn vị công tác thành công",
               departments
            ));
        }

        [HttpGet("by-manager/{managerId}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DepartmentDto>>>> GetAssignedDepartmentsByManager([FromRoute] Guid managerId)
        {
            try
            {
                // Kiểm tra người dùng hiện tại
                var (isSuccess, currentUserId, _) = currentUserService.GetCurrentUser();
                if (!isSuccess)
                {
                    return Unauthorized(new ApiResponse<object>(false, "Người dùng chưa đăng nhập"));
                }

                // Kiểm tra xem managerId có phải là người dùng hiện tại không
                if (currentUserId != managerId)
                {
                    return Forbid();
                }

                var departments = await departmentService.GetDepartmentsByManagerIdAsync(managerId);
                return Ok(new ApiResponse<IEnumerable<DepartmentDto>>(
                   true,
                   "Lấy dữ liệu đơn vị công tác theo manager thành công",
                   departments
                ));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching departments by manager");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DepartmentDto>>> GetDepartment([FromRoute] Guid id)
        {
            var department = await departmentService.GetByIdAsync(id);
            if (department is null)
            {
                return NotFound(new ApiResponse<DepartmentDto>(false, "Không tìm thấy đơn vị công tác"));
            }
            return Ok(new ApiResponse<DepartmentDto>(
               true,
               "Lấy dữ liệu đơn vị công tác thành công",
               department
            ));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<DepartmentDto>>> CreateDepartment([FromBody] CreateDepartmentRequestDto requestDto)
        {
            try
            {
                var departmentDto = new DepartmentDto
                {
                    Name = requestDto.Name,
                };

                var department = await departmentService.CreateAsync(departmentDto);
                var response = new ApiResponse<DepartmentDto>(
                   true,
                   "Tạo đơn vị công tác thành công",
                   department
                );

                return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error creating department");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateDepartment([FromRoute] Guid id, [FromBody] UpdateDepartmentRequestDto request)
        {
            try
            {
                var existingDepartment = await departmentService.GetByIdAsync(id);
                if (existingDepartment is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy đơn vị công tác"));
                }

                existingDepartment.Name = request.Name;
                await departmentService.UpdateAsync(existingDepartment);

                return Ok(new ApiResponse<object>(true, "Cập nhật đơn vị công tác thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error updating department");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteDepartment([FromRoute] Guid id)
        {
            try
            {
                var existingDepartment = await departmentService.GetByIdAsync(id);
                if (existingDepartment is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy đơn vị công tác"));
                }

                await departmentService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa đơn vị công tác thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting department");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
