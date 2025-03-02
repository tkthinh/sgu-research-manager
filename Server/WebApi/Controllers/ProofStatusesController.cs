using Application.ProofStatuses;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProofStatusesController : ControllerBase
    {
        private readonly IProofStatusService proofStatusService;
        private readonly ILogger<ProofStatusesController> logger;

        public ProofStatusesController(IProofStatusService proofStatusService, ILogger<ProofStatusesController> logger)
        {
            this.proofStatusService = proofStatusService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProofStatusDto>>>> GetProofStatuses()
        {
            var statuses = await proofStatusService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ProofStatusDto>>(
                true,
                "Lấy dữ liệu trạng thái minh chứng thành công",
                statuses
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProofStatusDto>>> GetProofStatus([FromRoute] Guid id)
        {
            var status = await proofStatusService.GetByIdAsync(id);
            if (status is null)
            {
                return NotFound(new ApiResponse<ProofStatusDto>(false, "Không tìm thấy trạng thái minh chứng"));
            }
            return Ok(new ApiResponse<ProofStatusDto>(
                true,
                "Lấy dữ liệu trạng thái minh chứng thành công",
                status
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProofStatusDto>>> CreateProofStatus([FromBody] CreateProofStatusRequestDto requestDto)
        {
            try
            {
                var statusDto = new ProofStatusDto
                {
                    Name = requestDto.Name,
                };

                var status = await proofStatusService.CreateAsync(statusDto);
                var response = new ApiResponse<ProofStatusDto>(
                    true,
                    "Tạo trạng thái minh chứng thành công",
                    status
                );

                return CreatedAtAction(nameof(GetProofStatus), new { id = status.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error creating proof status");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateProofStatus([FromRoute] Guid id, [FromBody] UpdateProofStatusRequestDto request)
        {
            try
            {
                var existingStatus = await proofStatusService.GetByIdAsync(id);
                if (existingStatus is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy trạng thái minh chứng"));
                }

                existingStatus.Name = request.Name;
                await proofStatusService.UpdateAsync(existingStatus);

                return Ok(new ApiResponse<object>(true, "Cập nhật trạng thái minh chứng thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error updating proof status");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteProofStatus([FromRoute] Guid id)
        {
            try
            {
                var existingStatus = await proofStatusService.GetByIdAsync(id);
                if (existingStatus is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy trạng thái minh chứng"));
                }

                await proofStatusService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa trạng thái minh chứng thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting proof status");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
