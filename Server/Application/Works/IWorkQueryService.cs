using Domain.Interfaces;
using Domain.Enums;

namespace Application.Works
{
    public interface IWorkQueryService
    {
        // Phương thức lấy một công trình theo ID
        Task<WorkDto?> GetWorkByIdWithAuthorsAsync(Guid id, CancellationToken cancellationToken = default);
        
        // Phương thức tổng quát để truy vấn công trình với bộ lọc
        Task<IEnumerable<WorkDto>> GetWorksAsync(
            WorkFilter filter, 
            CancellationToken cancellationToken = default);
    }
} 