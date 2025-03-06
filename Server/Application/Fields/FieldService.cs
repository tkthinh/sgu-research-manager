using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Fields
{
   public class FieldService : GenericCachedService<FieldDto, Field>, IFieldService
   {
      public FieldService(
         IUnitOfWork unitOfWork,
         IGenericMapper<FieldDto, Field> mapper,
         IDistributedCache cache,
         ILogger<FieldService> logger
         )
         : base(unitOfWork, mapper, cache, logger)
      {
      }


   }
}
