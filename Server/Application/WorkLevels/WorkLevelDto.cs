namespace Application.WorkLevels
{
    public class WorkLevelDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid WorkTypeId { get; set; }
        public string? WorkTypeName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
