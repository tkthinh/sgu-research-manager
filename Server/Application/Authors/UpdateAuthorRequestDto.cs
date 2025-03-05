namespace Application.Authors
{
    public class UpdateAuthorRequestDto
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
    }
}
