namespace Application.SystemConfigs
{
    public class CreateSystemConfigRequestDto
    {
        public required string Name { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }

        public Guid AcademicYearId { get; set; }
        public string? AcademicYearName { get; set; }
    }
}
