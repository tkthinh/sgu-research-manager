using Application.Authors;
using Application.Shared.Response;
using Application.Works;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly IWorkService _workService;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IWorkService workService, ILogger<AuthorsController> logger)
        {
            _workService = workService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuthorDto>>>> GetAuthors()
        {
            try
            {
                var works = await _workService.GetAllAsync();
                var authors = works.SelectMany(w => w.Authors ?? new List<AuthorDto>());
                return Ok(new ApiResponse<IEnumerable<AuthorDto>>(true, "Lấy dữ liệu tác giả thành công", authors));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách tác giả");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AuthorDto>>> GetAuthor([FromRoute] Guid id)
        {
            try
            {
                var works = await _workService.GetAllAsync();
                var author = works.SelectMany(w => w.Authors ?? new List<AuthorDto>())
                    .FirstOrDefault(a => a.Id == id);
                if (author == null)
                {
                    return NotFound(new ApiResponse<AuthorDto>(false, "Không tìm thấy tác giả"));
                }
                return Ok(new ApiResponse<AuthorDto>(true, "Lấy dữ liệu tác giả thành công", author));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin tác giả");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<ApiResponse<AuthorDto>>> CreateAuthor([FromBody] CreateAuthorRequestDto requestDto)
        {
            try
            {
                if (!Request.Query.TryGetValue("workId", out var workIdValue) || !Guid.TryParse(workIdValue, out var workId))
                {
                    return BadRequest(new ApiResponse<object>(false, "WorkId không hợp lệ"));
                }

                var author = await _workService.AddAuthorToWorkAsync(workId, requestDto);
                var response = new ApiResponse<AuthorDto>(true, "Tạo tác giả thành công", author);
                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo tác giả");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateAuthor([FromRoute] Guid id, [FromBody] UpdateAuthorRequestDto request)
        {
            try
            {
                var works = await _workService.GetAllAsync();
                var existingAuthor = works.SelectMany(w => w.Authors ?? new List<AuthorDto>())
                    .FirstOrDefault(a => a.Id == id);
                if (existingAuthor == null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy tác giả"));
                }

                var authorDto = new AuthorDto
                {
                    Id = id,
                    WorkId = existingAuthor.WorkId,
                    UserId = request.UserId,
                    AuthorRoleId = request.AuthorRoleId,
                    PurposeId = request.PurposeId,
                    Position = request.Position,
                    ScoreLevel = request.ScoreLevel,
                    FinalAuthorHour = existingAuthor.FinalAuthorHour,
                    TempAuthorHour = existingAuthor.TempAuthorHour,
                    TempWorkHour = existingAuthor.TempWorkHour,
                    IsNotMatch = existingAuthor.TempAuthorHour != existingAuthor.FinalAuthorHour,
                    MarkedForScoring = request.MarkedForScoring,
                    CoAuthors = request.CoAuthors,
                    CreatedDate = existingAuthor.CreatedDate,
                    ModifiedDate = DateTime.UtcNow
                };

                await _workService.UpdateAuthorAsync(id, authorDto);
                return Ok(new ApiResponse<object>(true, "Cập nhật tác giả thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật tác giả");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteAuthor([FromRoute] Guid id)
        {
            try
            {
                var works = await _workService.GetAllAsync();
                var existingAuthor = works.SelectMany(w => w.Authors ?? new List<AuthorDto>())
                    .FirstOrDefault(a => a.Id == id);
                if (existingAuthor == null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy tác giả"));
                }

                await _workService.DeleteAuthorAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa tác giả thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa tác giả");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }
    }
}