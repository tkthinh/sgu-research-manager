namespace Domain.Entities
{
   public class Employee : BaseEntity
   {
      public required string FullName { get; set; }

      // Identity
      public required string IdentityId { get; set; }

      // Khóa ngoại
      public Guid DepartmentId { get; set; }
      public Guid AcademicRankId { get; set; }
      public Guid OfficerRankId { get; set; }
      public Guid FieldId { get; set; }

      // Liên kết khóa ngoại
      public virtual Department? Department { get; set; }
      public virtual AcademicRank? AcademicRank { get; set; }
      public virtual OfficerRank? OfficerRank { get; set; }
      public virtual Field? Field { get; set; }
   }
}
