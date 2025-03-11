using Application.Factors;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactorsController : ControllerBase
    {
        private readonly IFactorService factorService;
        private readonly ILogger<FactorsController> logger;

        public FactorsController(IFactorService factorService, ILogger<FactorsController> logger)
        {
            this.factorService = factorService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<FactorDto>>>> GetFactors()
        {
            var factors = await factorService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<FactorDto>>(
                true,
                "Lấy dữ liệu hệ số thành công",
                factors
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<FactorDto>>> GetFactor([FromRoute] Guid id)
        {
            var factor = await factorService.GetByIdAsync(id);
            if (factor == null)
            {
                return NotFound(new ApiResponse<FactorDto>(false, "Không tìm thấy hệ số"));
            }
            return Ok(new ApiResponse<FactorDto>(
                true,
                "Lấy dữ liệu hệ số thành công",
                factor
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<FactorDto>>> CreateFactor([FromBody] CreateFactorRequestDto request)
        {
            try
            {
                var dto = new FactorDto
                {
                    WorkTypeId = request.WorkTypeId,
                    WorkLevelId = request.WorkLevelId,
                    PurposeId = request.PurposeId,
                    AuthorRoleId = request.AuthorRoleId,
                    Name = request.Name,
                    ScoreLevel = request.ScoreLevel,
                    ConvertHour = request.ConvertHour,
                    MaxAllowed = request.MaxAllowed
                };

                var createdFactor = await factorService.CreateAsync(dto);
                var response = new ApiResponse<FactorDto>(
                    true,
                    "Tạo hệ số thành công",
                    createdFactor
                );

                return CreatedAtAction(nameof(GetFactor), new { id = createdFactor.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error creating factor");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateFactor([FromRoute] Guid id, [FromBody] UpdateFactorRequestDto request)
        {
            try
            {
                var existingFactor = await factorService.GetByIdAsync(id);
                if (existingFactor == null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy hệ số"));
                }

                existingFactor.WorkTypeId = request.WorkTypeId;
                existingFactor.WorkLevelId = request.WorkLevelId;
                existingFactor.PurposeId = request.PurposeId;
                existingFactor.AuthorRoleId = request.AuthorRoleId;
                existingFactor.Name = request.Name;
                existingFactor.ScoreLevel = request.ScoreLevel;
                existingFactor.ConvertHour = request.ConvertHour;
                existingFactor.MaxAllowed = request.MaxAllowed;

                await factorService.UpdateAsync(existingFactor);
                return Ok(new ApiResponse<object>(true, "Cập nhật hệ số thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error updating factor");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteFactor([FromRoute] Guid id)
        {
            try
            {
                var existingFactor = await factorService.GetByIdAsync(id);
                if (existingFactor == null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy hệ số"));
                }

                await factorService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa hệ số thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting factor");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
