using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public interface IExcelService
    {
        Task ImportAsync(IFormFile file);
    }
}
