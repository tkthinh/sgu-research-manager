using Domain.Entities;
using Domain.Interfaces;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories
{
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
             .Include(w => w.AcademicYear)
             .Include(w => w.Authors!)
                 .ThenInclude(a => a.AuthorRole)
             .Include(w => w.Authors!)
                 .ThenInclude(a => a.Purpose)
             .Include(w => w.Authors!)
                 .ThenInclude(a => a.SCImagoField)
             .Include(w => w.Authors!)
                 .ThenInclude(a => a.Field)
             .Include(w => w.Authors!)
                 .ThenInclude(a => a.AuthorRegistration)
                 .ThenInclude(ar => ar!.AcademicYear)
             .Include(w => w.WorkAuthors!);
                 
         // Chỉ dùng AsNoTracking khi được yêu cầu rõ ràng
         return asNoTracking ? query.AsNoTracking() : query;
      }

      // Triển khai phương thức mới GetWorksByFilterAsync
      public async Task<IEnumerable<Work>> GetWorksByFilterAsync(
         Expression<Func<Work, bool>> predicate,
         CancellationToken cancellationToken = default)
      {
         return await GetWorksWithAuthorsQuery(true)
            .Where(predicate)
            .ToListAsync(cancellationToken);
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

      public async Task<IEnumerable<Work>> GetWorksWithAuthorsByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
      {
         return await GetWorksWithAuthorsQuery(true)
             .Where(w => ids.Contains(w.Id))
             .ToListAsync(cancellationToken);
      }
   }
}
