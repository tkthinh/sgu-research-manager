namespace Application.Users
{
    public interface IUserImportService
    {
        Task<UserImportResult> ImportUsersAsync(Stream excelStream);
    }
}
