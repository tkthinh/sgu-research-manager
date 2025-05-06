using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.WorkLevels
{
    public class WorkLevelService : GenericCachedService<WorkLevelDto, WorkLevel>, IWorkLevelService
    {
        protected override TimeSpan defaultCacheTime => TimeSpan.FromHours(24);

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

            // Lấy danh sách workLevel theo workTypeId
            Expression<Func<WorkLevel, bool>> filter = wl => wl.WorkTypeId == workTypeId;
            var workLevels = await unitOfWork.Repository<WorkLevel>().FindAsync(filter);

            if (workLevels == null || !workLevels.Any())
            {
                return Enumerable.Empty<WorkLevelDto>(); // Trả về danh sách rỗng thay vì null
            }

            // Load WorkType cho mỗi workLevel nếu cần
            foreach (var workLevel in workLevels)
            {
                // Load WorkType manually if not already loaded
                if (workLevel.WorkType == null)
                {
                    workLevel.WorkType = await unitOfWork.Repository<WorkType>().GetByIdAsync(workLevel.WorkTypeId);
                }
            }

            return mapper.MapToDtos(workLevels);
        }

        // Override để load WorkType khi lấy tất cả
        public override async Task<IEnumerable<WorkLevelDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            // Lấy tất cả WorkLevel từ base class
            var workLevels = await base.GetAllAsync(cancellationToken);

            // Nếu đã có WorkLevel, load thêm WorkTypes cho từng level
            if (workLevels != null && workLevels.Any())
            {
                // Lấy tất cả workTypes một lần để tránh nhiều lần truy vấn
                var workTypes = await unitOfWork.Repository<WorkType>().GetAllAsync();
                var workTypeDict = workTypes.ToDictionary(wt => wt.Id);

                // Lấy lại danh sách entities từ repository
                var workLevelEntities = await unitOfWork.Repository<WorkLevel>().GetAllAsync();

                // Gán WorkType cho từng WorkLevel entity
                foreach (var level in workLevelEntities)
                {
                    if (level.WorkType == null && workTypeDict.TryGetValue(level.WorkTypeId, out var workType))
                    {
                        level.WorkType = workType;
                    }
                }

                // Ánh xạ lại từ entities sang DTOs
                return mapper.MapToDtos(workLevelEntities);
            }

            return workLevels;
        }
    }
}
