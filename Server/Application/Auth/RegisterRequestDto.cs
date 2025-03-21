using Application.Users;

namespace Application.Auth
{
   public class RegisterRequestDto
   {
      public required string Email { get; set; }
      public required string Password { get; set; }

      public required string FullName { get; set; }
      public required string UserName { get; set; }
      public required string PhoneNumber { get; set; }
      public required string Specialization { get; set; }
      public required string AcademicTitle { get; set; }
      public required string OfficerRank { get; set; }

      public Guid DepartmentId { get; set; }
      public Guid FieldId { get; set; }
   }
}
