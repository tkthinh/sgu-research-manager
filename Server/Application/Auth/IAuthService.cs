using Application.Shared.Response;
using Application.Users;

namespace Application.Auth
{
    public interface IAuthService
    {
        Task<object> LoginAsync(LoginRequestDto request);
        Task<UserDto> RegisterAsync(RegisterRequestDto request);
        Task ChangePasswordAsync(ChangePasswordRequestDto request);
    }

}
