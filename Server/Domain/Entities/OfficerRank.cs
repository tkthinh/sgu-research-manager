namespace Domain.Entities
{
   public class OfficerRank : BaseEntity
   {
      public required string Name { get; set; }

      public ICollection<Employee>? Employees { get; set; }
   }
}
