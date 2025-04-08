using Domain.Enums;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public required string FullName { get; set; }
        public required string UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Specialization { get; set; }
        public AcademicTitle? AcademicTitle { get; set; }
        public OfficerRank? OfficerRank { get; set; }

        // Identity
        public required string IdentityId { get; set; }

        // Khóa ngoại
        public Guid DepartmentId { get; set; }
        public Guid? FieldId { get; set; }

        // Liên kết khóa ngoại
        public virtual Department? Department { get; set; }
        public virtual Field? Field { get; set; }
        public virtual ICollection<Assignment>? Assignments { get; set; }
        public virtual ICollection<Author>? Authors { get; set; }
    }
}
