using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWorkRepository : IGenericRepository<Work>
    {
        Task<IEnumerable<Work>> GetWorksWithAuthorsAsync(CancellationToken cancellationToken = default);
        Task<Work?> GetWorkWithAuthorsByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Work>> FindWorksWithAuthorsAsync(string title, CancellationToken cancellationToken = default);
        Task<IEnumerable<Work>> GetWorksByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Work>> GetWorksWithAuthorsByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
        
        // Phương thức lọc công trình theo đợt kê khai
        Task<IEnumerable<Work>> GetWorksBySystemConfigIdAsync(Guid systemConfigId, CancellationToken cancellationToken = default);
        
        // Phương thức lọc công trình theo nhiều đợt kê khai (dùng cho năm học)
        Task<IEnumerable<Work>> GetWorksBySystemConfigIdsAsync(List<Guid> systemConfigIds, CancellationToken cancellationToken = default);
        
        // Phương thức lấy công trình mà user đã kê khai ở một đợt cụ thể
        Task<IEnumerable<Work>> GetDeclaredWorksBySystemConfigIdAsync(Guid userId, Guid systemConfigId, CancellationToken cancellationToken = default);
        
        // Phương thức lấy công trình mà user là tác giả ở một đợt cụ thể
        Task<IEnumerable<Work>> GetAuthorWorksBySystemConfigIdAsync(Guid userId, Guid systemConfigId, CancellationToken cancellationToken = default);
        
        // Phương thức lấy công trình mà user là tác giả ở nhiều đợt (dùng cho năm học)
        Task<IEnumerable<Work>> GetAuthorWorksBySystemConfigIdsAsync(Guid userId, List<Guid> systemConfigIds, CancellationToken cancellationToken = default);
        
        // Phương thức lấy công trình mà user là đồng tác giả ở một đợt cụ thể
        Task<IEnumerable<Work>> GetCoAuthorWorksBySystemConfigIdAsync(Guid userId, Guid systemConfigId, CancellationToken cancellationToken = default);
        
        // Phương thức lấy công trình mà user là đồng tác giả ở nhiều đợt (dùng cho năm học)
        Task<IEnumerable<Work>> GetCoAuthorWorksBySystemConfigIdsAsync(Guid userId, List<Guid> systemConfigIds, CancellationToken cancellationToken = default);
        
        // Phương thức lấy các công trình chưa đánh dấu quy đổi từ các đợt trước
        Task<IEnumerable<Work>> GetUnmarkedPreviousWorksAsync(Guid userId, Guid currentSystemConfigId, CancellationToken cancellationToken = default);
    }
}