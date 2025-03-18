using Domain.Entities;

namespace Domain.Interfaces
{
   public interface IUserRepository
   {
      Task<User?> GetUserByIdWithDetailsAsync(Guid id);
      Task<User?> GetUserByIdentityIdAsync(string identityId);
      Task<IEnumerable<User>> GetUsersWithDetailsAsync();
   }
}
