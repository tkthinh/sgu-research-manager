using Domain.Interfaces;

namespace Application.AuthorRegistrations
{
    public interface IAuthorRegistrationService : IGenericService<AuthorRegistrationDto>
    {
        Task<IEnumerable<AuthorRegistrationDto>> RegisterAuthorsForCurrentYear(List<Guid> authorIds, CancellationToken cancellationToken = default);
    }
}
