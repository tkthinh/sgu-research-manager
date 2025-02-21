using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Fields
{
    public class FieldService : GenericCachedService<FieldDto, Field>, IFieldService
    {
        public FieldService(
           IUnitOfWork unitOfWork,
           IGenericMapper<FieldDto, Field> mapper,
           IDistributedCache cache
           )
           : base(unitOfWork, mapper, cache)
        {
        }


    }
}
