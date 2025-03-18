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
      private readonly IUserRepository repository;

      public UserService(
          IUnitOfWork unitOfWork,
          IGenericMapper<UserDto, User> mapper,
          IUserRepository repository,
          ILogger<UserService> logger
          )
          : base(unitOfWork, mapper, logger)
      {
         this.repository = repository;
      }

      public override async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
      {
         var user = await repository.GetUserByIdWithDetailsAsync(id);

         if(user is not null)
         {
            return mapper.MapToDto(user);
         }

         return null;
      }

      public override async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
      {
         var users = await repository.GetUsersWithDetailsAsync();

         return mapper.MapToDtos(users);
      }

      public async Task<UserDto?> GetUserByIdentityIdAsync(string identityId)
      {
         var user = await repository.GetUserByIdentityIdAsync(identityId);

         if (user is not null)
         {
            return mapper.MapToDto(user);
         }

         return null;
      }
   }
}
