namespace Domain.Entities
{
    public class WorkType : BaseEntity
    {
        public required string Name { get; set; }

        // Unidirectional binding
        public ICollection<AuthorRole>? AuthorRoles { get; set; }
    }
}
