namespace Application.SystemConfigs
{
    public class CreateSystemConfigRequestDto
    {
        public required string Name { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }

        public bool IsNotified { get; set; } = false;

        public Guid AcademicYearId { get; set; }
        public string? AcademicYearName { get; set; }
    }
}
