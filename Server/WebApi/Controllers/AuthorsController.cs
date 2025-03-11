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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuthorDto>>>> GetAuthors()
        {
            var authors = await authorService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<AuthorDto>>(
                true,
                "Lấy dữ liệu phân công thành công",
                authors
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AuthorDto>>> GetAuthor([FromRoute] Guid id)
        {
            var author = await authorService.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound(new ApiResponse<AuthorDto>(false, "Không tìm thấy phân công"));
            }
            return Ok(new ApiResponse<AuthorDto>(
                true,
                "Lấy dữ liệu phân công thành công",
                author
            ));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteAuthor([FromRoute] Guid id)
        {
            try
            {
                var existingAuthor = await authorService.GetByIdAsync(id);
                if (existingAuthor == null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy phân công"));
                }

                await authorService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa phân công thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting author");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
