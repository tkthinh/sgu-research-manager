using System.Text.Json;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Infrastructure.Data.Seeding;
using System.Reflection.Emit;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Purpose> Purposes { get; set; }
        public DbSet<WorkLevel> WorkLevels { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<WorkType> WorkTypes { get; set; }
        public DbSet<AuthorRole> AuthorRoles { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Factor> Factors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SCImagoField> SCImagoFields { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }
        public DbSet<WorkAuthor> WorkAuthors { get; set; }
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<AuthorRegistration> AuthorRegistrations { get; set; }

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

            // User Relationships
            builder.Entity<User>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasOne(e => e.Field)
                .WithMany(f => f.Users)
                .HasForeignKey(e => e.FieldId)
                .OnDelete(DeleteBehavior.Restrict);

            // Work Relationships
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
                .HasForeignKey(w => w.WorkTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Work>()
                .HasOne(w => w.WorkLevel)
                .WithMany()
                .HasForeignKey(w => w.WorkLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            // Author Relationships
            builder.Entity<Author>()
                .HasOne(a => a.Work)
                .WithMany(w => w.Authors)
                .HasForeignKey(a => a.WorkId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Author>()
                .HasOne(a => a.User)
                .WithMany(u => u.Authors)
                .HasForeignKey(a => a.UserId)
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

            builder.Entity<Author>()
                .Property(a => a.ProofStatus)
                .HasConversion<string>();

            builder.Entity<Author>()
                .HasOne(a => a.SCImagoField)
                .WithMany()
                .HasForeignKey(a => a.SCImagoFieldId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Author>()
                .HasOne(a => a.Field)
                .WithMany()
                .HasForeignKey(a => a.FieldId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Author>()
                .HasIndex(a => new { a.WorkId, a.UserId })
                .IsUnique();

            builder.Entity<Author>()
            .Property(a => a.AuthorHour)
            .HasPrecision(10, 1); // Precision = 10, Scale = 1 (1 chữ số thập phân)

            // WorkAuthor
            builder.Entity<WorkAuthor>()
                .HasKey(wa => new { wa.WorkId, wa.UserId });

            builder.Entity<WorkAuthor>()
                .HasOne(wa => wa.Work)
                .WithMany(w => w.WorkAuthors)
                .HasForeignKey(wa => wa.WorkId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<WorkAuthor>()
                .HasOne(wa => wa.User)
                .WithMany()
                .HasForeignKey(wa => wa.UserId)
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

            // Assignments
            builder.Entity<Assignment>()
                .HasOne(a => a.Department)
                .WithMany(d => d.Assignments)
                .HasForeignKey(a => a.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Assignment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Assignments)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // AcademicYear
            builder.Entity<AcademicYear>()
                .HasMany(a => a.SystemConfigs)
                .WithOne(sc => sc.AcademicYear)
                .HasForeignKey(sc => sc.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);

            // SystemConfig
            builder.Entity<SystemConfig>()
                .HasQueryFilter(sc => !sc.IsDeleted);

            // AuthorRegistration
            builder.Entity<AuthorRegistration>()
               .HasIndex(er => new { er.AcademicYearId, er.AuthorId })
               .IsUnique();

            builder.Entity<AuthorRegistration>()
                .HasOne(er => er.Author)
                .WithOne(a => a.AuthorRegistration)
                .HasForeignKey<AuthorRegistration>(er => er.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Enum to string
            builder.Entity<Work>()
                .Property(w => w.Source)
                .HasConversion<string>();

            builder.Entity<User>()
               .Property(e => e.AcademicTitle)
               .HasConversion<string>();

            builder.Entity<User>()
               .Property(e => e.OfficerRank)
               .HasConversion<string>();

            // Seed Data
            builder.ApplyConfiguration(new FieldSeeding());
            builder.ApplyConfiguration(new DepartmentSeeding());
            builder.ApplyConfiguration(new WorkTypeSeeding());
            builder.ApplyConfiguration(new WorkLevelSeeding());
            builder.ApplyConfiguration(new AuthorRoleSeeding());
            builder.ApplyConfiguration(new PurposeSeeding());
            builder.ApplyConfiguration(new SCImagoFieldSeeding());
            builder.ApplyConfiguration(new FactorSeeding());
            builder.ApplyConfiguration(new AcademicYearSeeding());
        }

    }
}

