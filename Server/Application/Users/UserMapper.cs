using Domain.Entities;
using Application.Users;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Users;
public class UserMapper : IGenericMapper<UserDto, User>
{
    public UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            UserName = user.UserName,
            Email = user.Email ?? "-",
            PhoneNumber = user.PhoneNumber ?? "-",
            AcademicTitle = user.AcademicTitle.ToString() ?? "-",
            OfficerRank = user.OfficerRank.ToString() ?? "-",
            IdentityId = user.IdentityId,
            Specialization = user.Specialization ?? "-",
            DepartmentId = user.DepartmentId,
            FieldId = user.FieldId ?? Guid.Empty,
            DepartmentName = user.Department?.Name ?? "-",
            FieldName = user.Field?.Name ?? "-",
        };
    }

    public User MapToEntity(UserDto dto)
    {
        return new User
        {
            Id = dto.Id,
            FullName = dto.FullName,
            UserName = dto.UserName,
            Email = dto.Email ?? null,
            PhoneNumber = dto.PhoneNumber ?? null,
            AcademicTitle = Enum.TryParse(dto.AcademicTitle, out AcademicTitle parsedTitle)
                                          ? parsedTitle
                                          : AcademicTitle.Unknown,
            OfficerRank = Enum.TryParse(dto.OfficerRank, out OfficerRank parsedOfficerRank)
                                        ? parsedOfficerRank :
                                        OfficerRank.Unknown,
            Specialization = dto.Specialization,
            IdentityId = dto.IdentityId,
            DepartmentId = dto.DepartmentId,
            FieldId = dto.FieldId != Guid.Empty ? dto.FieldId : null
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
