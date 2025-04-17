using Application.Works;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Enums;
using Application.Shared.Services;
using Application.SystemConfigs;
using Application.AcademicYears;
using Application.Notifications;

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
        private readonly ICurrentUserService _currentUserService;
        private readonly IAcademicYearService _academicYearService;
        private readonly INotificationService _notificationService;

        public WorksController(
            IWorkService workService, 
            ILogger<WorksController> logger, 
            IHubContext<NotificationHub> hubContext, 
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IAcademicYearService academicYearService,
            INotificationService notificationService
            )
        {
            _workService = workService;
            _logger = logger;
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _academicYearService = academicYearService;
            _notificationService = notificationService;
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
        [Authorize(Roles = "User, Manager")]
        public async Task<ActionResult<ApiResponse<WorkDto>>> CreateWork([FromBody] CreateWorkRequestDto request)
        {
            try
            {
                // Lấy năm học hiện tại
                var currentAcademicYear = await _academicYearService.GetCurrentAcademicYear();
                if (currentAcademicYear == null)
                {
                    return BadRequest(new ApiResponse<object>(false, "Không có năm học nào đang mở. Vui lòng thử lại sau."));
                }

                // Gán AcademicYearId cho công trình
                request.AcademicYearId = currentAcademicYear.Id;

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
                var (isSuccess, userId, userName) = _currentUserService.GetCurrentUser();
                if (!isSuccess)
                {
                    return Unauthorized(new ApiResponse<object>(false, "Không xác định được người dùng"));
                }

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

                // Kiểm tra xem tác giả hiện tại đã được admin set ProofStatus = HopLe chưa
                var currentAuthor = work.Authors?.FirstOrDefault(a => a.UserId == userId);
                if (currentAuthor != null && currentAuthor.ProofStatus == ProofStatus.HopLe)
                {
                    return BadRequest(new ApiResponse<object>(false, "Bạn không thể xóa công trình đã được xác nhận hợp lệ bởi admin"));
                }

                // Kiểm tra xem có tác giả nào khác của công trình đã được set ProofStatus = HopLe chưa
                bool hasOtherApprovedAuthor = work.Authors?.Any(a => a.UserId != userId && a.ProofStatus == ProofStatus.HopLe) ?? false;
                if (hasOtherApprovedAuthor)
                {
                    return BadRequest(new ApiResponse<object>(
                        false, 
                        "Công trình đã có tác giả khác được xác nhận hợp lệ. Bạn không thể xóa công trình này."
                    ));
                }

                // Gọi service để xóa công trình
                await _workService.DeleteWorkAsync(id, userId);

                // Gửi thông báo đến các tác giả khác
                if (work.Authors != null && work.Authors.Any())
                {
                    var recipientIds = new HashSet<string>();

                    // Thêm các tác giả khác
                    var authorIds = work.Authors
                        .Where(a => a.UserId != userId)
                        .Select(a => a.UserId.ToString());
                    foreach (var authorId in authorIds) recipientIds.Add(authorId);

                    // Thêm các đồng tác giả
                    var coAuthorIds = work.CoAuthorUserIds?
                        .Where(uid => uid != userId)
                        .Select(uid => uid.ToString()) ?? Enumerable.Empty<string>();
                    foreach (var coAuthorId in coAuthorIds) recipientIds.Add(coAuthorId);

                    if (recipientIds.Any())
                    {
                        var notificationMessage = $"Công trình '{work.Title}' đã bị xóa bởi {userName}.";
                        await _hubContext.Clients.Users(recipientIds.ToList()).SendAsync("ReceiveNotification", notificationMessage);
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
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<ApiResponse<WorkDto>>> UpdateWorkByAdmin(
            [FromRoute] Guid workId,
            [FromRoute] Guid userId,
            [FromBody] UpdateWorkWithAuthorRequestDto request)
        {
            try
            {
                var work = await _workService.GetWorkByIdWithAuthorsAsync(workId);
                if (work == null)
                    return NotFound(new ApiResponse<WorkDto>(false, "Công trình không tồn tại"));

                // Admin có thể thay đổi năm học của công trình nếu cần
                if (request.WorkRequest?.AcademicYearId.HasValue == true)
                {
                    // Kiểm tra xem AcademicYear có tồn tại không
                    var academicYear = await _academicYearService.GetByIdAsync(request.WorkRequest.AcademicYearId.Value);
                    if (academicYear == null)
                    {
                        return BadRequest(new ApiResponse<object>(false, "Năm học không tồn tại"));
                    }
                }

                var updatedWork = await _workService.UpdateWorkByAdminAsync(workId, userId, request);

                if (updatedWork.Authors != null && updatedWork.Authors.Any())
                {
                    var authorUserIds = updatedWork.Authors
                        .Where(a => a.UserId == userId)
                        .Select(a => a.UserId.ToString())
                        .Distinct()
                        .ToList();

                    var notificationMessage = $"Công trình '{updatedWork.Title}' đã được admin chấm.";

                    foreach(var authorUserId in authorUserIds)
                    {
                        await _notificationService.CreateNotificationForUserAsync(Guid.Parse(authorUserId), notificationMessage);
                        await _hubContext.Clients.User(authorUserId).SendAsync("ReceiveNotification", notificationMessage);
                    }
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
              [FromBody] UpdateWorkWithAuthorRequestDto request)
        {
            try
            {
                var (isSuccess, userId, userName) = _currentUserService.GetCurrentUser();
                if (!isSuccess)
                {
                    return Unauthorized(new ApiResponse<object>(false, "Không xác định được người dùng"));
                }

                // Kiểm tra xem công trình có tồn tại không trước khi làm bất kỳ thứ gì khác
                var work = await _workService.GetWorkByIdWithAuthorsAsync(workId);
                if (work == null)
                {
                    return NotFound(new ApiResponse<object>(false, "Công trình không tồn tại"));
                }

                // Kiểm tra quyền truy cập theo một truy vấn duy nhất
                bool hasAccess = (work.Authors?.Any(a => a.UserId == userId) ?? false) ||
                                 (work.CoAuthorUserIds?.Contains(userId) ?? false);

                if (!hasAccess)
                {
                    // Kiểm tra bổ sung trong bảng WorkAuthor nếu không thấy trong danh sách authors/coAuthors
                    var workAuthorExists = await _unitOfWork.Repository<WorkAuthor>()
                        .FirstOrDefaultAsync(wa => wa.WorkId == workId && wa.UserId == userId);

                    if (workAuthorExists == null)
                    {
                        return StatusCode(403, new ApiResponse<object>(false, "Bạn không có quyền cập nhật công trình này"));
                    }
                }

                // Kiểm tra xem tác giả hiện tại đã được admin set ProofStatus = HopLe chưa
                var currentAuthor = work.Authors?.FirstOrDefault(a => a.UserId == userId);
                if (currentAuthor != null && currentAuthor.ProofStatus == ProofStatus.HopLe)
                {
                    return BadRequest(new ApiResponse<object>(false, "Bạn không thể cập nhật thông tin khi tác giả đã được xác nhận hợp lệ bởi admin"));
                }

                // Kiểm tra xem có tác giả nào của công trình đã được set ProofStatus = HopLe chưa
                bool hasApprovedAuthor = work.Authors?.Any(a => a.ProofStatus == ProofStatus.HopLe) ?? false;
                
                UpdateWorkWithAuthorRequestDto updatedRequest = request;
                
                // Nếu có tác giả đã được xác nhận hợp lệ, chỉ cho phép cập nhật thông tin tác giả
                if (hasApprovedAuthor && request.WorkRequest != null)
                {
                    // Tạo request mới chỉ có phần thông tin tác giả
                    updatedRequest = new UpdateWorkWithAuthorRequestDto 
                    { 
                        AuthorRequest = request.AuthorRequest,
                        WorkRequest = null
                    };
                    
                    // Log thông báo
                    _logger.LogInformation("Công trình {WorkId} đã có tác giả được xác nhận hợp lệ. Tác giả {UserId} chỉ có thể cập nhật thông tin tác giả", workId, userId);
                }

                // Đảm bảo người dùng không thể thay đổi AcademicYearId
                if (updatedRequest.WorkRequest != null)
                {
                    updatedRequest.WorkRequest.AcademicYearId = null; // Không cho phép người dùng thay đổi năm học
                }

                // Tiến hành cập nhật công trình với request đã được điều chỉnh
                var updatedWork = await _workService.UpdateWorkByAuthorAsync(workId, updatedRequest, userId);

                // Gửi thông báo đến tất cả các tác giả khác
                var allRecipientsIds = new HashSet<string>();

                // Thêm UserId của tất cả tác giả
                if (updatedWork.Authors?.Any() == true)
                {
                    var authorIds = updatedWork.Authors
                        .Where(a => a.UserId != userId)
                        .Select(a => a.UserId.ToString());
                    foreach (var id in authorIds) allRecipientsIds.Add(id);
                }

                // Thêm UserId của tất cả đồng tác giả
                if (updatedWork.CoAuthorUserIds?.Any() == true)
                {
                    var coAuthorIds = updatedWork.CoAuthorUserIds
                        .Where(uid => uid != userId)
                        .Select(uid => uid.ToString());
                    foreach (var id in coAuthorIds) allRecipientsIds.Add(id);
                }

                // Gửi thông báo nếu có người nhận
                if (allRecipientsIds.Any())
                {
                    var notificationMessage = hasApprovedAuthor && request.WorkRequest != null
                        ? $"Tác giả {userName} đã cập nhật thông tin tác giả của công trình '{updatedWork.Title}'."
                        : $"Công trình '{updatedWork.Title}' đã được cập nhật bởi {userName}.";
                    
                    await _hubContext.Clients.Users(allRecipientsIds.ToList()).SendAsync("ReceiveNotification", notificationMessage);
                }

                return Ok(new ApiResponse<WorkDto>(
                    true,
                    hasApprovedAuthor && request.WorkRequest != null
                        ? "Công trình đã có tác giả được xác nhận hợp lệ. Chỉ cập nhật thông tin tác giả thành công."
                        : "Cập nhật thông tin công trình và tác giả thành công",
                    updatedWork
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật công trình {WorkId} của người dùng", workId);
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpGet("my-works")]
        [Authorize(Roles = "User, Manager")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkDto>>>> GetMyWorks()
        {
            try
            {
                var (isSuccess, userId, _) = _currentUserService.GetCurrentUser();
                if (!isSuccess)
                {
                    return Unauthorized(new ApiResponse<object>(false, "Không xác định được người dùng"));
                }

                // Lấy danh sách công trình mà user đã kê khai
                var works = await _workService.GetWorksByCurrentUserAsync(userId);

                return Ok(new ApiResponse<IEnumerable<WorkDto>>(
                    true,
                    "Lấy danh sách công trình của người dùng hiện tại thành công",
                    works
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách công trình của người dùng hiện tại");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpGet("all-my-works")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkDto>>>> GetAllMyWorks()
        {
            try
            {
                var (isSuccess, userId, _) = _currentUserService.GetCurrentUser();
                if (!isSuccess)
                {
                    return Unauthorized(new ApiResponse<object>(false, "Không xác định được người dùng"));
                }

                // Lấy tất cả công trình của người dùng, bao gồm cả công trình do người dùng kê khai và công trình được admin import vào
                var works = await _workService.GetAllWorksByCurrentUserAsync(userId);

                return Ok(new ApiResponse<IEnumerable<WorkDto>>(
                    true,
                    "Lấy tất cả công trình của người dùng hiện tại thành công",
                    works
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tất cả công trình của người dùng hiện tại");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpGet("academic-year/{academicYearId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkDto>>>> GetWorksByAcademicYearId([FromRoute] Guid academicYearId)
        {
            try
            {
                var works = await _workService.GetWorksByAcademicYearIdAsync(academicYearId);
                return Ok(new ApiResponse<IEnumerable<WorkDto>>(
                    true,
                    "Lấy danh sách công trình theo năm học thành công",
                    works
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách công trình theo năm học");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpGet("my-works/academic-year/{academicYearId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkDto>>>> GetCurrentUserWorksByAcademicYearId([FromRoute] Guid academicYearId)
        {
            try
            {
                var (isSuccess, userId, _) = _currentUserService.GetCurrentUser();
                if (!isSuccess)
                {
                    return Unauthorized(new ApiResponse<object>(false, "Không xác định được người dùng"));
                }

                var works = await _workService.GetCurrentUserWorksByAcademicYearIdAsync(userId, academicYearId);
                return Ok(new ApiResponse<IEnumerable<WorkDto>>(
                    true,
                    "Lấy danh sách công trình của người dùng theo năm học thành công",
                    works
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách công trình của người dùng theo năm học");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }
    }
}