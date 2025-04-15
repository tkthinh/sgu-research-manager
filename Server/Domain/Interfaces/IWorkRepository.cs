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
        
        // Phương thức lọc công trình theo năm học
        Task<IEnumerable<Work>> GetWorksByAcademicYearIdAsync(Guid academicYearId, CancellationToken cancellationToken = default);
        
        // Phương thức lọc công trình theo nhiều năm học
        Task<IEnumerable<Work>> GetWorksByAcademicYearIdsAsync(List<Guid> academicYearIds, CancellationToken cancellationToken = default);
        
        // Phương thức lấy công trình mà user đã kê khai ở một năm học cụ thể
        Task<IEnumerable<Work>> GetDeclaredWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default);
        
        // Phương thức lấy công trình mà user là tác giả ở một năm học cụ thể
        Task<IEnumerable<Work>> GetAuthorWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default);
        
        // Phương thức lấy công trình mà user là tác giả ở nhiều năm học
        Task<IEnumerable<Work>> GetAuthorWorksByAcademicYearIdsAsync(Guid userId, List<Guid> academicYearIds, CancellationToken cancellationToken = default);
        
        // Phương thức lấy công trình mà user là đồng tác giả ở một năm học cụ thể
        Task<IEnumerable<Work>> GetCoAuthorWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default);
        
        // Phương thức lấy công trình mà user là đồng tác giả ở nhiều năm học
        Task<IEnumerable<Work>> GetCoAuthorWorksByAcademicYearIdsAsync(Guid userId, List<Guid> academicYearIds, CancellationToken cancellationToken = default);
        
        // Phương thức lấy các công trình chưa đánh dấu quy đổi từ các đợt trước
        Task<IEnumerable<Work>> GetUnmarkedPreviousWorksAsync(Guid userId, Guid currentAcademicYearId, CancellationToken cancellationToken = default);
    }
}