using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Auth;
using Application.Users;
using Application.Shared.Response;
using Infrastructure.Identity;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
   private readonly UserManager<ApplicationUser> userManager;
   private readonly IUserService userService;
   private readonly IConfiguration configuration;

   public AuthController(
      UserManager<ApplicationUser> userManager,
      IUserService userService,
      IConfiguration configuration
      )
   {
      this.userManager = userManager;
      this.userService = userService;
      this.configuration = configuration;
   }

   [HttpPost("login")]
   public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
   {
      var identityUser = await userManager.FindByNameAsync(model.Username);

      if (identityUser == null || !await userManager.CheckPasswordAsync(identityUser, model.Password))
      {
         return Unauthorized(new ApiResponse<object>(false, "Sai thông tin đăng nhập", null));
      }

      if (!identityUser.IsApproved)
      {
         return Unauthorized(new ApiResponse<object>(false, "Tài khoản chưa được phê duyệt", null));
      }

      // Create user claims
      var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, identityUser.UserName!),
            new Claim(ClaimTypes.Email, identityUser.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

      // Optionally, add roles as claims:
      var userRoles = await userManager.GetRolesAsync(identityUser);
      authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

      var user = await userService.GetUserByIdentityIdAsync(identityUser.Id);
      if (user is not null)
      {
         user.Role = userRoles.Any() ? userRoles.First() : "No Role";
         authClaims.Add(new Claim("id", user.Id.ToString())); // Thêm UserId vào token
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

      var response = new ApiResponse<object>(
          true,
          "Đăng nhập thành công",
          new
          {
             token = new JwtSecurityTokenHandler().WriteToken(token),
             expiration = token.ValidTo,
             user
          });

      return Ok(response);
   }


   [HttpPost("register")]
   public async Task<ActionResult<UserDto>> Register([FromBody] RegisterRequestDto request)
   {
      try
      {
         // Check if username is already taken
         var existingUser = await userManager.FindByNameAsync(request.UserName);
         if (existingUser != null)
         {
            return BadRequest(new ApiResponse<object>(false, "Tên người dùng đã tồn tại"));
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
            return BadRequest(new ApiResponse<object>(false, $"Lỗi khi đăng ký: {errors}"));
         }

         // Add the "User" role to the newly registered user.
         var roleResult = await userManager.AddToRoleAsync(identityUser, "User");
         if (!roleResult.Succeeded)
         {
            var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            return BadRequest(new ApiResponse<object>(false, $"Lỗi khi đăng ký: {roleErrors}"));
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
            FieldId = request.FieldId,
            Role = "User"
         };

         // Create the domain user.
         var user = await userService.CreateAsync(userDto);

         userDto.Id = user.Id;

         return Ok(new ApiResponse<UserDto>(true, "Đăng ký tài khoản thành công", userDto));
      }
      catch (Exception ex)
      {
         return StatusCode(500, new ApiResponse<object>(false, "Lỗi khi đăng ký", ex.Message));
      }
   }
}