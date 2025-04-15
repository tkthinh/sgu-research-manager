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
                Name = entity.Name,
                OpenTime = entity.OpenTime,
                CloseTime = entity.CloseTime,
                IsNotified = entity.IsNotified,
                AcademicYearId = entity.AcademicYearId,
                AcademicYearName = entity.AcademicYear?.Name
            };
        }

        public SystemConfig MapToEntity(SystemConfigDto dto)
        {
            return new SystemConfig
            {
                Id = dto.Id,
                Name = dto.Name ?? "-",
                OpenTime = dto.OpenTime,
                CloseTime = dto.CloseTime,
                IsNotified = dto.IsNotified,
                AcademicYearId = dto.AcademicYearId
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