using Application.Shared.Response;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreLevelsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<object>>> GetScoreLevels()
        {
            // Lấy tất cả các giá trị của enum ScoreLevel, chuyển đổi sang object gồm id và name.
            var scoreLevels = Enum.GetValues(typeof(ScoreLevel))
                                  .Cast<ScoreLevel>()
                                  .Select(score => new
                                  {
                                      Id = (int)score,
                                      Name = score.ToString()
                                  })
                                  .ToList();

            return Ok(new ApiResponse<IEnumerable<object>>(
                true,
                "Lấy dữ liệu mức điểm thành công",
                scoreLevels
            ));
        }
    }
}
