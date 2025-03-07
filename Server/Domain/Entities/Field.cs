namespace Domain.Entities
{
   public class Field : BaseEntity
   {
      public required string Name { get; set; }

      public ICollection<Employee>? Employees { get; set; }
   }
}
