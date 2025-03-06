namespace Application.WorkTypes
{
   public class WorkTypeWithLevelCountDto
   {
      public Guid Id { get; set; }
      public required string Name { get; set; }
      public int? WorkLevelCount { get; set; }
      public DateTime CreatedDate { get; set; }
      public DateTime? ModifiedDate { get; set; }
   }
}
