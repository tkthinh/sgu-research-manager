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
                MarkedForScoring = entity.MarkedForScoring,
                ProofStatus = entity.ProofStatus,
                Note = entity.Note,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
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
                MarkedForScoring = dto.MarkedForScoring,
                ProofStatus = dto.ProofStatus,
                Note = dto.Note,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
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