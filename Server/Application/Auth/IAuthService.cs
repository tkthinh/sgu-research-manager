using Application.Shared.Response;
using Application.Users;

namespace Application.Auth
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> LoginAsync(LoginRequestDto request);
        Task<ApiResponse<UserDto>> RegisterAsync(RegisterRequestDto request);
        Task<ApiResponse<object>> ChangePasswordAsync(ChangePasswordRequestDto request);
    }
}
