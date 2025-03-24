using Application.Shared.Services;
using Application.Users;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Assignments
{
   public class AssignmentService : GenericCachedService<AssignmentDto, Assignment>, IAssignmentService
   {
      private readonly IAssignmentRepository repository;
      public AssignmentService(
          IUnitOfWork unitOfWork,
          IAssignmentRepository repository,
          IGenericMapper<AssignmentDto, Assignment> mapper,
          IDistributedCache cache,
          ILogger<AssignmentService> logger
      ) : base(unitOfWork, mapper, cache, logger)
      {
         this.repository = repository;
      }

      public override async Task<IEnumerable<AssignmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
      {
         var assignments = await repository.GetAssignmentsWithDetailsAsync(cancellationToken);

         return mapper.MapToDtos(assignments);
      }
   }
}
