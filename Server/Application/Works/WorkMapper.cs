//using Domain.Entities;
//using Domain.Interfaces;
//using System.Linq;

//namespace Application.Works
//{
//    public class WorkMapper : IGenericMapper<WorkDto, Work>
//    {
//        public WorkDto MapToDto(Work entity)
//        {
//            return new WorkDto
//            {
//                Id = entity.Id,
//                Title = entity.Title,
//                TimePublished = entity.TimePublished,
//                TotalAuthors = entity.TotalAuthors,
//                TotalMainAuthors = entity.TotalMainAuthors,
//                FinalWorkHour = entity.FinalWorkHour,
//                Note = entity.Note,
//                Details = entity.Details,
//                Source = entity.Source,
//                WorkTypeId = entity.WorkTypeId,
//                WorkLevelId = entity.WorkLevelId,
//                SCImagoFieldId = entity.SCImagoFieldId,
//                ScoringFieldId = entity.ScoringFieldId,
//                ProofStatusId = entity.ProofStatusId,
//                CreatedDate = entity.CreatedDate,
//                ModifiedDate = entity.ModifiedDate
//            };
//        }

//        public Work MapToEntity(WorkDto dto)
//        {
//            return new Work
//            {
//                Id = dto.Id,
//                Title = dto.Title,
//                TimePublished = dto.TimePublished,
//                TotalAuthors = dto.TotalAuthors,
//                TotalMainAuthors = dto.TotalMainAuthors,
//                FinalWorkHour = dto.FinalWorkHour,
//                Note = dto.Note,
//                Details = dto.Details,
//                Source = dto.Source,
//                WorkTypeId = dto.WorkTypeId,
//                WorkLevelId = dto.WorkLevelId,
//                SCImagoFieldId = dto.SCImagoFieldId,
//                ScoringFieldId = dto.ScoringFieldId,
//                ProofStatusId = dto.ProofStatusId,
//                CreatedDate = dto.CreatedDate,
//                ModifiedDate = dto.ModifiedDate
//            };
//        }

//        public IEnumerable<WorkDto> MapToDtos(IEnumerable<Work> entities)
//        {
//            return entities.Select(MapToDto);
//        }

//        public IEnumerable<Work> MapToEntities(IEnumerable<WorkDto> dtos)
//        {
//            return dtos.Select(MapToEntity);
//        }
//    }
//}

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
                FinalWorkHour = entity.FinalWorkHour,
                Note = entity.Note,
                Details = entity.Details,
                Source = entity.Source,
                WorkTypeId = entity.WorkTypeId,
                WorkLevelId = entity.WorkLevelId,
                SCImagoFieldId = entity.SCImagoFieldId,
                ScoringFieldId = entity.ScoringFieldId,
                ProofStatusId = entity.ProofStatusId,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                Authors = entity.Authors != null ? _authorMapper.MapToDtos(entity.Authors).ToList() : null
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
                FinalWorkHour = dto.FinalWorkHour,
                Note = dto.Note,
                Details = dto.Details,
                Source = dto.Source,
                WorkTypeId = dto.WorkTypeId,
                WorkLevelId = dto.WorkLevelId,
                SCImagoFieldId = dto.SCImagoFieldId,
                ScoringFieldId = dto.ScoringFieldId,
                ProofStatusId = dto.ProofStatusId,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate,
                Authors = dto.Authors != null ? _authorMapper.MapToEntities(dto.Authors).ToList() : null
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