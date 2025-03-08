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
        public int? TotalAuthors { get; set; }       // Số tác giả của công trình
        public int? TotalMainAuthors { get; set; }   // Số tác giả chính của công trình
        public int FinalWorkHour { get; set; }      // Giờ chính thức của công trình được quy đổi do Admin chấm
        public string? Note { get; set; }
        public Dictionary<string, string>? Details { get; set; }
        public WorkSource Source { get; set; }
        public Guid WorkTypeId { get; set; }
        public Guid WorkLevelId { get; set; }
        public Guid SCImagoFieldId { get; set; }
        public Guid ScoringFieldId { get; set; }
        public Guid ProofStatusId { get; set; }

        public virtual WorkType? WorkType { get; set; }
        public virtual WorkLevel? WorkLevel { get; set; }
        public virtual SCImagoField? SCImagoField { get; set; }
        public virtual Field? FieldForScoring { get; set; }
        public virtual ProofStatus? ProofStatus { get; set; }

        public virtual ICollection<Author>? Authors { get; set; }
    }
}