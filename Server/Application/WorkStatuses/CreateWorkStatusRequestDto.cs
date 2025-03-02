namespace Application.WorkStatuses
{
    public class CreateWorkStatusRequestDto
    {
        public required string Name { get; set; }
        public Guid WorkTypeId { get; set; }
    }
}
