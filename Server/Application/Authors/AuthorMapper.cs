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
                Position = entity.Position,
                ScoreLevel = entity.ScoreLevel,
                FinalAuthorHour = entity.FinalAuthorHour,
                TempAuthorHour = entity.TempAuthorHour,
                TempWorkHour = entity.TempWorkHour,
                IsNotMatch = entity.IsNotMatch,
                MarkedForScoring = entity.MarkedForScoring,
                CoAuthors = entity.CoAuthors,
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
                Position = dto.Position,
                ScoreLevel = dto.ScoreLevel,
                FinalAuthorHour = dto.FinalAuthorHour,
                TempAuthorHour = dto.TempAuthorHour,
                TempWorkHour = dto.TempWorkHour,
                IsNotMatch = dto.IsNotMatch,
                MarkedForScoring = dto.MarkedForScoring,
                CoAuthors = dto.CoAuthors,
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
