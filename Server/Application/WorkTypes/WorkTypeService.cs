using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.WorkTypes
{
    public class WorkTypeService : GenericCachedService<WorkTypeDto, WorkType>, IWorkTypeService
    {
        public WorkTypeService(
            IUnitOfWork unitOfWork,
            IGenericMapper<WorkTypeDto, WorkType> mapper,
            IDistributedCache cache
        ) : base(unitOfWork, mapper, cache)
        {
        }
    }
}
