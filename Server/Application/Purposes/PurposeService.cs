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
            if (workTypeId == Guid.Empty)
            {
                throw new ArgumentException("WorkTypeId không hợp lệ", nameof(workTypeId));
            }

            var purposes = await unitOfWork.Repository<Purpose>()
                .FindAsync(p => p.WorkTypeId == workTypeId);

            if (purposes == null)
            {
                return Enumerable.Empty<PurposeDto>();
            }

            return mapper.MapToDtos(purposes);
        }
    }
}
