using Application.AuthorRoles;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

public class AuthorRoleService : GenericCachedService<AuthorRoleDto, AuthorRole>, IAuthorRoleService
{
    public AuthorRoleService(
        IUnitOfWork unitOfWork,
        IGenericMapper<AuthorRoleDto, AuthorRole> mapper,
        IDistributedCache cache,
        ILogger<AuthorRoleService> logger
        )
        : base(unitOfWork, mapper, cache, logger)
    {
    }

    public async Task<IEnumerable<AuthorRoleDto>> GetByWorkTypeIdAsync(Guid workTypeId)
    {
        var authorRoles = await unitOfWork.Repository<AuthorRole>()
            .FindAsync(ar => ar.WorkTypeId == workTypeId);

        return mapper.MapToDtos(authorRoles);
    }
}
