using Domain.Entities;
using Domain.Interfaces;
using System.Linq;

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
                PurposeId = entity.PurposeId,
                SCImagoFieldId = entity.SCImagoFieldId,
                ScoringFieldId = entity.ScoringFieldId,
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
                ScoringFieldId = dto.ScoringFieldId,
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