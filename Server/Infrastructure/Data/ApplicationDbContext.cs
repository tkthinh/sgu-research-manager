using System.Reflection.Emit;
using System.Text.Json;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
      public DbSet<AuthorRole> AuthorRoles { get; set; }
      public DbSet<Work> Works { get; set; }
      public DbSet<Author> Authors { get; set; }
      public DbSet<Assignment> Assignments { get; set; }
      public DbSet<Factor> Factors { get; set; }
      public DbSet<BookExtraOption> BookExtraOptions { get; set; }

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

         // Employee Relationships
         builder.Entity<Employee>()
             .HasOne(e => e.Department)
             .WithMany(d => d.Employees)
             .HasForeignKey(e => e.DepartmentId)
             .OnDelete(DeleteBehavior.Restrict);

         builder.Entity<Employee>()
             .HasOne(e => e.AcademicRank)
             .WithMany(a => a.Employees)
             .HasForeignKey(e => e.AcademicRankId)
             .OnDelete(DeleteBehavior.Restrict);

         builder.Entity<Employee>()
             .HasOne(e => e.OfficerRank)
             .WithMany(o => o.Employees)
             .HasForeignKey(e => e.OfficerRankId)
             .OnDelete(DeleteBehavior.Restrict);

         builder.Entity<Employee>()
             .HasOne(e => e.Field)
             .WithMany(f => f.Employees)
             .HasForeignKey(e => e.FieldId)
             .OnDelete(DeleteBehavior.Restrict);

         // Work Relationships
         builder.Entity<Work>()
             .Property(w => w.Source)
             .HasConversion<string>();

         builder.Entity<Work>()
             .Property(w => w.Details)
             .HasConversion(
                 v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                 v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, new JsonSerializerOptions()) ?? new Dictionary<string, string>()
             )
             .Metadata.SetValueComparer(new ValueComparer<Dictionary<string, string>>(
                 (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),  // Equality check
                 c => c.Aggregate(0, (hash, kvp) => HashCode.Combine(hash, kvp.Key.GetHashCode(), kvp.Value.GetHashCode())),  // Hash code
                 c => c.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)  // Clone
             ));

         builder.Entity<Work>()
             .HasOne(w => w.WorkType)
             .WithMany()
             .HasForeignKey(w => w.TypeId)
             .OnDelete(DeleteBehavior.Restrict);

         builder.Entity<Work>()
             .HasOne(w => w.WorkLevel)
             .WithMany()
             .HasForeignKey(w => w.LevelId)
             .OnDelete(DeleteBehavior.Restrict);

         builder.Entity<Work>()
             .HasOne(w => w.WorkStatus)
             .WithMany()
             .HasForeignKey(w => w.StatusId)
             .OnDelete(DeleteBehavior.Restrict);

         builder.Entity<Work>()
             .HasOne(w => w.ProofStatus)
             .WithMany()
             .HasForeignKey(w => w.ProofId)
             .OnDelete(DeleteBehavior.Restrict);

         builder.Entity<Work>()
             .HasOne(w => w.FieldForScoring)
             .WithMany()
             .HasForeignKey(w => w.ScoringFieldId)
             .OnDelete(DeleteBehavior.Restrict);

         // Author Relationships
         builder.Entity<Author>()
             .HasOne(a => a.Work)
             .WithMany(w => w.Authors)
             .HasForeignKey(a => a.WorkId)
             .OnDelete(DeleteBehavior.Cascade);

         builder.Entity<Author>()
             .HasOne(a => a.User)
             .WithOne()
             .HasForeignKey<Author>(a => a.UserId)
             .OnDelete(DeleteBehavior.Restrict);

         builder.Entity<Author>()
             .HasOne(a => a.AuthorRole)
             .WithMany(ar => ar.Authors)
             .HasForeignKey(a => a.AuthorRoleId)
             .HasPrincipalKey(ar => ar.Id)
             .OnDelete(DeleteBehavior.Restrict);

         builder.Entity<Author>()
             .HasOne(a => a.Purpose)
             .WithMany(p => p.Authors)
             .HasForeignKey(a => a.PurposeId)
             .HasPrincipalKey(p => p.Id)
             .OnDelete(DeleteBehavior.Restrict);

         // AuthorRole
         builder.Entity<AuthorRole>()
             .HasOne(ar => ar.WorkType)
             .WithMany(wt => wt.AuthorRoles)
             .HasForeignKey(ar => ar.WorkTypeId)
             .OnDelete(DeleteBehavior.Restrict);

          // Department
          builder.Entity<Department>()
             .HasMany(d => d.Assignments)
             .WithOne(a => a.Department)
             .HasForeignKey(a => a.DepartmentId)
             .OnDelete(DeleteBehavior.Restrict);

            // Purpose
          builder.Entity<Purpose>()
            .HasMany(p => p.Factors)
            .WithOne(f => f.Purpose)
            .HasForeignKey(f => f.PurposeId)
            .OnDelete(DeleteBehavior.Restrict);

            // WorkLevel
          builder.Entity<WorkLevel>()
            .HasMany(wl => wl.Factors)
            .WithOne(f => f.WorkLevel)
            .HasForeignKey(f => f.WorkLevelId)
            .OnDelete(DeleteBehavior.Restrict);

            // WorkType
          builder.Entity<WorkType>()
            .HasMany(wt => wt.Factors)
            .WithOne(f => f.WorkType)
            .HasForeignKey(f => f.WorkTypeId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<WorkType>()
            .HasMany(wt => wt.WorkLevels)
            .WithOne(wl => wl.WorkType)
            .HasForeignKey(wl => wl.WorkTypeId)
            .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình quan hệ 1‑n giữa WorkType và BookExtraOption
            builder.Entity<WorkType>()
            .HasMany(wt => wt.BookExtraOptions)
            .WithOne(beo => beo.WorkType)
            .HasForeignKey(beo => beo.WorkTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        }

    }
}

