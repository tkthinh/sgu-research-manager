using Domain.Interfaces;

namespace Application.Assignments
{
    public interface IAssignmentService : IGenericService<AssignmentDto>
    {
      public Task<List<AssignmentDto>> CreateAsync(CreateAssignmentRequestDto request, CancellationToken cancellationToken = default);
      public Task<List<AssignmentDto>> UpdateAsync(UpdateAssignmentRequestDto request, CancellationToken cancellationToken = default);
      public Task<List<AssignmentDto>> GetAllAssignmentByUserAsync(Guid userId, CancellationToken cancellationToken = default);
      public Task DeleteAllAssignmentsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
   }
}
