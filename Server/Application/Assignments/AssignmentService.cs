using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Assignments
{
    public class AssignmentService : GenericCachedService<AssignmentDto, Assignment>, IAssignmentService
    {
        public AssignmentService(
            IUnitOfWork unitOfWork,
            IGenericMapper<AssignmentDto, Assignment> mapper,
            IDistributedCache cache
        ) : base(unitOfWork, mapper, cache)
        {
        }
    }
}
