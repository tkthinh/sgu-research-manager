using Domain.Interfaces;

namespace Application.Users
{
    public interface IUserService : IGenericService<UserDto>
    {
        Task<UserDto?> GetUserByIdentityIdAsync(string IdentityId);
        Task<UserConversionResultRequestDto> GetUserConversionResultAsync(Guid userId);
        Task<IEnumerable<UserSearchDto>> SearchUsersAsync(string searchTerm, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserDto>> GetUsersByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
    }
}
