using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Factors
{
    public class FactorService : GenericCachedService<FactorDto, Factor>, IFactorService
    {
        public FactorService(
            IUnitOfWork unitOfWork,
            IGenericMapper<FactorDto, Factor> mapper,
            IDistributedCache cache
        ) : base(unitOfWork, mapper, cache)
        {
        }
    }
}
