using Application.AcademicRanks;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademicRanksController : ControllerBase
    {
        private readonly IAcademicRankService academicRankService;
        private readonly ILogger<AcademicRanksController> logger;

        public AcademicRanksController(IAcademicRankService academicRankService, ILogger<AcademicRanksController> logger)
        {
            this.academicRankService = academicRankService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AcademicRankDto>>>> GetAcademicRanks()
        {
            var ranks = await academicRankService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<AcademicRankDto>>(
                true,
                "Lấy dữ liệu học hàm thành công",
                ranks
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AcademicRankDto>>> GetAcademicRank([FromRoute] Guid id)
        {
            var rank = await academicRankService.GetByIdAsync(id);
            if (rank is null)
            {
                return NotFound(new ApiResponse<AcademicRankDto>(false, "Không tìm thấy học hàm"));
            }
            return Ok(new ApiResponse<AcademicRankDto>(
                true,
                "Lấy dữ liệu học hàm thành công",
                rank
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<AcademicRankDto>>> CreateAcademicRank([FromBody] CreateAcademicRankRequestDto requestDto)
        {
            try
            {
                var rankDto = new AcademicRankDto
                {
                    Name = requestDto.Name,
                };

                var rank = await academicRankService.CreateAsync(rankDto);
                var response = new ApiResponse<AcademicRankDto>(
                    true,
                    "Tạo học hàm thành công",
                    rank
                );

                return CreatedAtAction(nameof(GetAcademicRank), new { id = rank.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error creating academic rank");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateAcademicRank([FromRoute] Guid id, [FromBody] UpdateAcademicRankRequestDto request)
        {
            try
            {
                var existingRank = await academicRankService.GetByIdAsync(id);
                if (existingRank is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy học hàm"));
                }

                existingRank.Name = request.Name;
                await academicRankService.UpdateAsync(existingRank);

                return Ok(new ApiResponse<object>(true, "Cập nhật học hàm thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error updating academic rank");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteAcademicRank([FromRoute] Guid id)
        {
            try
            {
                var existingRank = await academicRankService.GetByIdAsync(id);
                if (existingRank is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy học hàm"));
                }

                await academicRankService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa học hàm thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting academic rank");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
