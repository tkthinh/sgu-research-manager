using Application.Authors;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService authorService;
        private readonly ILogger<AuthorsController> logger;

        public AuthorsController(IAuthorService authorService, ILogger<AuthorsController> logger)
        {
            this.authorService = authorService;
            this.logger = logger;
        }

        [HttpGet("registrable")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuthorDto>>>> GetRegistableAuthors()
        {
            try
            {
                var userId = GetCurrentUserId();

                var authors = await authorService.GetAllRegistableAuthorsOfUser(userId);
                return Ok(new ApiResponse<IEnumerable<AuthorDto>>(
                    true,
                    "Lấy dữ liệu tác giả thành công",
                    authors
                ));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Lỗi khi lấy dữ liệu tác giả");
                return BadRequest(new ApiResponse<object>(
                    false,
                    "Lỗi khi lấy dữ liệu tác giả",
                    null
                ));
            }
        }

        private Guid GetCurrentUserId()
        {
            var userClaims = HttpContext.User.Claims;

            var userId = userClaims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (userId == null)
            {
                throw new UnauthorizedAccessException("Không tìm thấy thông tin người dùng");
            }
            return Guid.Parse(userId);
        }
    }
}
