namespace Domain.Entities
{
    public class WorkType : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<AuthorRole>? AuthorRoles { get; set; }
        public ICollection<WorkStatus>? WorkStatuses { get; set; }
    }
}
