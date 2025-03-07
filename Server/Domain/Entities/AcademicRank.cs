namespace Domain.Entities
{
   public class AcademicRank : BaseEntity
   {
      public required string Name { get; set; }

      public ICollection<Employee>? Employees { get; set; }
   }
}
