using Application.Shared.Services;
using Application.WorkTypes;
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
            IDistributedCache cache,
            IGenericMapper<WorkTypeDto, WorkType> mappe)
            : base(unitOfWork, mapper, cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WorkLevelDto>> GetWorkLevelsByWorkTypeIdAsync(Guid workTypeId)
        {
            // Sử dụng phương thức FindAsync được định nghĩa trong repository
            var workLevels = await _unitOfWork.Repository<WorkLevel>()
                .FindAsync(wl => wl.WorkTypeId == workTypeId);
            return _mapper.MapToDtos(workLevels);
        }
    }
}
