using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Purposes
{
   public class PurposeService : GenericCachedService<PurposeDto, Purpose>, IPurposeService
   {
      public PurposeService(
         IUnitOfWork unitOfWork,
         IGenericMapper<PurposeDto, Purpose> mapper,
         IDistributedCache cache
         ) 
         : base (unitOfWork, mapper, cache)
      {
      }
   }
}
