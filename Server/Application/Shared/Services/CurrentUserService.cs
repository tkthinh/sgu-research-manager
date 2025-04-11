using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Application.Shared.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public (bool isSuccess, Guid userId, string userName) GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                return (false, Guid.Empty, string.Empty);
            }

            var userIdClaim = user.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return (false, Guid.Empty, string.Empty);
            }

            var userName = user.FindFirst("fullName")?.Value ?? user.FindFirst(ClaimTypes.Name)?.Value ?? "Người dùng";
            return (true, userId, userName);
        }
    }
} 