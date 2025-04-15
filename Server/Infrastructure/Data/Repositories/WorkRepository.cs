using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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
                 .ThenInclude(ar => ar!.AcademicYear);
                 
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
         // Trả về các công trình có tác giả thuộc phòng ban với departmentId
         return await GetWorksWithAuthorsQuery(true)
             .Where(w => w.Authors!.Any(a => a.User != null && a.User.DepartmentId == departmentId))
             .ToListAsync(cancellationToken);
      }

      public async Task<IEnumerable<Work>> GetWorksWithAuthorsByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
      {
         return await GetWorksWithAuthorsQuery(true)
             .Where(w => ids.Contains(w.Id))
             .ToListAsync(cancellationToken);
      }

      // Lọc công trình theo năm học
      public async Task<IEnumerable<Work>> GetWorksByAcademicYearIdAsync(Guid academicYearId, CancellationToken cancellationToken = default)
      {
         return await GetWorksWithAuthorsQuery(true)
             .Where(w => w.AcademicYearId == academicYearId)
             .ToListAsync(cancellationToken);
      }

      // Lọc công trình theo nhiều năm học
      public async Task<IEnumerable<Work>> GetWorksByAcademicYearIdsAsync(List<Guid> academicYearIds, CancellationToken cancellationToken = default)
      {
         return await GetWorksWithAuthorsQuery(true)
             .Where(w => academicYearIds.Contains(w.AcademicYearId))
             .ToListAsync(cancellationToken);
      }

      // Lấy công trình mà user đã kê khai ở một năm học cụ thể
      public async Task<IEnumerable<Work>> GetDeclaredWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default)
      {
         return await GetWorksWithAuthorsQuery(true)
             .Where(w => w.AcademicYearId == academicYearId &&
                         w.Source == Domain.Enums.WorkSource.NguoiDungKeKhai &&
                         w.Authors!.Any(a => a.UserId == userId))
             .ToListAsync(cancellationToken);
      }

      // Lấy công trình mà user là tác giả ở một năm học cụ thể (bao gồm cả import)
      public async Task<IEnumerable<Work>> GetAuthorWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default)
      {
         return await GetWorksWithAuthorsQuery(true)
             .Where(w => w.AcademicYearId == academicYearId &&
                         w.Authors!.Any(a => a.UserId == userId))
             .ToListAsync(cancellationToken);
      }

      // Lấy công trình mà user là tác giả ở nhiều năm học
      public async Task<IEnumerable<Work>> GetAuthorWorksByAcademicYearIdsAsync(Guid userId, List<Guid> academicYearIds, CancellationToken cancellationToken = default)
      {
         return await GetWorksWithAuthorsQuery(true)
             .Where(w => academicYearIds.Contains(w.AcademicYearId) &&
                         w.Authors!.Any(a => a.UserId == userId))
             .ToListAsync(cancellationToken);
      }

      // Lấy công trình mà user là đồng tác giả ở một năm học cụ thể
      public async Task<IEnumerable<Work>> GetCoAuthorWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default)
      {
         return await GetWorksWithAuthorsQuery(true)
             .Where(w => w.AcademicYearId == academicYearId &&
                         w.WorkAuthors!.Any(wa => wa.UserId == userId))
             .ToListAsync(cancellationToken);
      }

      // Lấy công trình mà user là đồng tác giả ở nhiều năm học
      public async Task<IEnumerable<Work>> GetCoAuthorWorksByAcademicYearIdsAsync(Guid userId, List<Guid> academicYearIds, CancellationToken cancellationToken = default)
      {
         return await GetWorksWithAuthorsQuery(true)
             .Where(w => academicYearIds.Contains(w.AcademicYearId) &&
                         w.WorkAuthors!.Any(wa => wa.UserId == userId))
             .ToListAsync(cancellationToken);
      }

      // Lấy các công trình chưa đánh dấu quy đổi từ các đợt trước
      public async Task<IEnumerable<Work>> GetUnmarkedPreviousWorksAsync(Guid userId, Guid currentAcademicYearId, CancellationToken cancellationToken = default)
      {
         // Lấy năm học hiện tại
         var currentAcademicYear = await context.AcademicYears
             .FirstOrDefaultAsync(ay => ay.Id == currentAcademicYearId, 
                                 cancellationToken);

         if (currentAcademicYear == null)
         {
             return Enumerable.Empty<Work>();
         }
         
         var query = GetWorksWithAuthorsQuery(true)
             .Where(w => w.AcademicYearId != currentAcademicYearId &&
                         w.Authors!.Any(a => a.UserId == userId && 
                                          (a.AuthorRegistration == null || 
                                           a.AuthorRegistration.AcademicYearId != currentAcademicYearId)));
                                         
         // Ghi log để debug số lượng công trình tìm thấy
         var result = await query.ToListAsync(cancellationToken);
         Console.WriteLine($"GetUnmarkedPreviousWorksAsync: Found {result.Count} works");
         
         return result;
      }
   }
}
