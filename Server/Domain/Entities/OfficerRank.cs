namespace Domain.Entities
{
   public class OfficerRank : BaseEntity
   {
      public required string Name { get; set; }

      // Unidirectional binding
      public ICollection<Employee>? Employees { get; set; }
   }
}
