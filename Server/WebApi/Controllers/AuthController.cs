using Application.Auth;
using Application.Shared.Response;
using Application.Users;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var data = await authService.LoginAsync(request);
            return Ok(new ApiResponse<object>(true, "Đăng nhập thành công", data));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ApiResponse<object>(false, ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message));
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] RegisterRequestDto request)
    {
        try
        {
            var userDto = await authService.RegisterAsync(request);
            return Ok(new ApiResponse<UserDto>(true, "Đăng ký tài khoản thành công", userDto));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserDto>(false, ex.Message));
        }
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
    {
        try
        {
            await authService.ChangePasswordAsync(request);
            return Ok(new ApiResponse<object>(true, "Đổi mật khẩu thành công"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message));
        }
    }
}
