using Domain.Entities;
using Domain.Interfaces;

namespace Application.Notifications
{
    public class NotificationMapper : IGenericMapper<NotificationDto, Notification>
    {
        public NotificationDto MapToDto(Notification entity)
        {
            return new NotificationDto
            {
                Id = entity.Id,
                Content = entity.Content,
                IsGlobal = entity.IsGlobal,
                UserId = entity.UserId,
                IsRead = entity.IsRead,
                CreatedDate = entity.CreatedDate,
            };
        }
        public Notification MapToEntity(NotificationDto dto)
        {
            return new Notification
            {
                Id = dto.Id,
                Content = dto.Content,
                IsGlobal = dto.IsGlobal,
                UserId = dto.UserId,
                IsRead = dto.IsRead,
            };
        }
        public IEnumerable<NotificationDto> MapToDtos(IEnumerable<Notification> entities)
        {
            return entities.Select(MapToDto);
        }
        public IEnumerable<Notification> MapToEntities(IEnumerable<NotificationDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}
