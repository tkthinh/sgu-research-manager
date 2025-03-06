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


   }
}
