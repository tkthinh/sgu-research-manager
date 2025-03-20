using Domain.Entities;

namespace Domain.Interfaces
{
   public interface IWorkTypeRepository : IGenericRepository<WorkType>
   {
      Task<IEnumerable<WorkType>> GetWorkTypesWithDetailsCountAsync(CancellationToken cancellationToken = default);
   }
}
