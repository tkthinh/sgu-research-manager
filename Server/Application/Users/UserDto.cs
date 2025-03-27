using Domain.Enums;

namespace Application.Users
{
   public class UserDto
   {
      public Guid Id { get; set; }
      public Guid DepartmentId { get; set; }
      public Guid FieldId { get; set; }
      public required string UserName { get; set; }
      public required string FullName { get; set; }
      public string? Email { get; set; }
      public string? PhoneNumber { get; set; }
      public string? Specialization { get; set; }
      public string? AcademicTitle { get; set; }
      public string? OfficerRank { get; set; }

      public string? DepartmentName { get; set; }
      public string? FieldName { get; set; }

      // Identity
      public required string IdentityId { get; set; }
   }
}
