namespace Application.Assignments
{
   public class CreateAssignmentRequestDto
   {
      public Guid ManagerId { get; set; }
      public List<Guid> DepartmentIds { get; set; } = [];
   }
}
