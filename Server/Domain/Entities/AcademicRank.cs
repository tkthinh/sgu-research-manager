namespace Domain.Entities
{
   public class AcademicRank : BaseEntity
   {
      public required string Name { get; set; }

      // Unidirectional binding
      public ICollection<Employee>? Employees { get; set; }
   }
}
