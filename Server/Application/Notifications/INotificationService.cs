using Domain.Entities;
using Domain.Interfaces;

namespace Application.Notifications
{
    public interface INotificationService : IGenericService<NotificationDto>
    {
        Task CreateGlobalNotificationAsync(string content);
        Task<NotificationDto> CreateNotificationForUserAsync(Guid userId, string content);

        Task<IEnumerable<NotificationDto>> GetGlobalNotificationsAsync();
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(Guid userId);

        Task<NotificationDto> MarkNotificationAsReadAsync(Guid id);
    }
}
