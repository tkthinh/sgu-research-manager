using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Assignments
{
    public class AssignmentService : GenericCachedService<AssignmentDto, Assignment>, IAssignmentService
    {
        public AssignmentService(
            IUnitOfWork unitOfWork,
            IGenericMapper<AssignmentDto, Assignment> mapper,
            IDistributedCache cache,
            ILogger<AssignmentService> logger
        ) : base(unitOfWork, mapper, cache, logger)
        {
        }
    }
}
