namespace Domain.Entities
{
    public class AuthorRegistration : BaseEntity
    {
        public Guid AcademicYearId { get; set; }
        public Guid AuthorId { get; set; }

        public virtual AcademicYear? AcademicYear { get; set; }
        public virtual Author? Author { get; set; }
    }
}
