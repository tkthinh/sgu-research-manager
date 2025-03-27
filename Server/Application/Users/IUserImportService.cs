using Application.Shared.Response;

namespace Application.Users
{
    public interface IUserImportService
    {
        Task<ApiResponse<object>> ImportUsersAsync(Stream excelStream);
    }
}
