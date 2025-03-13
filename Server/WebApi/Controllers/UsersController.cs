using Application.Users;
using Application.Shared.Response;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger<UsersController> logger;
        private readonly UserManager<IdentityUser> userManager;

        public UsersController(IUserService userService,
                               ILogger<UsersController> logger,
                               UserManager<IdentityUser> userManager)
        {
            this.userService = userService;
            this.logger = logger;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetUsers()
        {
            var usersList = (await userService.GetAllAsync()).ToList();

            // For each domain user, retrieve the associated Identity roles and set the Role property.
            foreach (var user in usersList)
            {
                var identityUser = await userManager.FindByIdAsync(user.IdentityId);
                if (identityUser != null)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);
                    user.Role = roles.Any() ? roles.First() : "No Role";
                }
                else
                {
                    user.Role = "Unknown";
                }
            }

            return Ok(new ApiResponse<IEnumerable<UserDto>>(
                true,
                "Lấy danh sách người dùng thành công",
                usersList
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUser([FromRoute] Guid id)
        {
            var user = await userService.GetByIdAsync(id);
            if (user is null)
            {
                return NotFound(new ApiResponse<UserDto>(false, "Không tìm thấy người dùng"));
            }

            var identityUser = await userManager.FindByIdAsync(user.IdentityId);
            if (identityUser != null)
            {
                // You can join multiple roles if necessary. Here we take the first role.
                var roles = await userManager.GetRolesAsync(identityUser);
                user.Role = roles.Any() ? roles.First() : "No Role";
            }
            else
            {
                user.Role = "Unknown";
            }

            return Ok(new ApiResponse<UserDto>(
                      true,
                      "Lấy thông tin người dùng thành công",
                      user
                  ));
        }

        // Self update endpoint for a user to update their info without changing role.
        [HttpPut("me")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> UpdateMyProfile([FromBody] UpdateUserRequestDto requestDto)
        {
            // Get the current Identity user.
            var currentIdentityUser = await userManager.GetUserAsync(User);
            if (currentIdentityUser == null)
            {
                return Unauthorized(new ApiResponse<object>(false, "Không tìm thấy người dùng hiện tại"));
            }

            var user = await userService.GetUserByIdentityIdAsync(currentIdentityUser.Id);
            if (user == null)
            {
                return NotFound(new ApiResponse<object>(false, "Không tìm thấy thông tin người dùng"));
            }

            // Update only allowed fields.
            user.FullName = requestDto.FullName;
            user.AcademicTitle = requestDto.AcademicTitle;
            user.OfficerRank = requestDto.OfficerRank;
            user.DepartmentId = requestDto.DepartmentId;
            user.FieldId = requestDto.FieldId;

            await userService.UpdateAsync(user);
            return Ok(new ApiResponse<object>(true, "Cập nhật thông tin thành công"));
        }

        // Admin update endpoint for updating all user info including role.
        [HttpPut("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> AdminUpdateUser([FromRoute] Guid id, [FromBody] UpdateUserAdminRequestDto requestDto)
        {
            try
            {
                var user = await userService.GetByIdAsync(id);
                if (user is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy người dùng"));
                }
                // Update domain user properties.
                user.UserName = requestDto.UserName;
                user.FullName = requestDto.FullName;
                user.AcademicTitle = requestDto.AcademicTitle;
                user.OfficerRank = requestDto.OfficerRank;
                user.DepartmentId = requestDto.DepartmentId;
                user.FieldId = requestDto.FieldId;

                // Update the Identity user role.
                var identityUser = await userManager.FindByIdAsync(user.IdentityId);
                if (identityUser == null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy thông tin Identity user"));
                }
                // Remove all existing roles.
                var currentRoles = await userManager.GetRolesAsync(identityUser);
                await userManager.RemoveFromRolesAsync(identityUser, currentRoles);
                // Add the new role from the DTO.
                string newRole = requestDto.Role.ToString();
                var roleResult = await userManager.AddToRoleAsync(identityUser, newRole);
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    return BadRequest(new ApiResponse<object>(false, $"Lỗi cập nhật role: {errors}"));
                }

                await userService.UpdateAsync(user);
                return Ok(new ApiResponse<object>(true, "Cập nhật thông tin người dùng thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating user");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteUser([FromRoute] Guid id)
        {
            try
            {
                var existingUser = await userService.GetByIdAsync(id);
                if (existingUser is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy người dùng"));
                }

                await userService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa người dùng thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting user");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpGet("conversionresult/{userId}")]
        public async Task<ActionResult<ApiResponse<UserConversionResultRequestDto>>> GetUserConversionResult([FromRoute] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest(new ApiResponse<UserConversionResultRequestDto>(false, "UserId không hợp lệ"));
            }

            var result = await userService.GetUserConversionResultAsync(userId);
            if (result == null)
            {
                return Ok(new ApiResponse<UserConversionResultRequestDto>(
                    true,
                    "Không tìm thấy kết quả quy đổi cho người dùng này",
                    new UserConversionResultRequestDto
                    {
                        UserId = userId,
                        UserName = "Unknown",
                        ConversionResults = new ConversionDetailsRequestDto
                        {
                            DutyHourConversion = new ConversionItemRequestDto { TotalWorks = 0, TotalConvertedHours = 0, TotalCalculatedHours = 0 },
                            OverLimitConversion = new ConversionItemRequestDto { TotalWorks = 0, TotalConvertedHours = 0, TotalCalculatedHours = 0 },
                            ResearchProductConversion = new ConversionItemRequestDto { TotalWorks = 0, TotalConvertedHours = 0, TotalCalculatedHours = 0 },
                            TotalWorks = 0,
                            TotalCalculatedHours = 0
                        }
                    }
                ));
            }

            return Ok(new ApiResponse<UserConversionResultRequestDto>(
                true,
                "Lấy kết quả quy đổi thành công",
                result
            ));
        }

    }
}
