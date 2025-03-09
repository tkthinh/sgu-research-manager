using Domain.Interfaces;

namespace Application.Users
{
   public interface IUserService : IGenericService<UserDto>
   {
      Task<UserDto?> GetUserByIdentityIdAsync(string IdentityId);
   }
}
