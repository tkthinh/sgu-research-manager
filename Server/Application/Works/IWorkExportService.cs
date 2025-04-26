using Domain.Interfaces;

namespace Application.Works
{
    public interface IWorkExportService
    {
        Task<List<ExportExcelDto>> GetExportExcelDataAsync(WorkFilter filter, CancellationToken cancellationToken = default);
        Task<byte[]> ExportWorksByUserAsync(List<ExportExcelDto> exportData, CancellationToken cancellationToken = default);
        Task<byte[]> ExportWorksByAdminAsync(List<ExportExcelDto> exportData, Guid userId, CancellationToken cancellationToken = default);
    }
}
