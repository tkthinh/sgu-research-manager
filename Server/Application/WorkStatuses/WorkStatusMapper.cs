using Domain.Entities;
using Domain.Interfaces;

namespace Application.WorkStatuses
{
    public class WorkStatusMapper : IGenericMapper<WorkStatusDto, WorkStatus>
    {
        public WorkStatusDto MapToDto(WorkStatus entity)
        {
            return new WorkStatusDto
            {
                Id = entity.Id,
                Name = entity.Name,
                WorkTypeId = entity.WorkTypeId,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public WorkStatus MapToEntity(WorkStatusDto dto)
        {
            return new WorkStatus
            {
                Id = dto.Id,
                Name = dto.Name,
                WorkTypeId = dto.WorkTypeId,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public IEnumerable<WorkStatusDto> MapToDtos(IEnumerable<WorkStatus> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<WorkStatus> MapToEntities(IEnumerable<WorkStatusDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}
