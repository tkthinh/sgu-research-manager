using Application.Users;
using Application.Shared.Response;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Identity;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger<UsersController> logger;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(IUserService userService,
                               ILogger<UsersController> logger,
                               UserManager<ApplicationUser> userManager)
        {
            this.userService = userService;
            this.logger = logger;
            this.userManager = userManager;
        }

      [HttpGet]
      public async Task<ActionResult<ApiResponse<IEnumerable<UserWithRoleDto>>>> GetUsers()
      {
         var users = (await userService.GetAllAsync()).ToList();

         var usersWithRoles = new List<UserWithRoleDto>();

         // For each domain user, retrieve the associated Identity roles and set the Role property.
         foreach (var user in users)
         {
            var dto = new UserWithRoleDto
            {
               Id = user.Id,
               DepartmentId = user.DepartmentId,
               FieldId = user.FieldId,
               UserName = user.UserName,
               FullName = user.FullName,
               Email = user.Email,
               PhoneNumber = user.PhoneNumber,
               Specialization = user.Specialization,
               AcademicTitle = user.AcademicTitle,
               OfficerRank = user.OfficerRank,
               DepartmentName = user.DepartmentName,
               FieldName = user.FieldName,
               IdentityId = user.IdentityId
            };

            if (!string.IsNullOrEmpty(user.IdentityId))
            {
               var identityUser = await userManager.FindByIdAsync(user.IdentityId);
               if (identityUser != null)
               {
                  dto.IsApproved = identityUser.IsApproved;

                  var roles = await userManager.GetRolesAsync(identityUser);
                  dto.Role = roles.Any() ? roles.First() : "No Role";
               }
               else
               {
                  dto.Role = "Unknown";
               }
            }
            else
            {
               dto.Role = "Unknown";
            }

            usersWithRoles.Add(dto);
         }

         return Ok(new ApiResponse<IEnumerable<UserWithRoleDto>>(
             true,
             "Lấy danh sách người dùng thành công",
             usersWithRoles
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

            return Ok(new ApiResponse<UserDto>(
                      true,
                      "Lấy thông tin người dùng thành công",
                      user
                  ));
        }

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
            user.FullName = requestDto.FullName;
            user.Email = requestDto.Email;
            user.PhoneNumber = requestDto.PhoneNumber;
            user.AcademicTitle = requestDto.AcademicTitle;
            user.OfficerRank = requestDto.OfficerRank;
            user.Specialization = requestDto.Specialization;
            user.DepartmentId = requestDto.DepartmentId;
            user.FieldId = requestDto.FieldId;

            if (!string.IsNullOrEmpty(user.IdentityId))
            {
               var identityUser = await userManager.FindByIdAsync(user.IdentityId);
               if (identityUser == null)
               {
                  return NotFound(new ApiResponse<object>(false, "Không tìm thấy thông tin Identity user"));
               }

               // Approve the user account.
               identityUser.IsApproved = requestDto.IsApproved;

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

               var updateResult = await userManager.UpdateAsync(identityUser);
               if (!updateResult.Succeeded)
               {
                  var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                  return BadRequest(new ApiResponse<object>(false, $"Lỗi phê duyệt người dùng: {errors}"));
               }
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

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserSearchDto>>>> SearchUsers([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrEmpty(searchTerm))
                {
                    return BadRequest(new ApiResponse<object>(false, "Từ khóa tìm kiếm không được để trống"));
                }

                var users = await userService.SearchUsersAsync(searchTerm);
                return Ok(new ApiResponse<IEnumerable<UserSearchDto>>(
                    true,
                    "Tìm kiếm người dùng thành công",
                    users
                ));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Lỗi khi tìm kiếm người dùng");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetUsersByDepartmentId([FromRoute] Guid departmentId)
        {
            try
            {
                // Kiểm tra DepartmentId hợp lệ
                if (departmentId == Guid.Empty)
                {
                    return BadRequest(new ApiResponse<IEnumerable<UserDto>>(false, "DepartmentId không hợp lệ"));
                }

                // Lấy danh sách người dùng theo DepartmentId
                var usersList = (await userService.GetUsersByDepartmentIdAsync(departmentId)).ToList();

                // Nếu không có người dùng nào
                if (!usersList.Any())
                {
                    return Ok(new ApiResponse<IEnumerable<UserDto>>(
                        true,
                        "Không tìm thấy người dùng nào trong phòng ban này",
                        usersList
                    ));
                }

                return Ok(new ApiResponse<IEnumerable<UserDto>>(
                    true,
                    "Lấy danh sách người dùng theo phòng ban thành công",
                    usersList
                ));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Lỗi khi lấy danh sách người dùng theo phòng ban {DepartmentId}", departmentId);
                return BadRequest(new ApiResponse<IEnumerable<UserDto>>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }
    }
}