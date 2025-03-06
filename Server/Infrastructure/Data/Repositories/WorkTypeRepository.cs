using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
   public class WorkTypeRepository : GenericRepository<WorkType>, IWorkTypeRepository
   {
      public WorkTypeRepository(ApplicationDbContext context)
          : base(context)
      {
      }

      public async Task<IEnumerable<WorkType>> GetWorkTypesWithLevelCountAsync(CancellationToken cancellationToken = default)
      {
         return await context.WorkTypes
            .Include(wt => wt.WorkLevels)
            .ToListAsync(cancellationToken);
      }
   }
}
