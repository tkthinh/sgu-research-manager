namespace Domain.Entities
{
    public class Notification : BaseEntity
    {
        public required string Content { get; set; }
        public bool IsGlobal { get; set; }

        public Guid? UserId { get; set; }
        public bool? IsRead { get; set; }
    }
}
