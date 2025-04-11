using Domain.Entities;
using Domain.Interfaces;

namespace Application.AuthorRegistrations
{
    public class AuthorRegistrationMapper : IGenericMapper<AuthorRegistrationDto, AuthorRegistration>
    {
        public AuthorRegistrationDto MapToDto(AuthorRegistration entity)
        {
            return new AuthorRegistrationDto
            {
                AcademicYearId = entity.AcademicYearId,
                AuthorId = entity.AuthorId,
                AcademicYearName = entity.AcademicYear?.Name,
                AuthorName = entity.Author?.User?.FullName ?? "-"
            };
        }

        public AuthorRegistration MapToEntity(AuthorRegistrationDto dto)
        {
            return new AuthorRegistration
            {
                AcademicYearId = dto.AcademicYearId,
                AuthorId = dto.AuthorId,
            };
        }

        public IEnumerable<AuthorRegistrationDto> MapToDtos(IEnumerable<AuthorRegistration> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<AuthorRegistration> MapToEntities(IEnumerable<AuthorRegistrationDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}
