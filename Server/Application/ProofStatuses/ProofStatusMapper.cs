using Domain.Entities;
using Domain.Interfaces;

namespace Application.ProofStatuses
{
    public class ProofStatusMapper : IGenericMapper<ProofStatusDto, ProofStatus>
    {
        public ProofStatusDto MapToDto(ProofStatus entity)
        {
            return new ProofStatusDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public ProofStatus MapToEntity(ProofStatusDto dto)
        {
            return new ProofStatus
            {
                Id = dto.Id,
                Name = dto.Name,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public IEnumerable<ProofStatusDto> MapToDtos(IEnumerable<ProofStatus> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<ProofStatus> MapToEntities(IEnumerable<ProofStatusDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}
