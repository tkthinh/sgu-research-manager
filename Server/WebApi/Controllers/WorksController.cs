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
            try
            {
                var works = await _workService.GetAllWorksWithAuthorsAsync();
                return Ok(new ApiResponse<IEnumerable<WorkDto>>(
                    true,
                    "Lấy dữ liệu công trình thành công",
                    works
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách công trình");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WorkDto>>> GetWork([FromRoute] Guid id)
        {
            var work = await _workService.GetWorkByIdWithAuthorsAsync(id);
            if (work is null)
            {
                return NotFound(new ApiResponse<WorkDto>(
                    false,
                    "Không tìm thấy công trình"
                ));
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
        public async Task<ActionResult<ApiResponse<WorkDto>>> CreateWork([FromBody] CreateWorkRequestDto request)
        {
            try
            {
                var work = await _workService.CreateWorkWithAuthorAsync(request);
                return CreatedAtAction(nameof(GetWork), new { id = work.Id },
                    new ApiResponse<WorkDto>(true, "Tạo công trình và tác giả thành công", work));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo công trình và tác giả");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpPost("{workId}/co-author-declare")]
        public async Task<ActionResult<ApiResponse<WorkDto>>> CoAuthorDeclare([FromRoute] Guid workId,[FromBody] UpdateWorkWithAuthorRequestDto request)
        {
            try
            {
                var work = await _workService.CoAuthorDeclaredAsync(workId, request.WorkRequest, request.AuthorRequest);
                return Ok(new ApiResponse<WorkDto>(
                    true,
                    "Đồng tác giả kê khai công trình thành công",
                    work
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đồng tác giả kê khai công trình");
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

        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkDto>>>> GetWorksByDepartmentId([FromRoute] Guid departmentId)
        {
            try
            {
                var works = await _workService.GetWorksByDepartmentIdAsync(departmentId);
                return Ok(new ApiResponse<IEnumerable<WorkDto>>(
                    true,
                    "Lấy danh sách công trình theo phòng ban thành công",
                    works
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách công trình theo phòng ban");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpPatch("authors/{authorId}/mark")]
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

        [HttpPatch("{workId}/admin-update")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<WorkDto>>> AdminUpdateWork(
            [FromRoute] Guid workId,
            [FromBody] AdminUpdateWorkRequestDto request)
        {
            try
            {
                var work = await _workService.GetWorkByIdWithAuthorsAsync(workId);
                if (work == null)
                    return NotFound(new ApiResponse<WorkDto>(false, "Công trình không tồn tại"));

                var workEntity = await _workService.UpdateWorkAdminAsync(workId, request);
                return Ok(new ApiResponse<WorkDto>(
                    true,
                    "Cập nhật công trình thành công",
                    workEntity
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật công trình");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }
    }
}