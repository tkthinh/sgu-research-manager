using Microsoft.AspNetCore.Http;

namespace Application.Works
{
    public interface IWorkImportService
    {
        Task ImportAsync(IFormFile file);
    }
}
