using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.ProofStatuses
{
    public class ProofStatusService : GenericCachedService<ProofStatusDto, ProofStatus>, IProofStatusService
    {
        public ProofStatusService(
            IUnitOfWork unitOfWork,
            IGenericMapper<ProofStatusDto, ProofStatus> mapper,
            IDistributedCache cache,
            ILogger<ProofStatusService> logger
        ) : base(unitOfWork, mapper, cache, logger)
        {
        }
    }
}
