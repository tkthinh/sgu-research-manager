namespace Domain.Entities
{
    public class AcademicYear : BaseEntity
    {
        public required string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public virtual ICollection<SystemConfig>? SystemConfigs { get; set; }
    }
}
