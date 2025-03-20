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

   public async Task<IEnumerable<WorkTypeWithDetailsCountDto>> GetWorkTypesWithDetailsCountAsync(CancellationToken cancellationToken = default)
   {
      var entities = await workTypeRepository.GetWorkTypesWithDetailsCountAsync(cancellationToken);
      var dtos = entities.Select(wt => new WorkTypeWithDetailsCountDto
      {
         Id = wt.Id,
         Name = wt.Name,
         WorkLevelCount = wt.WorkLevels?.Count ?? 0,
         PurposeCount = wt.Purposes?.Count ?? 0,
         AuthorRoleCount = wt.AuthorRoles?.Count ?? 0,
         FactorCount = wt.Factors?.Count ?? 0,
         SCImagoFieldCount = wt.SCImagoFields?.Count ?? 0,
         WorkLevels = wt.WorkLevels?.Select(wl => new WorkLevelInfo 
         { 
            Id = wl.Id, 
            Name = wl.Name 
         }).ToList(),
         Purposes = wt.Purposes?.Select(p => new PurposeInfo 
         { 
            Id = p.Id, 
            Name = p.Name 
         }).ToList(),
         AuthorRoles = wt.AuthorRoles?.Select(ar => new AuthorRoleInfo 
         { 
            Id = ar.Id, 
            Name = ar.Name 
         }).ToList(),
         SCImagoFields = wt.SCImagoFields?.Select(sf => new SCImagoFieldInfo 
         { 
            Id = sf.Id, 
            Name = sf.Name 
         }).ToList(),
         CreatedDate = wt.CreatedDate,
         ModifiedDate = wt.ModifiedDate
      });

      return dtos;
   }
}
