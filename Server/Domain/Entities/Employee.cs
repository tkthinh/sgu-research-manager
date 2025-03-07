using Domain.Enums;

namespace Domain.Entities
{
   public class Employee : BaseEntity
   {
      public required string FullName { get; set; }
      public AcademicTitle AcademicTitle { get; set; }
      public OfficerRank OfficerRank { get; set; }

      // Identity
      public required string IdentityId { get; set; }

      // Khóa ngoại
      public Guid DepartmentId { get; set; }
      public Guid FieldId { get; set; }

      // Liên kết khóa ngoại
      public virtual Department? Department { get; set; }
      public virtual Field? Field { get; set; }
   }
}
