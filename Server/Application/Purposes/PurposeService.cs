using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Purposes
{
    public class PurposeService : GenericCachedService<PurposeDto, Purpose>, IPurposeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper<PurposeDto, Purpose> _mapper;
        public PurposeService(
            IUnitOfWork unitOfWork,
            IGenericMapper<PurposeDto, Purpose> mapper,
            IDistributedCache cache)
            : base(unitOfWork, mapper, cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PurposeDto>> GetPurposesByWorkTypeIdAsync(Guid workTypeId)
        {
            var workLevels = await _unitOfWork.Repository<Purpose>()
                .FindAsync(wl => wl.WorkTypeId == workTypeId);

            return _mapper.MapToDtos(workLevels);
        }
    }
}
