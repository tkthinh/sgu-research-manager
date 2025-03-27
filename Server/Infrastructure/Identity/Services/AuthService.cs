using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Auth;
using Application.Shared.Response;
using Application.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IUserService userService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IUserService userService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.userService = userService;
        }

        public async Task<ApiResponse<object>> LoginAsync(LoginRequestDto request)
        {
            var identityUser = await userManager.FindByNameAsync(request.Username);
            if (identityUser == null || !await userManager.CheckPasswordAsync(identityUser, request.Password))
            {
                return new ApiResponse<object>(false, "Sai thông tin đăng nhập");
            }

            if (!identityUser.IsApproved)
            {
                return new ApiResponse<object>(false, "Tài khoản chưa được phê duyệt");
            }

            // Create user claims
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, identityUser.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add roles as claims:
            var userRoles = await userManager.GetRolesAsync(identityUser);
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Add user id as a claim
            var user = await userService.GetUserByIdentityIdAsync(identityUser.Id);
            if (user != null)
            {
                authClaims.Add(new Claim("id", user.Id.ToString()));
            }

            // Generate the token
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

            return new ApiResponse<object>(true, "Đăng nhập thành công", responseData);
        }

        public async Task<ApiResponse<UserDto>> RegisterAsync(RegisterRequestDto request)
        {
            try
            {
                // Check if username is already taken
                var existingUser = await userManager.FindByNameAsync(request.UserName);
                if (existingUser != null)
                {
                    return new ApiResponse<UserDto>(false, "Tên người dùng đã tồn tại");
                }

                // Create the Identity user with the provided username.
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
                    return new ApiResponse<UserDto>(false, $"Lỗi khi đăng ký: {errors}");
                }

                // Add the "User" role to the newly registered user.
                var roleResult = await userManager.AddToRoleAsync(identityUser, "User");
                if (!roleResult.Succeeded)
                {
                    var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    return new ApiResponse<UserDto>(false, $"Lỗi khi đăng ký: {roleErrors}");
                }

                // Map to your domain user DTO.
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

                // Create the domain user.
                var user = await userService.CreateAsync(userDto);
                userDto.Id = user.Id;

                return new ApiResponse<UserDto>(true, "Đăng ký tài khoản thành công", userDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserDto>(false, "Lỗi khi đăng ký", null);
            }
        }
    }
}
