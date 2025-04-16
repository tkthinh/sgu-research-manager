
namespace Application.Works
{
    public interface IWorkExportService
    {
        Task<List<ExportExcelDto>> GetExportExcelDataAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<byte[]> ExportWorksByUserAsync(List<ExportExcelDto> exportData, CancellationToken cancellationToken = default);
    }
}
