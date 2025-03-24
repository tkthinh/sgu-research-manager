using Domain.Entities;

namespace Domain.Interfaces
{
   public interface IAssignmentRepository
   {
      Task<IEnumerable<Assignment>> GetAssignmentsWithDetailsAsync(CancellationToken cancellationToken = default);
   }
}
