namespace Domain.Entities
{
   public class Department : BaseEntity
   {
      public required string Name { get; set; }

      // Unidirectional binding
      public ICollection<User>? Users { get; set; }
      public ICollection<Assignment>? Assignments { get; set; }
    }
}
