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

   // Phương thức private để tạo ra query cơ bản với tất cả các Include cần thiết
   private IQueryable<Work> GetWorksWithAuthorsQuery(bool asNoTracking = false)
   {
      var query = context.Works
          .Include(w => w.WorkType)
          .Include(w => w.WorkLevel)
          .Include(w => w.SystemConfig)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.AuthorRole)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.Purpose)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.SCImagoField)
          .Include(w => w.Authors!)
              .ThenInclude(a => a.Field);
              
      // Chỉ dùng AsNoTracking khi được yêu cầu rõ ràng
      return asNoTracking ? query.AsNoTracking() : query;
   }

   public async Task<IEnumerable<Work>> GetWorksWithAuthorsAsync(CancellationToken cancellationToken = default)
   {
      // Không dùng AsNoTracking mặc định
      return await GetWorksWithAuthorsQuery()
          .ToListAsync(cancellationToken);
   }

   public async Task<Work?> GetWorkWithAuthorsByIdAsync(Guid id, CancellationToken cancellationToken = default)
   {
      // Không dùng AsNoTracking mặc định
      return await GetWorksWithAuthorsQuery()
          .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
   }

   public async Task<IEnumerable<Work>> FindWorksWithAuthorsAsync(string title, CancellationToken cancellationToken = default)
   {
      // Không dùng AsNoTracking mặc định
      return await GetWorksWithAuthorsQuery()
          .Where(w => w.Title.Contains(title))
          .ToListAsync(cancellationToken);
   }

   public async Task<IEnumerable<Work>> GetWorksByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
   {
      // Không dùng AsNoTracking mặc định
      return await GetWorksWithAuthorsQuery()
          .Where(w => w.Authors != null && w.Authors.Any(a => a.User!.DepartmentId == departmentId))
          .ToListAsync(cancellationToken);
   }

   public async Task<IEnumerable<Work>> GetWorksWithAuthorsByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
   {
      // Không dùng AsNoTracking mặc định
      return await GetWorksWithAuthorsQuery()
          .Where(w => ids.Contains(w.Id))
          .ToListAsync(cancellationToken);
   }
}
