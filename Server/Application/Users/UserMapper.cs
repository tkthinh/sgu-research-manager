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
            Email = user.Email ?? "Không rõ",
            PhoneNumber = user.PhoneNumber ?? "Không rõ",
            AcademicTitle = user.AcademicTitle.ToString() ?? "Không rõ",
            OfficerRank = user.OfficerRank.ToString() ?? "Không rõ",
            IdentityId = user.IdentityId,
            Specialization = user.Specialization ?? "Không rõ",
            DepartmentId = user.DepartmentId,
            FieldId = user.FieldId ?? Guid.Empty,
            DepartmentName = user.Department?.Name ?? "Không rõ",
            FieldName = user.Field?.Name ?? "Không rõ",
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
