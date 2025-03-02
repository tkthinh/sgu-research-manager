using System.Globalization;

namespace Domain.Entities
{
   public enum WorkSource
   {
      Imported = 1,
      Submitted = 2,
   }

   public class Work : BaseEntity
   {
      public required string Title { get; set; }
      public DateOnly? TimePublished { get; set; }
      public WorkSource Source { get; set; }
      public string? Note { get; set; }
      public Dictionary<string, string>? Details { get; set; }

      public Guid TypeId { get; set; }
      public Guid LevelId { get; set; }
      public Guid StatusId { get; set; }
      public Guid ScoringFieldId { get; set; }
      public Guid ProofId { get; set; }

      public float ManagerWorkScore { get; set; }
      public int TotalAuthors { get; set; }
      public int TotalHours { get; set; }
      public int MainAuthorCount { get; set; }


      // (Tuỳ chọn) Navigation properties đến các entity khác
      public virtual WorkType? WorkType { get; set; }
      public virtual WorkLevel? WorkLevel { get; set; }
      public virtual WorkStatus? WorkStatus { get; set; }
      public virtual Field? FieldForScoring { get; set; }
      public virtual ProofStatus? ProofStatus { get; set; }
      public virtual ICollection<Author>? Authors { get; set; }
   }
}
