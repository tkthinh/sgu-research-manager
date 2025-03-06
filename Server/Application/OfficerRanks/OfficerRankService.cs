using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.OfficerRanks
{
    public class OfficerRankService : GenericCachedService<OfficerRankDto, OfficerRank>, IOfficerRankService
    {
        public OfficerRankService(
            IUnitOfWork unitOfWork,
            IGenericMapper<OfficerRankDto, OfficerRank> mapper,
            IDistributedCache cache,
            ILogger<OfficerRankService> logger
        ) : base(unitOfWork, mapper, cache, logger)
        {
        }
    }
}
