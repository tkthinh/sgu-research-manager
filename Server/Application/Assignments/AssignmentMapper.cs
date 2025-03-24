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
            ManagerId = entity.UserId,
            ManagerFullName = entity.User?.FullName ?? null,
            ManagerDepartmentName = entity.User?.Department?.Name ?? null,
            DepartmentId = entity.DepartmentId,
            AssignedDepartmentName = entity.Department?.Name ?? null,
         };
      }

      public Assignment MapToEntity(AssignmentDto dto)
      {
         return new Assignment
         {
            Id = dto.Id,
            UserId = dto.ManagerId,
            DepartmentId = dto.DepartmentId,
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
