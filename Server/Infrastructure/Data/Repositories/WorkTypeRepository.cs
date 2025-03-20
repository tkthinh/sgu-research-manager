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

      public async Task<IEnumerable<WorkType>> GetWorkTypesWithDetailsCountAsync(CancellationToken cancellationToken = default)
      {
         return await context.WorkTypes
            .AsNoTracking()
            .Include(wt => wt.WorkLevels)
            .Include(wt => wt.Purposes)
            .Include(wt => wt.AuthorRoles)
            .Include(wt => wt.Factors)
            .Include(wt => wt.SCImagoFields)
            .OrderBy(wt => wt.Name)
            .ToListAsync(cancellationToken);
      }
   }
}
