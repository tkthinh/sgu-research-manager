namespace Application.Factors
{
    public class CreateFactorRequestDto
    {
        public Guid WorkTypeId { get; set; }
        public Guid WorkLevelId { get; set; }
        public Guid PurposeId { get; set; }
        public required string Name { get; set; }
        public float Score { get; set; }
        public int Hours { get; set; }
    }
}
