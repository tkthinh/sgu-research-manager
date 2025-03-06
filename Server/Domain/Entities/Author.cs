namespace Domain.Entities
{
    public class Author : BaseEntity
    {
        public Guid WorkId { get; set; }
        public Guid UserId { get; set; }
        public Guid AuthorRoleId { get; set; }
        public Guid PurposeId { get; set; }
        public int? Position { get; set; }
        public float DeclaredScore { get; set; }
        public float FinalScore { get; set; }
        public int DeclaredHours { get; set; }
        public int FinalHours { get; set; }
        public bool IsNotMatch { get; set; }
        public bool MarkedForScoring { get; set; }
        public string? CoAuthors { get; set; }

        public virtual Employee? User { get; set; }
        public virtual AuthorRole? AuthorRole { get; set; }
        public virtual Work? Work { get; set; }
        public virtual Purpose? Purpose { get; set; }
    }
}
