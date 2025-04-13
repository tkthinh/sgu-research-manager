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

   // Lọc công trình theo đợt kê khai
   public async Task<IEnumerable<Work>> GetWorksBySystemConfigIdAsync(Guid systemConfigId, CancellationToken cancellationToken = default)
   {
      return await GetWorksWithAuthorsQuery(true)
          .Where(w => w.SystemConfigId == systemConfigId)
          .ToListAsync(cancellationToken);
   }

   // Lọc công trình theo nhiều đợt kê khai (dùng cho năm học)
   public async Task<IEnumerable<Work>> GetWorksBySystemConfigIdsAsync(List<Guid> systemConfigIds, CancellationToken cancellationToken = default)
   {
      return await GetWorksWithAuthorsQuery(true)
          .Where(w => systemConfigIds.Contains(w.SystemConfigId))
          .ToListAsync(cancellationToken);
   }

   // Lấy công trình mà user đã kê khai ở một đợt cụ thể
   public async Task<IEnumerable<Work>> GetDeclaredWorksBySystemConfigIdAsync(Guid userId, Guid systemConfigId, CancellationToken cancellationToken = default)
   {
      return await GetWorksWithAuthorsQuery(true)
          .Where(w => w.SystemConfigId == systemConfigId &&
                      w.Source == Domain.Enums.WorkSource.NguoiDungKeKhai &&
                      w.Authors!.Any(a => a.UserId == userId))
          .ToListAsync(cancellationToken);
   }

   // Lấy công trình mà user là tác giả ở một đợt cụ thể (bao gồm cả import)
   public async Task<IEnumerable<Work>> GetAuthorWorksBySystemConfigIdAsync(Guid userId, Guid systemConfigId, CancellationToken cancellationToken = default)
   {
      return await GetWorksWithAuthorsQuery(true)
          .Where(w => w.SystemConfigId == systemConfigId &&
                      w.Authors!.Any(a => a.UserId == userId))
          .ToListAsync(cancellationToken);
   }

   // Lấy công trình mà user là tác giả ở nhiều đợt (dùng cho năm học)
   public async Task<IEnumerable<Work>> GetAuthorWorksBySystemConfigIdsAsync(Guid userId, List<Guid> systemConfigIds, CancellationToken cancellationToken = default)
   {
      return await GetWorksWithAuthorsQuery(true)
          .Where(w => systemConfigIds.Contains(w.SystemConfigId) &&
                      w.Authors!.Any(a => a.UserId == userId))
          .ToListAsync(cancellationToken);
   }

   // Lấy công trình mà user là đồng tác giả ở một đợt cụ thể
   public async Task<IEnumerable<Work>> GetCoAuthorWorksBySystemConfigIdAsync(Guid userId, Guid systemConfigId, CancellationToken cancellationToken = default)
   {
      return await GetWorksWithAuthorsQuery(true)
          .Where(w => w.SystemConfigId == systemConfigId &&
                      w.WorkAuthors!.Any(wa => wa.UserId == userId))
          .ToListAsync(cancellationToken);
   }

   // Lấy công trình mà user là đồng tác giả ở nhiều đợt (dùng cho năm học)
   public async Task<IEnumerable<Work>> GetCoAuthorWorksBySystemConfigIdsAsync(Guid userId, List<Guid> systemConfigIds, CancellationToken cancellationToken = default)
   {
      return await GetWorksWithAuthorsQuery(true)
          .Where(w => systemConfigIds.Contains(w.SystemConfigId) &&
                      w.WorkAuthors!.Any(wa => wa.UserId == userId))
          .ToListAsync(cancellationToken);
   }

   // Lấy các công trình chưa đánh dấu quy đổi từ các đợt trước
   public async Task<IEnumerable<Work>> GetUnmarkedPreviousWorksAsync(Guid userId, Guid currentSystemConfigId, CancellationToken cancellationToken = default)
   {
      // Ghi log để debug
      Console.WriteLine($"GetUnmarkedPreviousWorksAsync: userId={userId}, currentSystemConfigId={currentSystemConfigId}");
      
      var query = GetWorksWithAuthorsQuery(true)
          .Where(w => w.SystemConfigId != currentSystemConfigId &&
                      w.Authors!.Any(a => a.UserId == userId && 
                                       !a.MarkedForScoring));
                                      
      // Ghi log để debug số lượng công trình tìm thấy
      var result = await query.ToListAsync(cancellationToken);
      Console.WriteLine($"GetUnmarkedPreviousWorksAsync: Found {result.Count} works");
      
      return result;
   }
}
