using Application.Authors;
using Application.Shared.Services;
using Domain.Interfaces;

namespace Application.Works
{
    public interface IWorkService : IGenericService<WorkDto>
    {
        Task<WorkDto> CreateWorkWithAuthorAsync(CreateWorkRequestDto request, CancellationToken cancellationToken = default);
        Task<IEnumerable<AuthorDto>> GetWorksByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkDto>> GetWorksByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
        Task SetMarkedForScoringAsync(Guid authorId, bool marked, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkDto>> GetAllWorksWithAuthorsAsync(CancellationToken cancellationToken = default);
        Task<WorkDto?> GetWorkByIdWithAuthorsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<WorkDto> AddCoAuthorAsync(Guid workId, AddCoAuthorRequestDto request, CancellationToken cancellationToken = default); 
        Task<WorkDto> UpdateWorkAdminAsync(Guid workId, AdminUpdateWorkRequestDto request, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkDto>> SearchWorksAsync(string title, CancellationToken cancellationToken = default);
        Task DeleteWorkAsync(Guid workId, CancellationToken cancellationToken = default);

        Task<WorkDto> UpdateWorkByAuthorAsync(Guid workId, UpdateWorkByAuthorRequestDto request, Guid userId, CancellationToken cancellationToken = default);

        Task<Guid> GetUserIdFromUserNameAsync(string userName);
    }
}