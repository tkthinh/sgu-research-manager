using Domain.Entities;
using Domain.Interfaces;
using System.Linq;

namespace Application.Works
{
    public class WorkMapper : IGenericMapper<WorkDto, Work>
    {
        public WorkDto MapToDto(Work entity)
        {
            return new WorkDto
            {
                Id = entity.Id,
                Title = entity.Title,
                TimePublished = entity.TimePublished,
                Source = entity.Source,
                Note = entity.Note,
                Details = entity.Details,
                WorkTypeId = entity.WorkTypeId,
                WorkLevelId = entity.WorkLevelId,
                WorkStatusId = entity.WorkStatusId,
                ScoringFieldId = entity.ScoringFieldId,
                WorkProofId = entity.WorkProofId,
                ManagerWorkScore = entity.ManagerWorkScore,
                TotalAuthors = entity.TotalAuthors,
                TotalHours = entity.TotalHours,
                MainAuthorCount = entity.MainAuthorCount,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public Work MapToEntity(WorkDto dto)
        {
            return new Work
            {
                Id = dto.Id,
                Title = dto.Title,
                TimePublished = dto.TimePublished,
                Source = dto.Source,
                Note = dto.Note,
                Details = dto.Details,
                WorkTypeId = dto.WorkTypeId,
                WorkLevelId = dto.WorkLevelId,
                WorkStatusId = dto.WorkStatusId,
                ScoringFieldId = dto.ScoringFieldId,
                WorkProofId = dto.WorkProofId,
                ManagerWorkScore = dto.ManagerWorkScore,
                TotalAuthors = dto.TotalAuthors,
                TotalHours = dto.TotalHours,
                MainAuthorCount = dto.MainAuthorCount,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public IEnumerable<WorkDto> MapToDtos(IEnumerable<Work> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<Work> MapToEntities(IEnumerable<WorkDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}
