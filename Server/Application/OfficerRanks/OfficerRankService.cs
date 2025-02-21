using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.OfficerRanks
{
    public class OfficerRankService : GenericCachedService<OfficerRankDto, OfficerRank>, IOfficerRankService
    {
        public OfficerRankService(
            IUnitOfWork unitOfWork,
            IGenericMapper<OfficerRankDto, OfficerRank> mapper,
            IDistributedCache cache
        ) : base(unitOfWork, mapper, cache)
        {
        }
    }
}
