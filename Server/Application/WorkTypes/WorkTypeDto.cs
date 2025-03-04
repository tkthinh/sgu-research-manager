namespace Application.WorkTypes
{
    public class WorkTypeDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public bool HasExtraOption { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
