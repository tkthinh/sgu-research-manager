using System.Security.Cryptography.X509Certificates;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Users
{
   public class UserService : GenericService<UserDto, User>, IUserService
   {
      public UserService(
          IUnitOfWork unitOfWork,
          IGenericMapper<UserDto, User> mapper,
          ILogger<UserService> logger
          )
          : base(unitOfWork, mapper, logger)
      {
      }

      public async Task<UserDto?> GetUserByIdentityIdAsync(string IdentityId)
      {
         var user = await unitOfWork.Repository<User>()
             .FirstOrDefaultAsync(u => u.IdentityId == IdentityId);

         if (user is not null)
         {
            return mapper.MapToDto(user);
         }

         return null;
      }
   }
}
