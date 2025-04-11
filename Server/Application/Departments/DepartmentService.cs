using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Departments
{
    public class DepartmentService : GenericCachedService<DepartmentDto, Department>, IDepartmentService
    {
        public DepartmentService(
           IUnitOfWork unitOfWork,
           IGenericMapper<DepartmentDto, Department> mapper,
           IDistributedCache cache,
           ILogger<DepartmentService> logger
           )
           : base(unitOfWork, mapper, cache, logger)
        {
        }

        public async Task<DepartmentDto?> GetDepartmentByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var department = await unitOfWork.Repository<Department>().FirstOrDefaultAsync(
                predicate: d => d.Name == name,
                cancellationToken: cancellationToken
                );

            if (department == null)
            {
                return null;
            }

            return mapper.MapToDto(department);
        }

        public async Task<IEnumerable<DepartmentDto>> GetDepartmentsByManagerIdAsync(Guid managerId, CancellationToken cancellationToken = default)
        {
            var assignments = await unitOfWork.Repository<Assignment>().FindAsync(
                predicate: a => a.UserId == managerId
            );

            var departmentIds = assignments.Select(a => a.DepartmentId).Distinct();
            var departments = await unitOfWork.Repository<Department>().FindAsync(
                predicate: d => departmentIds.Contains(d.Id)
            );

            return mapper.MapToDtos(departments);
        }
    }
}
