namespace Application.Departments
{
   public class DepartmentDto
   {
      public Guid Id { get; set; }
      public required string Name { get; set; }
      public DateTime CreatedDate { get; set; }
      public DateTime? ModifiedDate { get; set; }
   }
}
