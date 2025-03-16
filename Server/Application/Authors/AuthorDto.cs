using Domain.Enums;

namespace Application.Authors
{
    public class AuthorDto
    {
        public Guid Id { get; set; }
        public Guid WorkId { get; set; }
        public Guid UserId { get; set; }
        public Guid AuthorRoleId { get; set; }
        public string? AuthorRoleName { get; set; }
        public Guid PurposeId { get; set; }
        public string? PurposeName { get; set; }
        public Guid? SCImagoFieldId { get; set; }
        public string? SCImagoFieldName { get; set; }
        public Guid? FieldId { get; set; }
        public string? FieldName { get; set; }

        public int? Position { get; set; }
        public ScoreLevel? ScoreLevel { get; set; }
        public decimal AuthorHour { get; set; }
        public int WorkHour { get; set; }
        public bool MarkedForScoring { get; set; }
        public ProofStatus ProofStatus { get; set; }
        public string? Note { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}