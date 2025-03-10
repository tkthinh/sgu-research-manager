using Domain.Enums;

namespace Application.Authors
{
    public class AuthorDto
    {
        public Guid Id { get; set; }
        public Guid WorkId { get; set; }
        public Guid UserId { get; set; }
        public Guid AuthorRoleId { get; set; }
        public Guid PurposeId { get; set; }
        public int? Position { get; set; }
        public ScoreLevel? ScoreLevel { get; set; }
        public int FinalAuthorHour { get; set; }
        public int TempAuthorHour { get; set; }
        public int TempWorkHour { get; set; }
        public bool IsNotMatch { get; set; }
        public bool MarkedForScoring { get; set; }
        public string? CoAuthors { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}