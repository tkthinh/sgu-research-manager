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
                "Lấy dữ liệu tác giả thành công",
                authors
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AuthorDto>>> GetAuthor([FromRoute] Guid id)
        {
            var author = await authorService.GetByIdAsync(id);
            if (author is null)
            {
                return NotFound(new ApiResponse<AuthorDto>(false, "Không tìm thấy tác giả"));
            }
            return Ok(new ApiResponse<AuthorDto>(
                true,
                "Lấy dữ liệu tác giả thành công",
                author
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<AuthorDto>>> CreateAuthor([FromBody] CreateAuthorRequestDto requestDto)
        {
            try
            {
                var dto = new AuthorDto
                {
                    WorkId = requestDto.WorkId,
                    UserId = requestDto.UserId,
                    AuthorRoleId = requestDto.AuthorRoleId,
                    PurposeId = requestDto.PurposeId,
                    Position = requestDto.Position,
                    DeclaredScore = requestDto.DeclaredScore,
                    FinalScore = requestDto.FinalScore,
                    DeclaredHours = requestDto.DeclaredHours,
                    FinalHours = requestDto.FinalHours,
                    IsNotMatch = requestDto.IsNotMatch,
                    MarkedForScoring = requestDto.MarkedForScoring,
                    CoAuthors = requestDto.CoAuthors
                };

                var createdAuthor = await authorService.CreateAsync(dto);
                var response = new ApiResponse<AuthorDto>(
                    true,
                    "Tạo tác giả thành công",
                    createdAuthor
                );
                return CreatedAtAction(nameof(GetAuthor), new { id = createdAuthor.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Lỗi khi tạo tác giả");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateAuthor([FromRoute] Guid id, [FromBody] UpdateAuthorRequestDto request)
        {
            try
            {
                var existingAuthor = await authorService.GetByIdAsync(id);
                if (existingAuthor is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy tác giả"));
                }

                existingAuthor.WorkId = request.WorkId;
                existingAuthor.UserId = request.UserId;
                existingAuthor.AuthorRoleId = request.AuthorRoleId;
                existingAuthor.PurposeId = request.PurposeId;
                existingAuthor.Position = request.Position;
                existingAuthor.DeclaredScore = request.DeclaredScore;
                existingAuthor.FinalScore = request.FinalScore;
                existingAuthor.DeclaredHours = request.DeclaredHours;
                existingAuthor.FinalHours = request.FinalHours;
                existingAuthor.IsNotMatch = request.IsNotMatch;
                existingAuthor.MarkedForScoring = request.MarkedForScoring;
                existingAuthor.CoAuthors = request.CoAuthors;

                await authorService.UpdateAsync(existingAuthor);
                return Ok(new ApiResponse<object>(true, "Cập nhật tác giả thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Lỗi khi cập nhật tác giả");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteAuthor([FromRoute] Guid id)
        {
            try
            {
                var existingAuthor = await authorService.GetByIdAsync(id);
                if (existingAuthor is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy tác giả"));
                }

                await authorService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa tác giả thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Lỗi khi xóa tác giả");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
