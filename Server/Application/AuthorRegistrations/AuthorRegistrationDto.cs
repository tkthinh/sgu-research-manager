using Domain.Entities;

namespace Application.AuthorRegistrations
{
    public class AuthorRegistrationDto
    {
        public Guid AcademicYearId { get; set; }
        public Guid AuthorId { get; set; }

        public string? AcademicYearName { get; set; }
        public string? AuthorName { get; set; }
        public decimal? AuthorScore { get; set; }
    }
}
