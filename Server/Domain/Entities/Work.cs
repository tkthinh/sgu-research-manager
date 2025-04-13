using Domain.Enums;

namespace Domain.Entities
{
    public class Work : BaseEntity
    {
        public required string Title { get; set; }
        public DateOnly? TimePublished { get; set; }
        public int? TotalAuthors { get; set; }
        public int? TotalMainAuthors { get; set; }
        public Dictionary<string, string>? Details { get; set; }
        public WorkSource Source { get; set; }
        public bool IsLocked { get; set; }

        public Guid WorkTypeId { get; set; }
        public Guid? WorkLevelId { get; set; }
        public Guid SystemConfigId { get; set; }

        public virtual WorkType? WorkType { get; set; }
        public virtual WorkLevel? WorkLevel { get; set; }
        public virtual SystemConfig? SystemConfig { get; set; }
        public virtual ICollection<Author>? Authors { get; set; } = new List<Author>();
        public virtual ICollection<WorkAuthor>? WorkAuthors { get; set; } = new List<WorkAuthor>();

        public DateOnly? ExchangeDeadline { get; set; }
    }
}