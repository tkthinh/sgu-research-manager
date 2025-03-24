using Application.ScoreLevels;
using Application.Shared.Response;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScoreLevelsController : ControllerBase
    {
        private readonly IScoreLevelService _scoreLevelService;
        private readonly ILogger<ScoreLevelsController> _logger;

        public ScoreLevelsController(IScoreLevelService scoreLevelService, ILogger<ScoreLevelsController> logger)
        {
            _scoreLevelService = scoreLevelService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<int>>>> GetScoreLevels(
            [FromQuery] Guid workTypeId,
            [FromQuery] Guid? workLevelId = null,
            [FromQuery] Guid? authorRoleId = null,
            [FromQuery] Guid? purposeId = null)
        {
            try
            {
                var scoreLevels = await _scoreLevelService.GetScoreLevelsByFiltersAsync(
                    workTypeId, workLevelId, authorRoleId, purposeId);
                
                return Ok(new ApiResponse<IEnumerable<int>>(
                    true,
                    "Lấy danh sách mức điểm thành công",
                    scoreLevels
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách mức điểm");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }
    }
}
