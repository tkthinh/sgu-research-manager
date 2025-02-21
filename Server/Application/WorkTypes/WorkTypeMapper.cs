using Domain.Entities;
using Domain.Interfaces;

namespace Application.WorkTypes
{
    public class WorkTypeMapper : IGenericMapper<WorkTypeDto, WorkType>
    {
        public WorkTypeDto MapToDto(WorkType entity)
        {
            return new WorkTypeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public WorkType MapToEntity(WorkTypeDto dto)
        {
            return new WorkType
            {
                Id = dto.Id,
                Name = dto.Name,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public IEnumerable<WorkTypeDto> MapToDtos(IEnumerable<WorkType> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<WorkType> MapToEntities(IEnumerable<WorkTypeDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}
