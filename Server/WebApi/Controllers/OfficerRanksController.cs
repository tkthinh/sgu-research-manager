using Application.OfficerRanks;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficerRanksController : ControllerBase
    {
        private readonly IOfficerRankService officerRankService;
        private readonly ILogger<OfficerRanksController> logger;

        public OfficerRanksController(IOfficerRankService officerRankService, ILogger<OfficerRanksController> logger)
        {
            this.officerRankService = officerRankService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<OfficerRankDto>>>> GetOfficerRanks()
        {
            var ranks = await officerRankService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<OfficerRankDto>>(
                true,
                "Lấy dữ liệu ngạch công chức thành công",
                ranks
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OfficerRankDto>>> GetOfficerRank([FromRoute] Guid id)
        {
            var rank = await officerRankService.GetByIdAsync(id);
            if (rank is null)
            {
                return NotFound(new ApiResponse<OfficerRankDto>(false, "Không tìm thấy ngạch công chức"));
            }
            return Ok(new ApiResponse<OfficerRankDto>(
                true,
                "Lấy dữ liệu ngạch công chức thành công",
                rank
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<OfficerRankDto>>> CreateOfficerRank([FromBody] CreateOfficerRankRequestDto requestDto)
        {
            try
            {
                var rankDto = new OfficerRankDto
                {
                    Name = requestDto.Name,
                };

                var rank = await officerRankService.CreateAsync(rankDto);
                var response = new ApiResponse<OfficerRankDto>(
                    true,
                    "Tạo ngạch công chức thành công",
                    rank
                );

                return CreatedAtAction(nameof(GetOfficerRank), new { id = rank.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error creating officer rank");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateOfficerRank([FromRoute] Guid id, [FromBody] UpdateOfficerRankRequestDto request)
        {
            try
            {
                var existingRank = await officerRankService.GetByIdAsync(id);
                if (existingRank is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy ngạch công chức"));
                }

                existingRank.Name = request.Name;
                await officerRankService.UpdateAsync(existingRank);

                return Ok(new ApiResponse<object>(true, "Cập nhật ngạch công chức thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error updating officer rank");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteOfficerRank([FromRoute] Guid id)
        {
            try
            {
                var existingRank = await officerRankService.GetByIdAsync(id);
                if (existingRank is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy ngạch công chức"));
                }

                await officerRankService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa ngạch công chức thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting officer rank");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
