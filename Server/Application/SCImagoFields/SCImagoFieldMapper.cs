using Domain.Entities;
using Domain.Interfaces;

namespace Application.SCImagoFields
{
    public class SCImagoFieldMapper : IGenericMapper<SCImagoFieldDto, SCImagoField>
    {
        public SCImagoFieldDto MapToDto(SCImagoField entity)
        {
            return new SCImagoFieldDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public SCImagoField MapToEntity(SCImagoFieldDto dto)
        {
            return new SCImagoField
            {
                Id = dto.Id,
                Name = dto.Name,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public IEnumerable<SCImagoFieldDto> MapToDtos(IEnumerable<SCImagoField> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<SCImagoField> MapToEntities(IEnumerable<SCImagoFieldDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}
