using Domain.Entities;

namespace Domain.Interfaces
{
   public interface IWorkTypeRepository : IGenericRepository<WorkType>
   {
      Task<IEnumerable<WorkType>> GetWorkTypesWithLevelCountAsync(CancellationToken cancellationToken = default);
   }
}
