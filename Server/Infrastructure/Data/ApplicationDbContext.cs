using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
   public class ApplicationDbContext : DbContext
   {
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
      {
      }

      public DbSet<Employee> Employees { get; set; }
      public DbSet<Department> Departments { get; set; }
      public DbSet<Purpose> Purposes { get; set; }
      public DbSet<WorkLevel> WorkLevels { get; set; }
      public DbSet<Field> Fields { get; set; }
      public DbSet<AcademicRank> AcademicRanks { get; set; }
      public DbSet<OfficerRank> OfficerRanks { get; set; }
      public DbSet<WorkStatus> WorkStatuses { get; set; }
      public DbSet<WorkType> WorkTypes { get; set; }
      public DbSet<ProofStatus> ProofStatuses { get; set; }

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

      protected override void OnModelCreating(ModelBuilder builder)
      {
         base.OnModelCreating(builder);

         // Configure relationships
         builder.Entity<Employee>()
            .HasOne(u => u.Department)
            .WithMany(u => u.Employees)
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

         builder.Entity<Employee>()
             .HasOne(u => u.AcademicRank)
             .WithMany(u => u.Employees)
             .HasForeignKey(u => u.AcademicRankId)
             .OnDelete(DeleteBehavior.Restrict);

         builder.Entity<Employee>()
             .HasOne(u => u.OfficerRank)
             .WithMany(u => u.Employees)
             .HasForeignKey(u => u.OfficerRankId)
             .OnDelete(DeleteBehavior.Restrict);

         builder.Entity<Employee>()
             .HasOne(u => u.Field)
             .WithMany(u => u.Employees)
             .HasForeignKey(u => u.FieldId)
             .OnDelete(DeleteBehavior.Restrict);
      }
   }
}

