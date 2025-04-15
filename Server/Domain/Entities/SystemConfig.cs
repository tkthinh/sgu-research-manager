namespace Domain.Entities
{
    public class SystemConfig : BaseEntity
    {
        public required string Name { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }

        // Soft delete flag
        public bool IsDeleted { get; set; } = false;
        public bool IsNotified { get; set; } = false;

        public Guid AcademicYearId { get; set; }
        public virtual AcademicYear? AcademicYear { get; set; }
    }
}
