using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Factors
{
    public class FactorService : GenericCachedService<FactorDto, Factor>, IFactorService
    {
        public FactorService(
            IUnitOfWork unitOfWork,
            IGenericMapper<FactorDto, Factor> mapper,
            IDistributedCache cache,
            ILogger<FactorService> logger
        ) : base(unitOfWork, mapper, cache, logger)
        {
        }
    }
}
