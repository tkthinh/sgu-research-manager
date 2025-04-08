namespace Application.AcademicYears
{
    public class AcademicYearDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
