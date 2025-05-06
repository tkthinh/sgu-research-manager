using Application.Fields;
using Application.Shared.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldsController : ControllerBase
    {
        private readonly IFieldService fieldService;
        private readonly ILogger<FieldsController> logger;

        public FieldsController(IFieldService fieldService, ILogger<FieldsController> logger)
        {
            this.fieldService = fieldService;
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<FieldDto>>>> GetFields()
        {
            var fields = await fieldService.GetAllAsync();

            return Ok(new ApiResponse<IEnumerable<FieldDto>>(
                true,
                "Lấy dữ liệu ngành thành công",
                fields.OrderBy(f => f.Name)
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<FieldDto>>> GetField([FromRoute] Guid id)
        {
            var field = await fieldService.GetByIdAsync(id);
            if (field is null)
            {
                return NotFound(new ApiResponse<FieldDto>(false, "Không tìm thấy ngành"));
            }
            return Ok(new ApiResponse<FieldDto>(
                true,
                "Lấy dữ liệu ngành thành công",
                field
            ));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<FieldDto>>> CreateField([FromBody] CreateFieldRequestDto requestDto)
        {
            try
            {
                var fieldDto = new FieldDto
                {
                    Name = requestDto.Name,
                };

                var field = await fieldService.CreateAsync(fieldDto);
                var response = new ApiResponse<FieldDto>(
                    true,
                    "Tạo ngành thành công",
                    field
                );

                return CreatedAtAction(nameof(GetField), new { id = field.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error creating field");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateField([FromRoute] Guid id, [FromBody] UpdateFieldRequestDto request)
        {
            try
            {
                var existingField = await fieldService.GetByIdAsync(id);
                if (existingField is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy ngành"));
                }

                existingField.Name = request.Name;
                await fieldService.UpdateAsync(existingField);

                return Ok(new ApiResponse<object>(true, "Cập nhật ngành thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error updating field");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteField([FromRoute] Guid id)
        {
            try
            {
                var existingField = await fieldService.GetByIdAsync(id);
                if (existingField is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy ngành"));
                }

                await fieldService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa ngành thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting field");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
