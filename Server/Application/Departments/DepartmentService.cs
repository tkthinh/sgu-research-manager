using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Departments
{
   public class DepartmentService : GenericCachedService<DepartmentDto, Department>, IDepartmentService
   {
      public DepartmentService(
         IUnitOfWork unitOfWork,
         IGenericMapper<DepartmentDto, Department> mapper,
         IDistributedCache cache
         ) 
         : base(unitOfWork, mapper, cache)
      {
      }


   }
}
