using Domain.Entities;

namespace Domain.Interfaces
{
   public interface IGenericRepository<T> where T : BaseEntity
   {
      Task<T?> GetByIdAsync(Guid id);
      Task<IEnumerable<T>> GetAllAsync();
      Task CreateAsync(T entity);
      Task UpdateAsync(T entity);
      Task DeleteAsync(Guid id);
      //Task SaveChangesAsync();
   }
}
