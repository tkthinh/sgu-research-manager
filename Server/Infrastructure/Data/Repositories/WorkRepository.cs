using Domain.Entities;
using Domain.Interfaces;
using Domain.Enums;
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

      // Phương thức lấy công trình có thể đăng ký quy đổi của người dùng trong năm học
      //public async Task<IEnumerable<Work>> GetRegisterableWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, DateTime currentDate, CancellationToken cancellationToken = default)
      //{
      //   // Lấy thông tin năm học để kiểm tra deadline
      //   var academicYear = await context.AcademicYears
      //      .FirstOrDefaultAsync(ay => ay.Id == academicYearId, cancellationToken);

      //   if (academicYear == null || academicYear.ExchangeDeadline < currentDate)
      //   {
      //      return Enumerable.Empty<Work>();
      //   }

      //   // Lấy tất cả các công trình của người dùng (là tác giả) trong năm học hiện tại
      //   var userWorks = await GetAuthorWorksByAcademicYearIdAsync(userId, academicYearId, cancellationToken);
         
      //   // Lấy tất cả công trình của người dùng được add vào làm đồng tác giả
      //   var coAuthorWorks = await GetCoAuthorWorksByAcademicYearIdAsync(userId, academicYearId, cancellationToken);
         
      //   // Lấy tất cả công trình của người dùng từ các năm học trước mà chưa đăng ký quy đổi
      //   var previousWorks = await GetUnmarkedPreviousWorksAsync(userId, academicYearId, cancellationToken);
         
      //   // Kết hợp tất cả công trình
      //   var allWorks = userWorks.Union(coAuthorWorks).Union(previousWorks).DistinctBy(w => w.Id);
         
      //   // Lấy danh sách ID tác giả đã đăng ký quy đổi trong năm học này
      //   var registeredAuthorIds = await context.AuthorRegistrations
      //      .Where(ar => ar.AcademicYearId == academicYearId)
      //      .Select(ar => ar.AuthorId)
      //      .ToListAsync(cancellationToken);
         
      //   // Lọc những công trình chưa đăng ký quy đổi (tức là không có author nào của user trong bảng AuthorRegistration)
      //   var registerableWorks = allWorks.Where(w => 
      //      !w.Authors!.Any(a => a.UserId == userId && registeredAuthorIds.Contains(a.Id))
      //   ).ToList();
         
      //   return registerableWorks;
      //}

      // Phương thức lấy công trình đã đăng ký quy đổi của người dùng trong năm học
      public async Task<IEnumerable<Work>> GetRegisteredWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default)
      {
         // Lấy tất cả AuthorRegistration của user trong năm học
         var authorRegistrationIds = await context.AuthorRegistrations
            .Where(ar => ar.AcademicYearId == academicYearId && ar.Author.UserId == userId)
            .Select(ar => ar.AuthorId)
            .ToListAsync(cancellationToken);
         
         if (!authorRegistrationIds.Any())
         {
            return Enumerable.Empty<Work>();
         }
         
         // Lấy tất cả công trình có author trong danh sách đã đăng ký
         var works = await GetWorksWithAuthorsQuery(true)
            .Where(w => w.Authors!.Any(a => authorRegistrationIds.Contains(a.Id)))
            .ToListAsync(cancellationToken);
         
         return works;
      }

      // Phương thức lấy công trình của người dùng theo năm học và trạng thái
      public async Task<IEnumerable<Work>> GetWorksByAcademicYearAndProofStatusAsync(Guid userId, Guid academicYearId, ProofStatus proofStatus, CancellationToken cancellationToken = default)
      {
         // Lấy tất cả công trình của người dùng trong năm học
         var userWorks = await GetAuthorWorksByAcademicYearIdAsync(userId, academicYearId, cancellationToken);
         var coAuthorWorks = await GetCoAuthorWorksByAcademicYearIdAsync(userId, academicYearId, cancellationToken);
         
         // Kết hợp tất cả công trình
         var allWorks = userWorks.Union(coAuthorWorks).DistinctBy(w => w.Id).ToList();
         
         // Lọc công trình theo ProofStatus của người dùng
         var filteredWorks = allWorks.Where(w => 
            w.Authors!.Any(a => a.UserId == userId && a.ProofStatus == proofStatus)
         ).ToList();
         
         return filteredWorks;
      }

      // Phương thức lấy công trình của người dùng theo năm học và nguồn
      public async Task<IEnumerable<Work>> GetWorksByAcademicYearAndSourceAsync(Guid userId, Guid academicYearId, WorkSource source, CancellationToken cancellationToken = default)
      {
         // Lấy tất cả công trình của người dùng trong năm học có nguồn phù hợp
         return await GetWorksWithAuthorsQuery(true)
            .Where(w => w.AcademicYearId == academicYearId && 
                      w.Source == source &&
                      (w.Authors!.Any(a => a.UserId == userId) || 
                       w.WorkAuthors!.Any(wa => wa.UserId == userId)))
            .ToListAsync(cancellationToken);
      }

      // Phương thức lấy công trình của người dùng cụ thể theo năm học (dành cho admin)
      public async Task<IEnumerable<Work>> GetUserWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default)
      {
         // Cho admin: lấy tất cả công trình của một user cụ thể theo năm học
         var userWorks = await GetAuthorWorksByAcademicYearIdAsync(userId, academicYearId, cancellationToken);
         var coAuthorWorks = await GetCoAuthorWorksByAcademicYearIdAsync(userId, academicYearId, cancellationToken);
         
         return userWorks.Union(coAuthorWorks).DistinctBy(w => w.Id).ToList();
      }

      // Phương thức lấy công trình theo khoa/phòng ban và năm học
      public async Task<IEnumerable<Work>> GetWorksByDepartmentAndAcademicYearIdAsync(Guid departmentId, Guid academicYearId, CancellationToken cancellationToken = default)
      {
         return await GetWorksWithAuthorsQuery(true)
            .Where(w => w.AcademicYearId == academicYearId && 
                      w.Authors!.Any(a => a.User != null && a.User.DepartmentId == departmentId))
            .ToListAsync(cancellationToken);
      }
   }
}
