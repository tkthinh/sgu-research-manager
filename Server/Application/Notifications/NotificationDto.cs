namespace Application.Notifications
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public required string Content { get; set; }
        public bool IsGlobal { get; set; }
        public Guid? UserId { get; set; }
        public bool? IsRead { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
