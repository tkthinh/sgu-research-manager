using Domain.Enums;

namespace Application.Authors
{
    public class CreateAuthorRequestDto
    {
        public Guid UserId { get; set; }
        public Guid AuthorRoleId { get; set; }
        public Guid PurposeId { get; set; }
        public int? Position { get; set; }
        public ScoreLevel? ScoreLevel { get; set; }
        public string? CoAuthors { get; set; }
    }
}