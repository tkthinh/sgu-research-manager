using Domain.Entities;
using Domain.Interfaces;

namespace Application.AcademicRanks
{
    public class AcademicRankMapper : IGenericMapper<AcademicRankDto, AcademicRank>
    {
        public AcademicRankDto MapToDto(AcademicRank entity)
        {
            return new AcademicRankDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public AcademicRank MapToEntity(AcademicRankDto dto)
        {
            return new AcademicRank
            {
                Id = dto.Id,
                Name = dto.Name,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public IEnumerable<AcademicRankDto> MapToDtos(IEnumerable<AcademicRank> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<AcademicRank> MapToEntities(IEnumerable<AcademicRankDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}
