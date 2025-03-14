using Domain.Enums;

namespace Application.Authors
{
    public class UpdateAuthorByAdminRequestDto
    {
        public Guid? AuthorRoleId { get; set; }
        public Guid? PurposeId { get; set; }
        public Guid? SCImagoFieldId { get; set; }
        public Guid? ScoringFieldId { get; set; }
        public int? Position { get; set; }
        public ScoreLevel? ScoreLevel { get; set; }
        public ProofStatus? ProofStatus { get; set; }
        public string? Note { get; set; }
    }
}