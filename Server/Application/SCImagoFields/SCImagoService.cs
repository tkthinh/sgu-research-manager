using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.SCImagoFields
{
    public class SCImagoFieldService : GenericCachedService<SCImagoFieldDto, SCImagoField>, ISCImagoFieldService
    {
        public SCImagoFieldService(
            IUnitOfWork unitOfWork,
            IGenericMapper<SCImagoFieldDto, SCImagoField> mapper,
            IDistributedCache cache,
            ILogger<SCImagoFieldService> logger
            )
            : base(unitOfWork, mapper, cache, logger)
        {
        }

        public async Task<IEnumerable<SCImagoFieldDto>> GetSCImagoFieldsByWorkTypeIdAsync(Guid workTypeId)
        {
            var workLevels = await unitOfWork.Repository<SCImagoField>()
                .FindAsync(wl => wl.WorkTypeId == workTypeId);

            return mapper.MapToDtos(workLevels);
        }

    }
}
