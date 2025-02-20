namespace Application.Purposes
{
   public class PurposeDto
   {
      public Guid Id { get; set; }
      public required string Name { get; set; }
      public DateTime CreatedDate { get; set; }
      public DateTime? ModifiedDate { get; set; }
   }
}
