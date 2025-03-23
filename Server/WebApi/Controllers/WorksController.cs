using Application.Authors;
using Application.Works;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Enums;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorksController : ControllerBase
    {
        private readonly IWorkService _workService;
        private readonly ILogger<WorksController> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;

        public WorksController(IWorkService workService, ILogger<WorksController> logger, IHubContext<NotificationHub> hubContext, IUnitOfWork unitOfWork)
        {
            _workService = workService;
            _logger = logger;
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
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

        [HttpPost]
        [Authorize(Roles = "User")]
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteWork(Guid id)
        {
            try
            {
                // Lấy UserId từ token
                var userIdClaim = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new ApiResponse<object>(false, "Không xác định được người dùng"));
                }

                // Lấy userName để log (tùy chọn)
                var userName = User.FindFirst("fullName")?.Value ?? User.FindFirst(ClaimTypes.Name)?.Value ?? "Người dùng";

                // Kiểm tra xem userId có phải là tác giả của công trình không
                var work = await _workService.GetWorkByIdWithAuthorsAsync(id);
                if (work == null)
                {
                    return NotFound(new ApiResponse<object>(false, "Công trình không tồn tại"));
                }

                var isAuthor = work.Authors?.Any(a => a.UserId == userId) ?? false;
                if (!isAuthor)
                {
                    return StatusCode(403, new ApiResponse<object>(false, "Bạn không có quyền xóa công trình này"));
                }

                // Gọi service để xóa công trình
                await _workService.DeleteWorkAsync(id, userId);

                // Gửi thông báo đến các tác giả khác (tùy chọn, có thể bỏ nếu không cần)
                if (work.Authors != null && work.Authors.Any())
                {
                    var authorUserIds = work.Authors
                        .Where(a => a.UserId != userId)
                        .Select(a => a.UserId.ToString())
                        .Distinct()
                        .ToList();

                    if (authorUserIds.Any())
                    {
                        var notificationMessage = $"Công trình '{work.Title}' đã bị xóa bởi {userName}.";
                        await _hubContext.Clients.Users(authorUserIds).SendAsync("ReceiveNotification", notificationMessage);
                    }
                }

                return Ok(new ApiResponse<bool>(true, "Xóa công trình thành công", true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting work with ID {WorkId}", id);
                return BadRequest(new ApiResponse<bool>(false, "Có lỗi xảy ra khi xóa công trình: " + ex.Message));
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkDto>>>> GetWorksByUserId([FromRoute] Guid userId)
        {
            try
            {
                var works = await _workService.GetWorksByUserIdAsync(userId);
                return Ok(new ApiResponse<IEnumerable<WorkDto>>(
                    true,
                    "Lấy danh sách công trình của user thành công",
                    works
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
        [Authorize(Roles = "User")]
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

        [HttpPatch("{workId}/admin-update/{userId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<WorkDto>>> UpdateWorkByAdmin(
            [FromRoute] Guid workId,
            [FromRoute] Guid userId,
            [FromBody] UpdateWorkByAdminRequestDto request)
        {
            try
            {
                var work = await _workService.GetWorkByIdWithAuthorsAsync(workId);
                if (work == null)
                    return NotFound(new ApiResponse<WorkDto>(false, "Công trình không tồn tại"));

                var updatedWork = await _workService.UpdateWorkByAdminAsync(workId, userId, request);

                if (updatedWork.Authors != null && updatedWork.Authors.Any())
                {
                    var authorUserIds = updatedWork.Authors
                        .Where(a => a.UserId == userId)
                        .Select(a => a.UserId.ToString())
                        .Distinct()
                        .ToList();

                    var notificationMessage = $"Công trình '{updatedWork.Title}' đã được admin chấm.";
                    await _hubContext.Clients.Users(authorUserIds).SendAsync("ReceiveNotification", notificationMessage);
                }

                return Ok(new ApiResponse<WorkDto>(
                    true,
                    "Cập nhật công trình thành công",
                    updatedWork
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật công trình");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpPatch("{workId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ApiResponse<WorkDto>>> UpdateWorkByAuthor(
              [FromRoute] Guid workId,
              [FromBody] UpdateWorkByAuthorRequestDto request)
        {
            try
            {
                // Lấy UserId từ token
                var userIdClaim = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new ApiResponse<object>(false, "Không xác định được người dùng"));
                }

                // Lấy userName để hiển thị trong thông báo
                var userName = User.FindFirst("fullName")?.Value ?? User.FindFirst(ClaimTypes.Name)?.Value ?? "Người dùng";

                // Kiểm tra xem userId có phải là tác giả của công trình không
                var work = await _workService.GetWorkByIdWithAuthorsAsync(workId);

                if (work == null)
                {
                    return NotFound(new ApiResponse<object>(false, "Công trình không tồn tại"));
                }

                // Kiểm tra xem userId có phải là tác giả của công trình không
                var isAuthor = work.Authors?.Any(a => a.UserId == userId) ?? false;
                
                // Kiểm tra xem userId có phải là đồng tác giả của công trình không
                var isCoAuthor = work.CoAuthorUserIds?.Contains(userId) ?? false;
                
                if (!isAuthor && !isCoAuthor)
                {
                    // Kiểm tra nếu userId nằm trong danh sách WorkAuthor nhưng không có trong CoAuthorUserIds (có thể do lỗi cập nhật)
                    // Sử dụng Repository để kiểm tra thêm
                    var workAuthorExists = await _unitOfWork.Repository<WorkAuthor>()
                        .FirstOrDefaultAsync(wa => wa.WorkId == workId && wa.UserId == userId);
                    
                    if (workAuthorExists == null)
                    {
                        return StatusCode(403, new ApiResponse<object>(false, "Bạn không có quyền cập nhật công trình này"));
                    }
                }

                // Cập nhật công trình
                var updatedWork = await _workService.UpdateWorkByAuthorAsync(workId, request, userId);

                // Gửi thông báo đến các tác giả khác
                if (updatedWork.Authors != null && updatedWork.Authors.Any())
                {
                    var authorUserIds = updatedWork.Authors
                        .Where(a => a.UserId != userId)
                        .Select(a => a.UserId.ToString())
                        .Distinct()
                        .ToList();
                    var coAuthorUserIds = updatedWork.CoAuthorUserIds
                        .Where(uid => uid != userId)
                        .Select(uid => uid.ToString())
                        .Distinct()
                        .ToList();
                    authorUserIds.AddRange(coAuthorUserIds);

                    if (authorUserIds.Any())
                    {
                        var notificationMessage = $"Công trình '{updatedWork.Title}' đã được cập nhật bởi {userName}.";
                        await _hubContext.Clients.Users(authorUserIds).SendAsync("ReceiveNotification", notificationMessage);
                    }
                }

                return Ok(new ApiResponse<WorkDto>(
                    true,
                    "Cập nhật thông tin công trình và tác giả thành công",
                    updatedWork
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tác giả cập nhật công trình và thông tin tác giả");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpGet("my-works")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkDto>>>> GetMyWorks()
        {
            try
            {
                var useridclaim = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(useridclaim) || !Guid.TryParse(useridclaim, out var userid))
                {
                    return Unauthorized(new ApiResponse<object>(false, "không xác định được người dùng"));
                }

                // lấy danh sách công trình mà user đã kê khai
                var works = await _workService.GetWorksByCurrentUserAsync(userid);

                return Ok(new ApiResponse<IEnumerable<WorkDto>>(
                    true,
                    "lấy danh sách công trình của người dùng hiện tại thành công",
                    works
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách công trình của người dùng hiện tại");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }
    }
}