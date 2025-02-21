using Domain.Entities;
using Domain.Interfaces;

namespace Application.OfficerRanks
{
    public class OfficerRankMapper : IGenericMapper<OfficerRankDto, OfficerRank>
    {
        public OfficerRankDto MapToDto(OfficerRank entity)
        {
            return new OfficerRankDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public OfficerRank MapToEntity(OfficerRankDto dto)
        {
            return new OfficerRank
            {
                Id = dto.Id,
                Name = dto.Name,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public IEnumerable<OfficerRankDto> MapToDtos(IEnumerable<OfficerRank> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<OfficerRank> MapToEntities(IEnumerable<OfficerRankDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}
