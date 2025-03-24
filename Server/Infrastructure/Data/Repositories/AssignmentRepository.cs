using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
   public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
   {
      public AssignmentRepository(ApplicationDbContext context) : base(context)
      {
      }

      public async Task<IEnumerable<Assignment>> GetAssignmentsWithDetailsAsync(CancellationToken cancellationToken = default)
      {
         return await context.Assignments
             .Include(a => a.User)
             .Include(a => a.Department)
             .ToListAsync(cancellationToken);
      }
   }
   
   }

