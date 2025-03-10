namespace Domain.Entities
{
    public class AuthorRole : BaseEntity
    {
        public required string Name { get; set; }
        public bool IsMainAuthor { get; set; }
        public Guid WorkTypeId { get; set; }

        public virtual WorkType? WorkType { get; set; }

        public ICollection<Author>? Authors { get; set; }
        public ICollection<Factor>? Factors { get; set; }
    }
}
