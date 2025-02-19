using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Departments
{
   public class DepartmentService : GenericService<DepartmentDto, Department>, IDepartmentService
   {
      public DepartmentService(IUnitOfWork unitOfWork, IGenericMapper<DepartmentDto, Department> mapper) 
         : base(unitOfWork, mapper)
      {
      }


   }
}
