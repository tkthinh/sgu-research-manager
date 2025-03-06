using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.AcademicRanks
{
    public class AcademicRankService : GenericCachedService<AcademicRankDto, AcademicRank>, IAcademicRankService
    {
        public AcademicRankService(
            IUnitOfWork unitOfWork,
            IGenericMapper<AcademicRankDto, AcademicRank> mapper,
            IDistributedCache cache,
            ILogger<AcademicRankService> logger
        )
        : base(unitOfWork, mapper, cache, logger)
        {
        }
    }
}
