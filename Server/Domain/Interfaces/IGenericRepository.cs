//using Domain.Entities;
//using System.Linq.Expressions;

//namespace Domain.Interfaces
//{
//   public interface IGenericRepository<T> where T : BaseEntity
//   {
//      Task<T?> GetByIdAsync(Guid id);
//      Task<IEnumerable<T>> GetAllAsync();
//      Task CreateAsync(T entity);
//      Task UpdateAsync(T entity);
//      Task DeleteAsync(Guid id);
//        //Task SaveChangesAsync();
//      Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
//    }
//}

using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        // Task SaveChangesAsync(CancellationToken cancellationToken = default); // Quản lý trong IUnitOfWork
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    }
}