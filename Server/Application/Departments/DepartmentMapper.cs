using Domain.Entities;
using Domain.Interfaces;

namespace Application.Departments
{
   public class DepartmentMapper : IGenericMapper<DepartmentDto, Department>
   {
      public DepartmentDto MapToDto(Department entity)
      {
         return new DepartmentDto
         {
            Id = entity.Id,
            Name = entity.Name,
            CreatedDate = entity.CreatedDate,
            ModifiedDate = entity.ModifiedDate
         };
      }

      public Department MapToEntity(DepartmentDto dto)
      {
         return new Department
         {
            Id = dto.Id,
            Name = dto.Name,
            CreatedDate = dto.CreatedDate,
            ModifiedDate = dto.ModifiedDate
         };
      }

      public IEnumerable<DepartmentDto> MapToDtos(IEnumerable<Department> entities)
      {
         return entities.Select(MapToDto);
      }

      public IEnumerable<Department> MapToEntities(IEnumerable<DepartmentDto> dtos)
      {
         return dtos.Select(MapToEntity);
      }
   }

}
