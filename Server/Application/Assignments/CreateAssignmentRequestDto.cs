namespace Application.Assignments
{
    public class CreateAssignmentRequestDto
    {
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
