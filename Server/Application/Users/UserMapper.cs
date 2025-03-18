using Domain.Entities;
using Application.Users;
using Domain.Enums;
using Domain.Interfaces;

public class UserMapper : IGenericMapper<UserDto, User>
{
   public UserDto MapToDto(User user)
   {
      return new UserDto
      {
         Id = user.Id,
         FullName = user.FullName,
         UserName = user.UserName,
         Email = user.Email,
         AcademicTitle = user.AcademicTitle.ToString(),
         OfficerRank = user.OfficerRank.ToString(),
         IdentityId = user.IdentityId,
         DepartmentId = user.DepartmentId,
         FieldId = user.FieldId,
         DepartmentName = user.Department?.Name ?? "Unknown",
         FieldName = user.Field?.Name ?? "Unknown",
         CreatedDate = user.CreatedDate,
         ModifiedDate = user.ModifiedDate
      };
   }

   public User MapToEntity(UserDto dto)
   {
      return new User
      {
         FullName = dto.FullName,
         UserName = dto.UserName,
         Email = dto.Email,
         AcademicTitle = Enum.Parse<AcademicTitle>(dto.AcademicTitle),
         OfficerRank = Enum.Parse<OfficerRank>(dto.OfficerRank),
         IdentityId = dto.IdentityId,
         DepartmentId = dto.DepartmentId,
         FieldId = dto.FieldId
      };
   }

   public IEnumerable<UserDto> MapToDtos(IEnumerable<User> entities)
   {
      return entities.Select(MapToDto);
   }

   public IEnumerable<User> MapToEntities(IEnumerable<UserDto> dtos)
   {
      return dtos.Select(MapToEntity);
   }
}
