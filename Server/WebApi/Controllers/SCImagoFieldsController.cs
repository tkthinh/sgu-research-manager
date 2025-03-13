using Application.SCImagoFields;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SCImagoFieldsController : ControllerBase
    {
        private readonly ISCImagoFieldService scImagoFieldService;
        private readonly ILogger<SCImagoFieldsController> logger;

        public SCImagoFieldsController(ISCImagoFieldService scImagoFieldService, ILogger<SCImagoFieldsController> logger)
        {
            this.scImagoFieldService = scImagoFieldService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SCImagoFieldDto>>>> GetSCImagoFields()
        {
            var fields = await scImagoFieldService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<SCImagoFieldDto>>(
                true,
                "Lấy dữ liệu ngành SCImagos thành công",
                fields
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SCImagoFieldDto>>> GetSCImagoField([FromRoute] Guid id)
        {
            var field = await scImagoFieldService.GetByIdAsync(id);
            if (field is null)
            {
                return NotFound(new ApiResponse<SCImagoFieldDto>(false, "Không tìm thấy ngành SCImago"));
            }
            return Ok(new ApiResponse<SCImagoFieldDto>(
                true,
                "Lấy dữ liệu ngành SCImago thành công",
                field
            ));
        }

        [HttpGet("by-worktype/{workTypeId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SCImagoFieldDto>>>> GetSCImagoFieldsByWorkTypeId([FromRoute] Guid workTypeId)
        {
            if (workTypeId == Guid.Empty)
            {
                return BadRequest(new ApiResponse<IEnumerable<SCImagoFieldDto>>(false, "WorkTypeId không hợp lệ"));
            }

            var scImagoFields = await scImagoFieldService.GetSCImagoFieldsByWorkTypeIdAsync(workTypeId);
            if (scImagoFields == null || !scImagoFields.Any())
            {
                return Ok(new ApiResponse<IEnumerable<SCImagoFieldDto>>(
                    true,
                    "Không tìm thấy lĩnh vực SCImago cho loại công trình này",
                    Enumerable.Empty<SCImagoFieldDto>()
                ));
            }

            return Ok(new ApiResponse<IEnumerable<SCImagoFieldDto>>(
                true,
                "Lấy dữ liệu lĩnh vực SCImago theo loại công trình thành công",
                scImagoFields
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<SCImagoFieldDto>>> CreateSCImagoField([FromBody] CreateSCImagoFieldRequestDto request)
        {
            try
            {
                var dto = new SCImagoFieldDto
                {
                    Name = request.Name,
                    WorkTypeId = request.WorkTypeId
                };

                var field = await scImagoFieldService.CreateAsync(dto);
                var response = new ApiResponse<SCImagoFieldDto>(true, "Tạo ngành SCImago thành công", field);
                return CreatedAtAction(nameof(GetSCImagoField), new { id = field.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Lỗi khi tạo ngành SCImago");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateSCImagoField([FromRoute] Guid id, [FromBody] UpdateSCImagoFieldRequestDto request)
        {
            try
            {
                var existingField = await scImagoFieldService.GetByIdAsync(id);
                if (existingField is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy ngành SCImago"));
                }
                existingField.Name = request.Name;
                existingField.WorkTypeId = request.WorkTypeId;
                await scImagoFieldService.UpdateAsync(existingField);
                return Ok(new ApiResponse<object>(true, "Cập nhật ngành SCImago thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Lỗi khi cập nhật ngành SCImago");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteSCImagoField([FromRoute] Guid id)
        {
            try
            {
                var existingField = await scImagoFieldService.GetByIdAsync(id);
                if (existingField is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy ngành SCImago"));
                }
                await scImagoFieldService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa ngành SCImago thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Lỗi khi xóa ngành SCImago");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
