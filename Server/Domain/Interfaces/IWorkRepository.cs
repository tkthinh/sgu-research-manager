using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWorkRepository : IGenericRepository<Work>
    {
        Task<IEnumerable<Work>> GetWorksWithAuthorsAsync();
        Task<Work?> GetWorkWithAuthorsByIdAsync(Guid id);
    }
}
