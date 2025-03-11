using Domain.Enums;

namespace Domain.Entities
{
    public class Work : BaseEntity
    {
        public required string Title { get; set; }
        public DateOnly? TimePublished { get; set; }
        public int? TotalAuthors { get; set; }
        public int? TotalMainAuthors { get; set; }
        public int FinalWorkHour { get; set; }
        public ProofStatus ProofStatus { get; set; }
        public string? Note { get; set; }
        public Dictionary<string, string>? Details { get; set; }
        public WorkSource Source { get; set; }

        public Guid WorkTypeId { get; set; }
        public Guid? WorkLevelId { get; set; }
        public Guid? SCImagoFieldId { get; set; }
        public Guid? ScoringFieldId { get; set; }


      // (Tuỳ chọn) Navigation properties đến các entity khác
      public virtual WorkType? WorkType { get; set; }
      public virtual WorkLevel? WorkLevel { get; set; }
      public virtual Field? FieldForScoring { get; set; }
      public virtual ICollection<Author>? Authors { get; set; }
   }
}