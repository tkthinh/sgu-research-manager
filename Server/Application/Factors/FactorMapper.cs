using Domain.Entities;
using Domain.Interfaces;

namespace Application.Factors
{
    public class FactorMapper : IGenericMapper<FactorDto, Factor>
    {
        public FactorDto MapToDto(Factor entity)
        {
            return new FactorDto
            {
                Id = entity.Id,
                WorkTypeId = entity.WorkTypeId,
                WorkLevelId = entity.WorkLevelId,
                PurposeId = entity.PurposeId,
                AuthorRoleId = entity.AuthorRoleId,
                Name = entity.Name,
                ScoreLevel = entity.ScoreLevel,
                ConvertHour = entity.ConvertHour,
                MaxAllowed = entity.MaxAllowed,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public Factor MapToEntity(FactorDto dto)
        {
            return new Factor
            {
                Id = dto.Id,
                WorkTypeId = dto.WorkTypeId,
                WorkLevelId = dto.WorkLevelId,
                PurposeId = dto.PurposeId,
                AuthorRoleId = dto.AuthorRoleId,
                Name = dto.Name,
                ScoreLevel = dto.ScoreLevel,
                ConvertHour = dto.ConvertHour,
                MaxAllowed = dto.MaxAllowed,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public IEnumerable<FactorDto> MapToDtos(IEnumerable<Factor> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<Factor> MapToEntities(IEnumerable<FactorDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}
