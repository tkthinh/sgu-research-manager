using Application.Purposes;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurposesController : ControllerBase
    {
        private readonly IPurposeService purposeService;
        private readonly ILogger<PurposesController> logger;

        public PurposesController(IPurposeService purposeService, ILogger<PurposesController> logger)
        {
            this.purposeService = purposeService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PurposeDto>>>> GetPurposes()
        {
            var purposes = await purposeService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<PurposeDto>>(
               true,
               "Lấy dữ liệu mục đích quy đổi thành công",
               purposes
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PurposeDto>>> GetPurpose([FromRoute] Guid id)
        {
            var purpose = await purposeService.GetByIdAsync(id);
            if (purpose is null)
            {
                return NotFound(new ApiResponse<PurposeDto>(false, "Không tìm thấy mục đích quy đổi"));
            }
            return Ok(new ApiResponse<PurposeDto>(
               true,
               "Lấy dữ liệu mục đích quy đổi thành công",
               purpose
            ));
        }

        [HttpGet("by-worktype/{workTypeId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PurposeDto>>>> GetPurposesByWorkTypeId([FromRoute] Guid workTypeId)
        {
            var purposes = await purposeService.GetPurposesByWorkTypeIdAsync(workTypeId);
            return Ok(new ApiResponse<IEnumerable<PurposeDto>>(
                   true,
                   "Lấy dữ liệu trạng thái công việc theo loại công trình thành công",
                   purposes
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PurposeDto>>> CreatePurpose([FromBody] CreatePurposeRequestDto requestDto)
        {
            try
            {
                var purposeDto = new PurposeDto
                {
                    Name = requestDto.Name,
                    WorkTypeId = requestDto.WorkTypeId
                };

                var purpose = await purposeService.CreateAsync(purposeDto);
                var response = new ApiResponse<PurposeDto>(
                   true,
                   "Tạo mục đích quy đổi thành công",
                   purpose
                );

                return CreatedAtAction(nameof(GetPurpose), new { id = purpose.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error creating purpose");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdatePurpose([FromRoute] Guid id, [FromBody] UpdatePurposeRequestDto request)
        {
            try
            {
                var existingPurpose = await purposeService.GetByIdAsync(id);
                if (existingPurpose is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy mục đích quy đổi"));
                }

                existingPurpose.Name = request.Name;
                existingPurpose.WorkTypeId = request.WorkTypeId;
                await purposeService.UpdateAsync(existingPurpose);

                return Ok(new ApiResponse<object>(true, "Cập nhật mục đích quy đổi thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error updating purpose");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeletePurpose([FromRoute] Guid id)
        {
            try
            {
                var existingPurpose = await purposeService.GetByIdAsync(id);
                if (existingPurpose is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy mục đích quy đổi"));
                }

                await purposeService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa mục đích quy đổi thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting purpose");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }
    }
}