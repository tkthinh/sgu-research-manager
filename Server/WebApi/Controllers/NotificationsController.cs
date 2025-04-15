using System.Security.Cryptography.Pkcs;
using Application.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService notificationService;
        public NotificationsController(
            INotificationService notificationService
        )
        {
            this.notificationService = notificationService;
        }

        [HttpGet("global")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetGlobalNotifications()
        {
            var notifications = await notificationService.GetGlobalNotificationsAsync();
            return Ok(notifications);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetUserNotifications()
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                return BadRequest("Người dùng không hợp lệ");
            }

            var notifications = await notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
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
