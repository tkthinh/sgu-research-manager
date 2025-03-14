namespace Domain.Entities
{
    public class WorkAuthor : BaseEntity
    {
        public Guid WorkId { get; set; }
        public Guid UserId { get; set; }

        public virtual Work? Work { get; set; }
        public virtual User? User { get; set; }
    }
}