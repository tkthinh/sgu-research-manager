using Domain.Entities;
using Domain.Interfaces;

namespace Application.AcademicYears
{
    public class AcademicYearMapper : IGenericMapper<AcademicYearDto, AcademicYear>
    {
        public AcademicYearDto MapToDto(AcademicYear entity)
        {
            return new AcademicYearDto
            {
                Id = entity.Id,
                Name = entity.Name,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
            };
        }
        public AcademicYear MapToEntity(AcademicYearDto dto)
        {
            return new AcademicYear
            {
                Id = dto.Id,
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
            };
        }
        public IEnumerable<AcademicYearDto> MapToDtos(IEnumerable<AcademicYear> entities)
        {
            return entities.Select(MapToDto);
        }
        public IEnumerable<AcademicYear> MapToEntities(IEnumerable<AcademicYearDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
    
}
