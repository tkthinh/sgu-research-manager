using Application.Departments;
using Application.Shared.Response;
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

      public DepartmentsController(IDepartmentService departmentService, ILogger<DepartmentsController> logger)
      {
         this.departmentService = departmentService;
         this.logger = logger;
      }

      [HttpGet]
      public async Task<ActionResult<ApiResponse<IEnumerable<DepartmentDto>>>> GetDepartments()
      {
         var departments = await departmentService.GetAllAsync();
         return Ok(new ApiResponse<IEnumerable<DepartmentDto>>(
            true,
            "Lấy dữ liệu đơn vị công tác thành công",
            departments
         ));
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
