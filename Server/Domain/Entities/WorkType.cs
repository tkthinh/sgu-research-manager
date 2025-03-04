namespace Domain.Entities
{
    public class WorkType : BaseEntity
    {
        public required string Name { get; set; }
        public bool HasExtraOption { get; set; } // True nếu là Sách, false nếu các loại còn lại
        public ICollection<AuthorRole>? AuthorRoles { get; set; }
        public ICollection<WorkStatus>? WorkStatuses { get; set; }
        public ICollection<Factor>? Factors { get; set; }
        public ICollection<WorkLevel>? WorkLevels { get; set; }
        public ICollection<BookExtraOption>? BookExtraOptions { get; set; }
    }
}
