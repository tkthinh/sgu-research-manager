namespace Domain.Entities
{
   public class Field : BaseEntity
   {
      public required string Name { get; set; }

      // Unidirectional binding
      public ICollection<User>? Users { get; set; }
   }
}
