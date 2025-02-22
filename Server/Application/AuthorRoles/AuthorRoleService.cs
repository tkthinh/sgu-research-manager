using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.AuthorRoles
{
    public class AuthorRoleService : GenericCachedService<AuthorRoleDto, AuthorRole>, IAuthorRoleService
    {
        public AuthorRoleService(
            IUnitOfWork unitOfWork,
            IGenericMapper<AuthorRoleDto, AuthorRole> mapper,
            IDistributedCache cache
        ) : base(unitOfWork, mapper, cache)
        {
        }
    }
}
