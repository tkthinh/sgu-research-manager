using Application.Shared.Response;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class AcademicTitlesController : ControllerBase
   {
      [HttpGet]
      public ActionResult<ApiResponse<IEnumerable<object>>> GetAcademicTitles()
      {
         // Lấy tất cả các giá trị của enum AcademicTitle, chuyển đổi sang object gồm id và name.
         var ranks = Enum.GetValues(typeof(AcademicTitle))
                         .Cast<AcademicTitle>()
                         .Select(rank => new
                         {
                            Id = (int)rank,
                            Name = rank.ToString()
                         })
                         .ToList();

         return Ok(new ApiResponse<IEnumerable<object>>(
             true,
             "Lấy dữ liệu học vị thành công",
             ranks
         ));
      }
   }
}
