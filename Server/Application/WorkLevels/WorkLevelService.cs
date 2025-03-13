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
            if (workTypeId == Guid.Empty)
            {
                throw new ArgumentException("WorkTypeId không hợp lệ", nameof(workTypeId));
            }

            var workLevels = await unitOfWork.Repository<WorkLevel>()
                .FindAsync(wl => wl.WorkTypeId == workTypeId);

            if (workLevels == null)
            {
                return Enumerable.Empty<WorkLevelDto>(); // Trả về danh sách rỗng thay vì null
            }

            return mapper.MapToDtos(workLevels);
        }

    }
}
