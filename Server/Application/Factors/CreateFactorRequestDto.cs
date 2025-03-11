using Domain.Enums;

namespace Application.Factors
{
    public class CreateFactorRequestDto
    {
        public Guid WorkTypeId { get; set; }
        public Guid? WorkLevelId { get; set; }
        public Guid PurposeId { get; set; }
        public Guid? AuthorRoleId { get; set; }
        public required string Name { get; set; }
        public ScoreLevel? ScoreLevel { get; set; }
        public int ConvertHour { get; set; }
        public int? MaxAllowed { get; set; }
    }
}
