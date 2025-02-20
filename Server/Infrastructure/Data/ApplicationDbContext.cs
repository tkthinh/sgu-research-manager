using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
   public class ApplicationDbContext : DbContext
   {
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
      {
      }

      public DbSet<Department> Departments { get; set; }
      public DbSet<Purpose> Purposes { get; set; }

      public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
      {
         foreach (var entry in ChangeTracker.Entries<BaseEntity>())
         {
            switch (entry.State)
            {
               case EntityState.Added:
                  entry.Entity.CreatedDate = DateTime.UtcNow;
                  break;
               case EntityState.Modified:
                  entry.Entity.ModifiedDate = DateTime.UtcNow;
                  break;
            }
         }

         return base.SaveChangesAsync(cancellationToken);
      }
   }
}
