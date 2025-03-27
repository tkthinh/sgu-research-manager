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

    }
}
