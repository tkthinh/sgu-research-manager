using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.WorkLevels
{
    public class WorkLevelService : GenericCachedService<WorkLevelDto, WorkLevel>, IWorkLevelService
    {
        public WorkLevelService(
            IUnitOfWork unitOfWork,
            IGenericMapper<WorkLevelDto, WorkLevel> mapper,
            IDistributedCache cache
            ) : base(unitOfWork, mapper, cache)
        {

        }
    }
}
