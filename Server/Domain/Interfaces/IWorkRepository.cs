using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWorkRepository : IGenericRepository<Work>
    {
        Task<IEnumerable<Work>> GetWorksWithAuthorsAsync();
        Task<Work?> GetWorkWithAuthorsByIdAsync(Guid id);
        Task<IEnumerable<Work>> FindWorksWithAuthorsAsync(string title);
        Task<IEnumerable<Work>> GetWorksByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Work>> GetWorksWithAuthorsByIdsAsync(IEnumerable<Guid> workIds, CancellationToken cancellationToken = default);
    }
}
