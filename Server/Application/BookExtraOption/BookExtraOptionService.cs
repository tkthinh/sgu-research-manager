using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.BookExtraOptions
{
    public class BookExtraOptionService : GenericCachedService<BookExtraOptionDto, BookExtraOption>, IBookExtraOptionService
    {
        public BookExtraOptionService(IUnitOfWork unitOfWork, IGenericMapper<BookExtraOptionDto, BookExtraOption> mapper, IDistributedCache cache)
            : base(unitOfWork, mapper, cache)
        {
        }
    }
}
