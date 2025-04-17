using Application.AuthorRegistrations;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Authors
{
    public class AuthorMapper : IGenericMapper<AuthorDto, Author>
    {
        public AuthorDto MapToDto(Author entity)
        {
            return new AuthorDto
            {
                Id = entity.Id,
                WorkId = entity.WorkId,
                WorkTitle = entity.Work?.Title,
                UserId = entity.UserId,
                AuthorRoleId = entity.AuthorRoleId,
                AuthorRoleName = entity.AuthorRole?.Name,
                PurposeId = entity.PurposeId,
                PurposeName = entity.Purpose?.Name,
                SCImagoFieldId = entity.SCImagoFieldId,
                SCImagoFieldName = entity.SCImagoField?.Name,
                FieldId = entity.FieldId,
                FieldName = entity.Field?.Name,
                Position = entity.Position,
                ScoreLevel = entity.ScoreLevel,
                AuthorHour = entity.AuthorHour,
                WorkHour = entity.WorkHour,
                ProofStatus = entity.ProofStatus,
                Note = entity.Note,
                AuthorRegistration = entity.AuthorRegistration != null ? new AuthorRegistrationDto
                {
                    Id = entity.AuthorRegistration.Id,
                    AcademicYearId = entity.AuthorRegistration.AcademicYearId,
                    AuthorId = entity.AuthorRegistration.AuthorId,
                    AcademicYearName = entity.AuthorRegistration.AcademicYear?.Name,
                    AuthorName = entity.AuthorRegistration.Author?.User?.FullName ?? "-"
                } : null
            };
        }

        public Author MapToEntity(AuthorDto dto)
        {
            return new Author
            {
                Id = dto.Id,
                WorkId = dto.WorkId,
                UserId = dto.UserId,
                AuthorRoleId = dto.AuthorRoleId,
                PurposeId = dto.PurposeId,
                SCImagoFieldId = dto.SCImagoFieldId,
                FieldId = dto.FieldId,
                Position = dto.Position,
                ScoreLevel = dto.ScoreLevel,
                AuthorHour = dto.AuthorHour,
                WorkHour = dto.WorkHour,
                ProofStatus = dto.ProofStatus,
                Note = dto.Note,
            };
        }

        public IEnumerable<AuthorDto> MapToDtos(IEnumerable<Author> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<Author> MapToEntities(IEnumerable<AuthorDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}