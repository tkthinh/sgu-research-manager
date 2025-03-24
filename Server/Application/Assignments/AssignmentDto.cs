namespace Application.Assignments
{
   public class AssignmentDto
   {
      public Guid Id { get; set; }
      public Guid ManagerId { get; set; }
      public Guid DepartmentId { get; set; }

      public string? ManagerFullName { get; set; }
      public string? ManagerDepartmentName { get; set; }
      public string? AssignedDepartmentName { get; set; }
   }
}
