using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.WorkLevels
{
    public class WorkLevelService : GenericCachedService<WorkLevelDto, WorkLevel>, IWorkLevelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper<WorkLevelDto, WorkLevel> _mapper;
        public WorkLevelService(
            IUnitOfWork unitOfWork,
            IGenericMapper<WorkLevelDto, WorkLevel> mapper,
            IDistributedCache cache)
            : base(unitOfWork, mapper, cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WorkLevelDto>> GetWorkLevelsByWorkTypeIdAsync(Guid workTypeId)
        {
            var workLevels = await _unitOfWork.Repository<WorkLevel>()
                .FindAsync(wl => wl.WorkTypeId == workTypeId);

            return _mapper.MapToDtos(workLevels);
        }
    }
}
