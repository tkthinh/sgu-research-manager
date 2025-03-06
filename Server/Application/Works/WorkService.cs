using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Works
{
   public class WorkService : GenericCachedService<WorkDto, Work>, IWorkService
   {
      public WorkService(
         IUnitOfWork unitOfWork,
         IGenericMapper<WorkDto, Work> mapper,
         IDistributedCache cache,
         ILogger<WorkService> logger
         )
         : base(unitOfWork, mapper, cache, logger)
      {
      }
   }
}
