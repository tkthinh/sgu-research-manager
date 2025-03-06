using Domain.Entities;
using Domain.Interfaces;

namespace Application.Assignments
{
    public class AssignmentMapper : IGenericMapper<AssignmentDto, Assignment>
    {
        public AssignmentDto MapToDto(Assignment entity)
        {
            return new AssignmentDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                DepartmentId = entity.DepartmentId,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public Assignment MapToEntity(AssignmentDto dto)
        {
            return new Assignment
            {
                Id = dto.Id,
                UserId = dto.UserId,
                DepartmentId = dto.DepartmentId,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public IEnumerable<AssignmentDto> MapToDtos(IEnumerable<Assignment> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<Assignment> MapToEntities(IEnumerable<AssignmentDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}
