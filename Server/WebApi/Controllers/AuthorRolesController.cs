using Application.AuthorRoles;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorRolesController : ControllerBase
    {
        private readonly IAuthorRoleService authorRoleService;
        private readonly ILogger<AuthorRolesController> logger;

        public AuthorRolesController(IAuthorRoleService authorRoleService, ILogger<AuthorRolesController> logger)
        {
            this.authorRoleService = authorRoleService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuthorRoleDto>>>> GetAuthorRoles()
        {
            var roles = await authorRoleService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<AuthorRoleDto>>(
                true,
                "Lấy dữ liệu vai trò tác giả thành công",
                roles
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AuthorRoleDto>>> GetAuthorRole([FromRoute] Guid id)
        {
            var role = await authorRoleService.GetByIdAsync(id);
            if (role is null)
            {
                return NotFound(new ApiResponse<AuthorRoleDto>(false, "Không tìm thấy vai trò tác giả"));
            }
            return Ok(new ApiResponse<AuthorRoleDto>(
                true,
                "Lấy dữ liệu vai trò tác giả thành công",
                role
            ));
        }

        [HttpGet("work-type/{workTypeId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuthorRoleDto>>>> GetAuthorRolesByWorkTypeId([FromRoute] Guid workTypeId)
        {
            if (workTypeId == Guid.Empty)
            {
                return BadRequest(new ApiResponse<IEnumerable<AuthorRoleDto>>(false, "WorkTypeId không hợp lệ"));
            }

            var roles = await authorRoleService.GetByWorkTypeIdAsync(workTypeId);
            if (roles == null || !roles.Any())
            {
                return Ok(new ApiResponse<IEnumerable<AuthorRoleDto>>(
                    true,
                    "Không tìm thấy vai trò tác giả cho loại công trình này",
                    Enumerable.Empty<AuthorRoleDto>()
                ));
            }

            return Ok(new ApiResponse<IEnumerable<AuthorRoleDto>>(
                true,
                "Lấy dữ liệu vai trò tác giả theo loại công trình thành công",
                roles
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<AuthorRoleDto>>> CreateAuthorRole([FromBody] CreateAuthorRoleRequestDto requestDto)
        {
            try
            {
                var dto = new AuthorRoleDto
                {
                    Name = requestDto.Name,
                    IsMainAuthor = requestDto.IsMainAuthor,
                    WorkTypeId = requestDto.WorkTypeId
                };

                var role = await authorRoleService.CreateAsync(dto);
                var response = new ApiResponse<AuthorRoleDto>(
                    true,
                    "Tạo vai trò tác giả thành công",
                    role
                );

                return CreatedAtAction(nameof(GetAuthorRole), new { id = role.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error creating author role");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateAuthorRole([FromRoute] Guid id, [FromBody] UpdateAuthorRoleRequestDto request)
        {
            try
            {
                var existingRole = await authorRoleService.GetByIdAsync(id);
                if (existingRole is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy vai trò tác giả"));
                }

                existingRole.Name = request.Name;
                existingRole.IsMainAuthor = request.IsMainAuthor;
                existingRole.WorkTypeId = request.WorkTypeId;
                await authorRoleService.UpdateAsync(existingRole);

                return Ok(new ApiResponse<object>(true, "Cập nhật vai trò tác giả thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error updating author role");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteAuthorRole([FromRoute] Guid id)
        {
            try
            {
                var existingRole = await authorRoleService.GetByIdAsync(id);
                if (existingRole is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy vai trò tác giả"));
                }

                await authorRoleService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa vai trò tác giả thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting author role");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
