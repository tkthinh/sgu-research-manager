using Domain.Entities;
using Domain.Interfaces;

namespace Application.Purposes
{
   public class PurposeMapper : IGenericMapper<PurposeDto, Purpose>
   {
      public PurposeDto MapToDto(Purpose entity)
      {
         return new PurposeDto
         {
            Id = entity.Id,
            Name = entity.Name,
            WorkTypeId = entity.WorkTypeId,
            WorkTypeName = entity.WorkType?.Name,
            CreatedDate = entity.CreatedDate,
            ModifiedDate = entity.ModifiedDate
         };
      }
      public Purpose MapToEntity(PurposeDto dto)
      {
         return new Purpose
         {
            Id = dto.Id,
            Name = dto.Name,
            WorkTypeId = dto.WorkTypeId,
            CreatedDate = dto.CreatedDate,
            ModifiedDate = dto.ModifiedDate
         };
      }

      public IEnumerable<PurposeDto> MapToDtos(IEnumerable<Purpose> entities)
      {
         return entities.Select(MapToDto);
      }

      public IEnumerable<Purpose> MapToEntities(IEnumerable<PurposeDto> dtos)
      {
         return dtos.Select(MapToEntity);
      }

      
   }
}
