namespace Application.Purposes
{
   public class CreatePurposeRequestDto
   {
      public required string Name { get; set; }
      public Guid WorkTypeId { get; set; }
   }
}