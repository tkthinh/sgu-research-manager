using Application.Authors;
using Application.Works;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorksController : ControllerBase
    {
        private readonly IWorkService _workService;
        private readonly ILogger<WorksController> _logger;

        public WorksController(IWorkService workService, ILogger<WorksController> logger)
        {
            _workService = workService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkDto>>>> GetWorks()
        {
            var works = await _workService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<WorkDto>>(
                true,
                "Lấy dữ liệu công trình thành công",
                works
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WorkDto>>> GetWork([FromRoute] Guid id)
        {
            var work = await _workService.GetByIdAsync(id);
            if (work == null)
            {
                return NotFound(new ApiResponse<WorkDto>(false, "Không tìm thấy công trình"));
            }
            return Ok(new ApiResponse<WorkDto>(
                true,
                "Lấy dữ liệu công trình thành công",
                work
            ));
        }

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkDto>>>> SearchWorks([FromQuery] string title)
        {
            if (string.IsNullOrEmpty(title))
                return BadRequest(new ApiResponse<object>(false, "Tiêu đề không được để trống"));

            var works = await _workService.SearchWorksAsync(title);
            return Ok(new ApiResponse<IEnumerable<WorkDto>>(
                true,
                "Tìm kiếm công trình thành công",
                works
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<WorkDto>>> CreateWork([FromBody] CreateWorkRequestDto requestDto)
        {
            try
            {
                var createdWork = await _workService.CreateWorkWithAuthorAsync(requestDto);
                var response = new ApiResponse<WorkDto>(
                    true,
                    "Tạo công trình thành công",
                    createdWork
                );
                return CreatedAtAction(nameof(GetWork), new { id = createdWork.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo công trình");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpPost("{workId}/authors")]
        public async Task<ActionResult<ApiResponse<AuthorDto>>> AddAuthorToWork([FromRoute] Guid workId, [FromBody] CreateAuthorRequestDto request)
        {
            try
            {
                var author = await _workService.AddAuthorToWorkAsync(workId, request);
                return Ok(new ApiResponse<AuthorDto>(
                    true,
                    "Thêm tác giả vào công trình thành công",
                    author
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm tác giả vào công trình");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateWork([FromRoute] Guid id, [FromBody] UpdateWorkRequestDto request)
        {
            try
            {
                var existingWork = await _workService.GetByIdAsync(id);
                if (existingWork == null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy công trình"));
                }

                var workDto = new WorkDto
                {
                    Id = id,
                    Title = request.Title,
                    TimePublished = request.TimePublished,
                    TotalAuthors = request.TotalAuthors,
                    TotalMainAuthors = request.TotalMainAuthors,
                    FinalWorkHour = request.FinalWorkHour,
                    Note = request.Note,
                    Details = request.Details,
                    Source = request.Source,
                    WorkTypeId = request.WorkTypeId,
                    WorkLevelId = request.WorkLevelId,
                    SCImagoFieldId = request.SCImagoFieldId,
                    ScoringFieldId = request.ScoringFieldId,
                    ProofStatusId = request.ProofStatusId,
                    CreatedDate = existingWork.CreatedDate,
                    ModifiedDate = DateTime.UtcNow
                };

                await _workService.UpdateAsync(workDto);
                return Ok(new ApiResponse<object>(true, "Cập nhật công trình thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật công trình");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpPut("authors/{id}")]
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
                    DeclaredScore = request.DeclaredScore,
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
        public async Task<ActionResult<ApiResponse<object>>> DeleteWork([FromRoute] Guid id)
        {
            try
            {
                var existingWork = await _workService.GetByIdAsync(id);
                if (existingWork == null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy công trình"));
                }

                await _workService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa công trình thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa công trình");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuthorDto>>>> GetWorksByUserId([FromRoute] Guid userId)
        {
            try
            {
                var authors = await _workService.GetWorksByUserIdAsync(userId);
                return Ok(new ApiResponse<IEnumerable<AuthorDto>>(
                    true,
                    "Lấy danh sách công trình của user thành công",
                    authors
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách công trình của user");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpPut("{workId}/final-hour")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateWorkFinalHour([FromRoute] Guid workId, [FromBody] int finalWorkHour)
        {
            try
            {
                await _workService.UpdateWorkFinalHourAsync(workId, finalWorkHour);
                return Ok(new ApiResponse<object>(true, "Cập nhật giờ chính thức thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật giờ chính thức");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpPut("authors/{authorId}/mark")]
        public async Task<ActionResult<ApiResponse<object>>> SetMarkedForScoring([FromRoute] Guid authorId, [FromBody] bool marked)
        {
            try
            {
                await _workService.SetMarkedForScoringAsync(authorId, marked);
                return Ok(new ApiResponse<object>(true, "Cập nhật trạng thái MarkedForScoring thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật trạng thái MarkedForScoring");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }
    }
}