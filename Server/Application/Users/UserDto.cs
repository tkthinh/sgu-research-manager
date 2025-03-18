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
      public required string Email { get; set; }
      public required string AcademicTitle { get; set; }
      public required string OfficerRank { get; set; }
      public DateTime CreatedDate { get; set; }
      public DateTime? ModifiedDate { get; set; }

      public string? DepartmentName { get; set; }
      public string? FieldName { get; set; }

      // Identity
      public required string IdentityId { get; set; }
      public string? Role { get; set; }
   }
}
