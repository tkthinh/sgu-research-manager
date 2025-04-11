using Domain.Interfaces;

namespace Application.Authors
{
    public interface IAuthorService : IGenericService<AuthorDto>
    {
        Task<IEnumerable<AuthorDto>> GetAllRegistableAuthorsOfUser(Guid userId, CancellationToken cancellationToken = default);

    }
}
