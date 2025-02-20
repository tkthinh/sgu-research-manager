using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.WorkStatuses
{
    public class WorkStatusService : GenericCachedService<WorkStatusDto, WorkStatus>, IWorkStatusService
    {
        public WorkStatusService(
            IUnitOfWork unitOfWork,
            IGenericMapper<WorkStatusDto, WorkStatus> mapper,
            IDistributedCache cache
        ) : base(unitOfWork, mapper, cache)
        {
        }
    }
}
