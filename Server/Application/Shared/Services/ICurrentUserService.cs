namespace Application.Shared.Services
{
    public interface ICurrentUserService
    {
        (bool isSuccess, Guid userId, string userName) GetCurrentUser();
    }
} 