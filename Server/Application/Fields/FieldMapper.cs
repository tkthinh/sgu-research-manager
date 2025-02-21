using Domain.Entities;
using Domain.Interfaces;

namespace Application.Fields
{
    public class FieldMapper : IGenericMapper<FieldDto, Field>
    {
        public FieldDto MapToDto(Field entity)
        {
            return new FieldDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public Field MapToEntity(FieldDto dto)
        {
            return new Field
            {
                Id = dto.Id,
                Name = dto.Name,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public IEnumerable<FieldDto> MapToDtos(IEnumerable<Field> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<Field> MapToEntities(IEnumerable<FieldDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}
