namespace Application.WorkLevels
{
    public class CreateWorkLevelRequestDto
    {
        public required string Name { get; set; }
        public Guid WorkTypeId { get; set; }
    }
}
