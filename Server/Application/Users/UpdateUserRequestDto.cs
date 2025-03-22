using Domain.Enums;

namespace Application.Users
{
   public class UpdateUserRequestDto
   {
      public required string FullName { get; set; }
      public required string Email { get; set; }
      public required string PhoneNumber { get; set; }
      public required string Specialization { get; set; }
      public required string AcademicTitle { get; set; }
      public required string OfficerRank { get; set; }
      public Guid DepartmentId { get; set; }
      public Guid FieldId { get; set; }
   }

   public class UpdateUserAdminRequestDto : UpdateUserRequestDto
   {
      public required string Role { get; set; }
      public bool IsApproved { get; set; }
   }
}
