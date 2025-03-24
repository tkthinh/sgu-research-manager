using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
   public class UserRepository : GenericRepository<User>, IUserRepository
   {
      public UserRepository(ApplicationDbContext context)
          : base(context)
      {
      }

      public async Task<User?> GetUserByIdWithDetailsAsync(Guid id)
      {
         return await context.Users
            .Include(u => u.Department)
            .Include(u => u.Field)
            .FirstOrDefaultAsync(u => u.Id == id);
      }

      public async Task<IEnumerable<User>> GetUsersWithDetailsAsync()
      {
         return await context.Users
             .Include(u => u.Department)
             .Include(u => u.Field)
             .ToListAsync();
      }

      public async Task<User?> GetUserByIdentityIdAsync(string identityId)
      {
         return await context.Users
            .Include(u => u.Department)
            .Include(u => u.Field)
            .FirstOrDefaultAsync(u => u.IdentityId == identityId);
      }


   }
}
