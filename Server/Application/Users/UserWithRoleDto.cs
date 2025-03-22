using Domain.Enums;

namespace Application.Users
{
   public class UserWithRoleDto
   {
      public Guid Id { get; set; }
      public Guid DepartmentId { get; set; }
      public Guid FieldId { get; set; }
      public required string UserName { get; set; }
      public required string FullName { get; set; }
      public required string Email { get; set; }
      public required string PhoneNumber { get; set; }
      public required string Specialization { get; set; }
      public required string AcademicTitle { get; set; }
      public required string OfficerRank { get; set; }

      public string? DepartmentName { get; set; }
      public string? FieldName { get; set; }

      // Identity and Role
      public required string IdentityId { get; set; }
      public bool IsApproved { get; set; }
      public string? Role { get; set; }
   }
}
