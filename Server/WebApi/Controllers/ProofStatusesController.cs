using Application.Shared.Response;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
   [Route("api/[controller]")]
    [ApiController]
    public class ProofStatusesController : ControllerBase
    {
      [HttpGet]
      public ActionResult<ApiResponse<IEnumerable<object>>> GetProofStatuss()
      {
         // Lấy tất cả các giá trị của enum ProofStatus, chuyển đổi sang object gồm id và name.
         var ranks = Enum.GetValues(typeof(ProofStatus))
                         .Cast<ProofStatus>()
                         .Select(rank => new
                         {
                            Id = (int)rank,
                            Name = rank.ToString()
                         })
                         .ToList();

         return Ok(new ApiResponse<IEnumerable<object>>(
             true,
             "Lấy dữ liệu tình trạng minh chứng thành công",
             ranks
         ));
      }
   }
}
