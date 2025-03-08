using Application.Authors;
using Application.Shared.Services;
using Domain.Interfaces;

namespace Application.Works
{
    public interface IWorkService : IGenericService<WorkDto>
    {
        Task<IEnumerable<WorkDto>> SearchWorksAsync(string title, CancellationToken cancellationToken = default);
        Task<WorkDto> CreateWorkWithAuthorAsync(CreateWorkRequestDto request, CancellationToken cancellationToken = default);
        Task<AuthorDto> AddAuthorToWorkAsync(Guid workId, CreateAuthorRequestDto request, CancellationToken cancellationToken = default);
        Task UpdateAuthorAsync(Guid authorId, AuthorDto authorDto, CancellationToken cancellationToken = default);
        Task<IEnumerable<AuthorDto>> GetWorksByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task UpdateWorkFinalHourAsync(Guid workId, int finalWorkHour, CancellationToken cancellationToken = default);
        Task SetMarkedForScoringAsync(Guid authorId, bool marked, CancellationToken cancellationToken = default);
        Task DeleteAuthorAsync(Guid authorId, CancellationToken cancellationToken = default);
    }
}