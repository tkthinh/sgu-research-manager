using Domain.Enums;

namespace Application.Factors
{
    public class FactorDto
    {
        public Guid Id { get; set; }
        public Guid WorkTypeId { get; set; }
        public Guid? WorkLevelId { get; set; }
        public Guid PurposeId { get; set; }
        public Guid? AuthorRoleId { get; set; }
        public required string Name { get; set; }
        public ScoreLevel? ScoreLevel { get; set; }
        public int ConvertHour { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
