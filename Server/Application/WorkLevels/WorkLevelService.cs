using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.WorkLevels
{
   public class WorkLevelService : GenericCachedService<WorkLevelDto, WorkLevel>, IWorkLevelService
   {
      public WorkLevelService(
          IUnitOfWork unitOfWork,
          IGenericMapper<WorkLevelDto, WorkLevel> mapper,
          IDistributedCache cache,
          ILogger<WorkLevelService> logger
          )
          : base(unitOfWork, mapper, cache, logger)
      {
      }

      public async Task<IEnumerable<WorkLevelDto>> GetWorkLevelsByWorkTypeIdAsync(Guid workTypeId)
      {
         var workLevels = await unitOfWork.Repository<WorkLevel>()
             .FindAsync(wl => wl.WorkTypeId == workTypeId);

         return mapper.MapToDtos(workLevels);
      }

   }
}
