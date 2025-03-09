using Domain.Entities;
using System.Linq.Expressions;

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
      Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
      Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
