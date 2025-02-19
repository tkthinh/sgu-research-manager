using Application.Departments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class DepartmentsController : ControllerBase
   {
      private readonly IDepartmentService departmentService;

      public DepartmentsController(IDepartmentService departmentService)
      {
         this.departmentService = departmentService;
      }

      [HttpGet]
      public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
      {
         var departments = await departmentService.GetAllAsync();
         return Ok(departments);
      }

      [HttpGet("{id}")]
      public async Task<ActionResult<DepartmentDto>> GetDepartment([FromRoute] Guid id)
      {
         var department = await departmentService.GetByIdAsync(id);
         if (department is null)
         {
            return NotFound();
         }
         return Ok(department);
      }

      [HttpPost]
      public async Task<ActionResult<DepartmentDto>> CreateDepartment([FromBody] CreateDepartmentRequestDto requestDto)
      {
         try
         {
            var departmentDto = new DepartmentDto
            {
               Name = requestDto.Name,
            };

            var department = await departmentService.CreateAsync(departmentDto);
            return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
         }
         catch (ArgumentException ex)
         {
            return BadRequest(ex.Message);
         }
      }


      [HttpPut("{id}")]
      public async Task<IActionResult> UpdateDepartment([FromRoute] Guid id, [FromBody] UpdateDepartmentRequestDto request)
      {
         try
         {
            var existingDepartment = await departmentService.GetByIdAsync(id);
            if (existingDepartment is null)
            {
               return NotFound();
            }

            existingDepartment.Name = request.Name;

            await departmentService.UpdateAsync(existingDepartment);
            return NoContent();
         }
         catch (ArgumentException ex)
         {
            return BadRequest(ex.Message);
         }
      }

      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteDepartment([FromRoute] Guid id)
      {
         var existingDepartment = await departmentService.GetByIdAsync(id);
         if (existingDepartment is null)
         {
            return NotFound();
         }

         await departmentService.DeleteAsync(id);
         return NoContent();
      }
   }
}
