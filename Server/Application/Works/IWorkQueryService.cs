using Domain.Interfaces;
using Domain.Enums;

namespace Application.Works
{
    public interface IWorkQueryService
    {
        Task<IEnumerable<WorkDto>> GetWorksByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkDto>> GetWorksByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkDto>> GetAllWorksWithAuthorsAsync(CancellationToken cancellationToken = default);
        Task<WorkDto?> GetWorkByIdWithAuthorsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkDto>> GetWorksByCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkDto>> GetAllWorksByCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default);

        // Phương thức lọc công trình theo năm học
        Task<IEnumerable<WorkDto>> GetWorksByAcademicYearIdAsync(Guid academicYearId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkDto>> GetMyWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkDto>> GetAllMyWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default);
        
        // Phương thức lấy công trình có thể đăng ký quy đổi
        //Task<IEnumerable<WorkDto>> GetMyRegisterableWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default);
        
        // Phương thức lấy công trình đã đăng ký quy đổi
        Task<IEnumerable<WorkDto>> GetMyRegisteredWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default);
        
        // Phương thức lọc công trình theo năm học và trạng thái
        Task<IEnumerable<WorkDto>> GetMyWorksByAcademicYearAndProofStatusAsync(Guid userId, Guid academicYearId, ProofStatus proofStatus, CancellationToken cancellationToken = default);
        
        // Phương thức lọc công trình theo năm học và nguồn
        Task<IEnumerable<WorkDto>> GetMyWorksByAcademicYearAndSourceAsync(Guid userId, Guid academicYearId, WorkSource source, CancellationToken cancellationToken = default);
        
        // Phương thức lấy công trình mà user là đồng tác giả
        Task<IEnumerable<WorkDto>> GetMyCoAuthorWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default);
        
        // Phương thức cho admin
        Task<IEnumerable<WorkDto>> GetUserWorksByAcademicYearIdAsync(Guid userId, Guid academicYearId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkDto>> GetWorksByDepartmentAndAcademicYearIdAsync(Guid departmentId, Guid academicYearId, CancellationToken cancellationToken = default);
    }
} 