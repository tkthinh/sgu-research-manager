using Application.Authors;
using Application.Works;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorksController : ControllerBase
    {
        private readonly IWorkService _workService;
        private readonly ILogger<WorksController> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public WorksController(IWorkService workService, ILogger<WorksController> logger, IHubContext<NotificationHub> hubContext)
        {
            _workService = workService;
            _logger = logger;
            _hubContext = hubContext;
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

        [HttpPost("{workId}/add-co-author")]
        public async Task<ActionResult<ApiResponse<WorkDto>>> AddCoAuthor([FromRoute] Guid workId, [FromBody] AddCoAuthorRequestDto request)
        {
            try
            {
                var work = await _workService.AddCoAuthorAsync(workId, request);
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
        public async Task<ActionResult<ApiResponse<bool>>> DeleteWork(Guid id)
        {
            try
            {
                await _workService.DeleteWorkAsync(id);
                return Ok(new ApiResponse<bool>(true, "Xóa công trình thành công", true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting work with ID {WorkId}", id);
                return BadRequest(new ApiResponse<bool>(false, "Có lỗi xảy ra khi xóa công trình: " + ex.Message));
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

                if (workEntity.Authors != null && workEntity.Authors.Any())
                {
                    // Lấy danh sách UserId (đã convert sang string vì SignalR sử dụng chuỗi làm định danh người dùng)
                    var authorUserIds = workEntity.Authors
                        .Select(a => a.UserId.ToString())
                        .Distinct()
                        .ToList();

                    var notificationMessage = $"Công trình '{workEntity.Title}' đã được admin chấm.";

                    // Gửi thông báo đến những user có UserId tương ứng
                    await _hubContext.Clients.Users(authorUserIds).SendAsync("ReceiveNotification", notificationMessage);
                }

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

        [HttpPatch("{workId}/update-by-author")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ApiResponse<WorkDto>>> UpdateWorkByAuthor(
            [FromRoute] Guid workId,
            [FromBody] UpdateWorkByAuthorRequestDto request)
        {
            try
            {
                _logger.LogInformation("User Claims: {Claims}", string.Join(", ", User.Claims.Select(c => $"{c.Type}: {c.Value}")));

                var userNameClaim = User.FindFirst(ClaimTypes.Name);
                if (userNameClaim == null)
                {
                    return Unauthorized(new ApiResponse<object>(false, "Không xác định được người dùng"));
                }
                var userId = await _workService.GetUserIdFromUserNameAsync(userNameClaim.Value);

                var work = await _workService.UpdateWorkByAuthorAsync(workId, request, userId);

                // Gửi thông báo đến tất cả các tác giả của công trình
                if (work.Authors != null && work.Authors.Any())
                {
                    var authorUserIds = work.Authors
                        .Select(a => a.UserId.ToString())
                        .Distinct()
                        .ToList();

                    var notificationMessage = $"Công trình '{work.Title}' đã được cập nhật bởi {userId}.";
                    await _hubContext.Clients.Users(authorUserIds).SendAsync("ReceiveNotification", notificationMessage);
                }

                return Ok(new ApiResponse<WorkDto>(
                    true,
                    "Cập nhật thông tin công trình và tác giả thành công",
                    work
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tác giả cập nhật công trình và thông tin tác giả");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }
    }
}