namespace Application.Assignments
{
   public class UpdateAssignmentRequestDto
   {
      public Guid ManagerId { get; set; }
      public List<Guid> DepartmentIds { get; set; } = [];
   }
}
