namespace Application.Works
{
    public interface IWorkQueryService
    {
        Task<WorkDto?> GetWorkByIdWithAuthorsAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<IEnumerable<WorkDto>> GetWorksAsync(WorkFilter filter, CancellationToken cancellationToken = default);
    }
} 