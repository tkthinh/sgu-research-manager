namespace Application.Factors
{
    public class FactorDto
    {
        public Guid Id { get; set; }
        public Guid WorkTypeId { get; set; }
        public Guid WorkLevelId { get; set; }
        public Guid PurposeId { get; set; }
        public required string Name { get; set; }
        public float Score { get; set; }
        public int Hours { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
