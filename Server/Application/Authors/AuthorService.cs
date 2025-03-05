using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Authors
{
    public class AuthorService : GenericCachedService<AuthorDto, Author>, IAuthorService
    {
        public AuthorService(
            IUnitOfWork unitOfWork,
            IGenericMapper<AuthorDto, Author> mapper,
            IDistributedCache cache)
            : base(unitOfWork, mapper, cache)
        {
        }
    }
}
