using Application.SystemConfigs;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.SystemConfigs
{
    public class SystemConfigMapper : IGenericMapper<SystemConfigDto, SystemConfig>
    {
        public SystemConfigDto MapToDto(SystemConfig entity)
        {
            return new SystemConfigDto
            {
                Id = entity.Id,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsClosed = entity.IsClosed,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public SystemConfig MapToEntity(SystemConfigDto dto)
        {
            return new SystemConfig
            {
                Id = dto.Id,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsClosed = dto.IsClosed,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public IEnumerable<SystemConfigDto> MapToDtos(IEnumerable<SystemConfig> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<SystemConfig> MapToEntities(IEnumerable<SystemConfigDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}