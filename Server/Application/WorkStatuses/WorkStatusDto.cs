namespace Application.WorkStatuses
{
    public class WorkStatusDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid WorkTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
