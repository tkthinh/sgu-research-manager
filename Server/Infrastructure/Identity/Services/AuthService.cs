using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Auth;
using Application.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<AuthService> logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AuthService> logger)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.userService = userService;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }

        public async Task<object> LoginAsync(LoginRequestDto request)
        {
            var identityUser = await userManager.FindByNameAsync(request.Username);
            if (identityUser == null || !await userManager.CheckPasswordAsync(identityUser, request.Password))
                throw new UnauthorizedAccessException("Sai thông tin đăng nhập");

            if (!identityUser.IsApproved)
                throw new Exception("Tài khoản chưa được phê duyệt");

            // Build up those claims like a boss
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, identityUser.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await userManager.GetRolesAsync(identityUser);
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var user = await userService.GetUserByIdentityIdAsync(identityUser.Id);
            if (user != null)
                authClaims.Add(new Claim("id", user.Id.ToString()));

            // Generate JWT token
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(8),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var responseData = new
            {
                token = tokenString,
                expiration = token.ValidTo,
                user
            };

            return responseData;
        }

        public async Task<UserDto> RegisterAsync(RegisterRequestDto request)
        {
            // Check if username already exists
            var existingUser = await userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
                throw new Exception("Tên người dùng đã tồn tại");

            // Create new identity user
            var identityUser = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                IsApproved = false
            };

            var identityResult = await userManager.CreateAsync(identityUser, request.Password);
            if (!identityResult.Succeeded)
            {
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                throw new Exception($"Lỗi khi đăng ký: {errors}");
            }

            // Assign the "User" role
            var roleResult = await userManager.AddToRoleAsync(identityUser, "User");
            if (!roleResult.Succeeded)
            {
                var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception($"Lỗi khi đăng ký: {roleErrors}");
            }

            // Map to domain user DTO
            var userDto = new UserDto
            {
                FullName = request.FullName,
                UserName = identityUser.UserName,
                Email = identityUser.Email,
                PhoneNumber = identityUser.PhoneNumber,
                IdentityId = identityUser.Id,
                AcademicTitle = request.AcademicTitle,
                OfficerRank = request.OfficerRank,
                Specialization = request.Specialization,
                DepartmentId = request.DepartmentId,
                FieldId = request.FieldId
            };

            // Create the domain user and return the DTO
            var user = await userService.CreateAsync(userDto);
            userDto.Id = user.Id;
            return userDto;
        }

        public async Task ChangePasswordAsync(ChangePasswordRequestDto request)
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user == null)
                throw new Exception("Không tìm thấy thông tin người dùng");

            var userName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var identityUser = await userManager.FindByNameAsync(userName!);
            if (identityUser == null || !await userManager.CheckPasswordAsync(identityUser, request.CurrentPassword))
                throw new Exception("Mật khẩu cũ không trùng khớp");

            var result = await userManager.ChangePasswordAsync(identityUser, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
                throw new Exception("Lỗi khi thay đổi mật khẩu");
        }
    }
}
