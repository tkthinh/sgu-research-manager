using Application.Authors;
using Domain.Interfaces;

namespace Application.Works
{
    public interface IWorkService : IGenericService<WorkDto>
    {
        Task<WorkDto> CreateWorkWithAuthorAsync(CreateWorkRequestDto request, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkDto>> GetWorksByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkDto>> GetWorksByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
        Task SetMarkedForScoringAsync(Guid authorId, bool marked, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkDto>> GetAllWorksWithAuthorsAsync(CancellationToken cancellationToken = default);
        Task<WorkDto?> GetWorkByIdWithAuthorsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<WorkDto> UpdateWorkByAdminAsync(Guid workId, Guid userId, UpdateWorkByAdminRequestDto request, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkDto>> SearchWorksAsync(string title, CancellationToken cancellationToken = default);
        Task DeleteWorkAsync(Guid workId, Guid userId, CancellationToken cancellationToken = default);
        Task<WorkDto> UpdateWorkByAuthorAsync(Guid workId, UpdateWorkByAuthorRequestDto request, Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkDto>> GetWorksByCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}