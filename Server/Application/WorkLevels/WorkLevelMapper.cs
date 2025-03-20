using Domain.Entities;
using Domain.Interfaces;

namespace Application.WorkLevels
{
    public class WorkLevelMapper : IGenericMapper<WorkLevelDto, WorkLevel>
    {
        public WorkLevelDto MapToDto(WorkLevel entity)
        {
            return new WorkLevelDto
            {
                Id = entity.Id,
                Name = entity.Name,
                WorkTypeId = entity.WorkTypeId,
                WorkTypeName = entity.WorkType?.Name,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public WorkLevel MapToEntity(WorkLevelDto dto)
        {
            return new WorkLevel
            {
                Id = dto.Id,
                Name = dto.Name,
                WorkTypeId = dto.WorkTypeId,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public IEnumerable<WorkLevelDto> MapToDtos(IEnumerable<WorkLevel> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<WorkLevel> MapToEntities(IEnumerable<WorkLevelDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}
