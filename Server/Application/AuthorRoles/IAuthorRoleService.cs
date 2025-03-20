using Domain.Interfaces;

namespace Application.AuthorRoles
{
    public interface IAuthorRoleService : IGenericService<AuthorRoleDto>
    {
        Task<IEnumerable<AuthorRoleDto>> GetAuthorRolesByWorkTypeIdAsync(Guid workTypeId);
    }
}
