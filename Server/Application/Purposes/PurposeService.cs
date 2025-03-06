using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Purposes
{
   public class PurposeService : GenericCachedService<PurposeDto, Purpose>, IPurposeService
   {
      public PurposeService(
          IUnitOfWork unitOfWork,
          IGenericMapper<PurposeDto, Purpose> mapper,
          IDistributedCache cache,
          ILogger<PurposeService> logger
          )
          : base(unitOfWork, mapper, cache, logger)
      {
      }

      public async Task<IEnumerable<PurposeDto>> GetPurposesByWorkTypeIdAsync(Guid workTypeId)
      {
         var workLevels = await unitOfWork.Repository<Purpose>()
             .FindAsync(wl => wl.WorkTypeId == workTypeId);

         return mapper.MapToDtos(workLevels);
      }
   }
}
