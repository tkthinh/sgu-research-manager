namespace Domain.Entities
{
    public class Assignment : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }

        public virtual User? User { get; set; }
        public virtual Department? Department { get; set; }
    }
}
