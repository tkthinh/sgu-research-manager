using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.AcademicRanks
{
    public class AcademicRankService : GenericCachedService<AcademicRankDto, AcademicRank>, IAcademicRankService
    {
        public AcademicRankService(
            IUnitOfWork unitOfWork,
            IGenericMapper<AcademicRankDto, AcademicRank> mapper,
            IDistributedCache cache
        )
        : base(unitOfWork, mapper, cache)
        {
        }
    }
}
