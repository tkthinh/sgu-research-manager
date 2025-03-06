using Application.Shared.Services;
using Application.WorkTypes;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public class WorkTypeService : GenericCachedService<WorkTypeDto, WorkType>, IWorkTypeService
{
   private readonly IWorkTypeRepository workTypeRepository;
   public WorkTypeService(
       IUnitOfWork unitOfWork,
       IGenericMapper<WorkTypeDto, WorkType> mapper,
       IDistributedCache cache,
       ILogger<WorkTypeService> logger,
       IWorkTypeRepository workTypeRepository
       )
       : base(unitOfWork, mapper, cache, logger)
   {
      this.workTypeRepository = workTypeRepository;
   }

   public async Task<IEnumerable<WorkTypeWithLevelCountDto>> GetWorkTypesWithCountAsync(CancellationToken cancellationToken = default)
   {
      var entities = await workTypeRepository.GetWorkTypesWithLevelCountAsync(cancellationToken);
      var dtos = entities.Select(wt => new WorkTypeWithLevelCountDto
      {
         Id = wt.Id,
         Name = wt.Name,
         WorkLevelCount = wt.WorkLevels?.Count ?? 0
      });

      return dtos;
   }
}
