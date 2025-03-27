using Domain.Enums;

namespace Application.Authors
{
    public class UpdateAuthorRequestDto
    {
        public Guid? AuthorRoleId { get; set; }
        public Guid? PurposeId { get; set; }
        public Guid? SCImagoFieldId { get; set; }
        public Guid? FieldId { get; set; }
        public int? Position { get; set; }
        public ScoreLevel? ScoreLevel { get; set; }
        public ProofStatus? ProofStatus { get; set; }
        public string? Note { get; set; }
    }
}