using Domain.Entities;
using Domain.Interfaces;
using System.Linq;

namespace Application.BookExtraOptions
{
    public class BookExtraOptionMapper : IGenericMapper<BookExtraOptionDto, BookExtraOption>
    {
        public BookExtraOptionDto MapToDto(BookExtraOption entity)
        {
            return new BookExtraOptionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                WorkTypeId = entity.WorkTypeId,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public BookExtraOption MapToEntity(BookExtraOptionDto dto)
        {
            return new BookExtraOption
            {
                Id = dto.Id,
                Name = dto.Name,
                WorkTypeId = dto.WorkTypeId,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public IEnumerable<BookExtraOptionDto> MapToDtos(IEnumerable<BookExtraOption> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<BookExtraOption> MapToEntities(IEnumerable<BookExtraOptionDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}
