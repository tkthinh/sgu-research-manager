using Application.AuthorRoles;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

public class AuthorRoleService : GenericCachedService<AuthorRoleDto, AuthorRole>, IAuthorRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericMapper<AuthorRoleDto, AuthorRole> _mapper;

    public AuthorRoleService(
        IUnitOfWork unitOfWork,
        IGenericMapper<AuthorRoleDto, AuthorRole> mapper,
        IDistributedCache cache)
        : base(unitOfWork, mapper, cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AuthorRoleDto>> GetByWorkTypeIdAsync(Guid workTypeId)
    {
        var authorRoles = await _unitOfWork.Repository<AuthorRole>()
            .FindAsync(ar => ar.WorkTypeId == workTypeId);

        return _mapper.MapToDtos(authorRoles);
    }
}
