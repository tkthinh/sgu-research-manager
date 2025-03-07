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
                Name = entity.Name,
                Score = entity.Score,
                ConvertHour = entity.ConvertHour,
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
                Name = dto.Name,
                Score = dto.Score,
                ConvertHour = dto.ConvertHour,
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
