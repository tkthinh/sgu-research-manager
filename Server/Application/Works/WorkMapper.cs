using Application.Authors;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Works
{
    public class WorkMapper : IGenericMapper<WorkDto, Work>
    {
        private readonly IGenericMapper<AuthorDto, Author> _authorMapper;

        public WorkMapper(IGenericMapper<AuthorDto, Author> authorMapper)
        {
            _authorMapper = authorMapper;
        }

        public WorkDto MapToDto(Work entity)
        {
            return new WorkDto
            {
                Id = entity.Id,
                Title = entity.Title,
                TimePublished = entity.TimePublished,
                TotalAuthors = entity.TotalAuthors,
                TotalMainAuthors = entity.TotalMainAuthors,
                Details = entity.Details,
                Source = entity.Source,
                WorkTypeId = entity.WorkTypeId,
                WorkTypeName = entity.WorkType?.Name,
                WorkLevelId = entity.WorkLevelId,
                WorkLevelName = entity.WorkLevel?.Name,
                ExchangeDeadline = entity.ExchangeDeadline,
                Authors = entity.Authors != null ? _authorMapper.MapToDtos(entity.Authors) : null,
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
                TotalAuthors = dto.TotalAuthors,
                TotalMainAuthors = dto.TotalMainAuthors,
                Details = dto.Details,
                Source = dto.Source,
                WorkTypeId = dto.WorkTypeId,
                WorkLevelId = dto.WorkLevelId,
                Authors = dto.Authors != null ? _authorMapper.MapToEntities(dto.Authors).ToList() : null,
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