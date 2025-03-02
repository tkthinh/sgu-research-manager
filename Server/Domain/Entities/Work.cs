using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public enum WorkSource
    {
        
    }

    public class Work : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public int WorkTypeId { get; set; }
        public int WorkLevelId { get; set; }
        public int WorkStatusId { get; set; }
        public int FieldId { get; set; }
        public int WorkProofId { get; set; }
        public DateTime? TimePublished { get; set; }
        public float ManagerWorkScore { get; set; }
        public string? Note { get; set; }
        public int TotalAuthors { get; set; }
        public int TotalHours { get; set; }
        public string? Detail { get; set; }
        public int MainAuthorCount { get; set; }
        public WorkSource Source { get; set; }

        // (Tuỳ chọn) Navigation properties đến các entity khác
        // public virtual WorkType? WorkType { get; set; }
        // public virtual WorkLevel? WorkLevel { get; set; }
        // public virtual WorkStatus? WorkStatus { get; set; }
        // public virtual Field? Field { get; set; }
        // public virtual WorkProof? WorkProof { get; set; }
    }
}
