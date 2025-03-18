using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class WorkRepository : GenericRepository<Work>, IWorkRepository
{
   public WorkRepository(ApplicationDbContext context) : base(context)
   {
   }

   public async Task<IEnumerable<Work>> GetWorksWithAuthorsAsync(CancellationToken cancellationToken = default)
   {
      return await context.Works
          .Include(w => w.WorkType)
          .Include(w => w.WorkLevel)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.AuthorRole)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.Purpose)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.SCImagoField)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.Field)
          .ToListAsync(cancellationToken);
   }

   public async Task<Work?> GetWorkWithAuthorsByIdAsync(Guid id, CancellationToken cancellationToken = default)
   {
      return await context.Works
          .Include(w => w.WorkType)
          .Include(w => w.WorkLevel)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.AuthorRole)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.Purpose)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.SCImagoField)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.Field)
          .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
   }

   public async Task<IEnumerable<Work>> FindWorksWithAuthorsAsync(string title, CancellationToken cancellationToken = default)
   {
      return await context.Works
          .Where(w => w.Title.Contains(title))
          .Include(w => w.WorkType)
          .Include(w => w.WorkLevel)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.AuthorRole)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.Purpose)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.SCImagoField)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.Field)
          .ToListAsync(cancellationToken);
   }

   public async Task<IEnumerable<Work>> GetWorksByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
   {
      return await context.Works
          .Include(w => w.WorkType)
          .Include(w => w.WorkLevel)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.AuthorRole)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.Purpose)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.SCImagoField)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.Field)
          .Where(w => w.Authors != null && w.Authors.Any(a => a.User!.DepartmentId == departmentId))
          .ToListAsync(cancellationToken);
   }

   public async Task<IEnumerable<Work>> GetWorksWithAuthorsByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
   {
      return await context.Works
          .Where(w => ids.Contains(w.Id))
          .Include(w => w.WorkType)
          .Include(w => w.WorkLevel)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.AuthorRole)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.Purpose)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.SCImagoField)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.Field)
          .ToListAsync(cancellationToken);
   }
}
