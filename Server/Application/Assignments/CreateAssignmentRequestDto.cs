namespace Application.Assignments
{
    public class CreateAssignmentRequestDto
    {
        public Guid ManagerId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
