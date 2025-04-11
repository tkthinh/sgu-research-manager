using Domain.Interfaces;

namespace Application.Departments
{
   public interface IDepartmentService : IGenericService<DepartmentDto>
   {
        Task<DepartmentDto?> GetDepartmentByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<DepartmentDto>> GetDepartmentsByManagerIdAsync(Guid managerId, CancellationToken cancellationToken = default);
   }
}
