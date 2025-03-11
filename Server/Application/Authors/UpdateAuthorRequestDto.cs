using Domain.Enums;

namespace Application.Authors
{
    public class UpdateAuthorRequestDto
    {
        public Guid UserId { get; set; }
        public Guid AuthorRoleId { get; set; }
        public Guid PurposeId { get; set; }
        public int? Position { get; set; }
        public ScoreLevel? ScoreLevel { get; set; }
        public string? CoAuthors { get; set; }
        public bool MarkedForScoring { get; set; } // Cho phép admin hoặc user cập nhật
    }
}