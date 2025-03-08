using Domain.Enums;

namespace Domain.Entities
{
    public class Work : BaseEntity
    {
        public required string Title { get; set; }
        public DateOnly? TimePublished { get; set; }
        public int? TotalAuthors { get; set; }       // Số tác giả của công trình
        public int? TotalMainAuthors { get; set; }   // Số tác giả chính của công trình
        public int FinalWorkHour { get; set; }      // Giờ chính thức của công trình được quy đổi do Admin chấm
        public ProofStatus ProofStatus { get; set; } // Trạng thái chứng minh
        public string? Note { get; set; }
        public Dictionary<string, string>? Details { get; set; }
        public WorkSource Source { get; set; }
        public Guid WorkTypeId { get; set; }
        public Guid WorkLevelId { get; set; }
        public Guid SCImagoFieldId { get; set; }
        public Guid ScoringFieldId { get; set; }


      // (Tuỳ chọn) Navigation properties đến các entity khác
      public virtual WorkType? WorkType { get; set; }
      public virtual WorkLevel? WorkLevel { get; set; }
      public virtual Field? FieldForScoring { get; set; }
      public virtual ICollection<Author>? Authors { get; set; }
   }
}