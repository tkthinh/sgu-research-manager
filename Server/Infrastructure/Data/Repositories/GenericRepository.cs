//using Domain.Entities;
//using Domain.Interfaces;
//using Microsoft.EntityFrameworkCore;
//using System.Linq.Expressions;

//namespace Infrastructure.Data.Repositories
//{
//   public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
//   {
//      protected readonly ApplicationDbContext context;
//      private readonly DbSet<T> dbSet;

//      public GenericRepository(ApplicationDbContext context)
//      {
//         this.context = context;
//         dbSet = context.Set<T>();
//      }

//      public virtual async Task CreateAsync(T entity)
//      {
//         entity.CreatedDate = DateTime.UtcNow;
//         await dbSet.AddAsync(entity);
//      }
//      public virtual async Task<IEnumerable<T>> GetAllAsync()
//      {
//         return await dbSet.ToListAsync();
//      }

//      public virtual async Task<T?> GetByIdAsync(Guid id)
//      {
//         return await dbSet.FindAsync(id);
//      }

//      public virtual async Task UpdateAsync(T entity)
//      {
//         var existingEntity = await dbSet.FindAsync(entity.Id);
//         if (existingEntity is not null)
//         {
//            context.Entry(existingEntity).CurrentValues.SetValues(entity);
//         }
//         else
//         {
//            dbSet.Attach(entity);
//            context.Entry(entity).State = EntityState.Modified;
//         }

//         entity.ModifiedDate = DateTime.UtcNow;
//         await context.SaveChangesAsync();
//      }

//      public virtual async Task DeleteAsync(Guid id)
//      {
//         var entity = await dbSet.FindAsync(id);
//         if (entity is not null)
//         {
//            dbSet.Remove(entity);
//            await context.SaveChangesAsync();
//         }
//      }

//      public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
//      {
//         return await dbSet.Where(predicate).ToListAsync();
//      }
//   }
//}

using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext context;
        private readonly DbSet<T> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public virtual async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.CreatedDate = DateTime.UtcNow;
            await dbSet.AddAsync(entity, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await dbSet.ToListAsync(cancellationToken);
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var existingEntity = await dbSet.FindAsync(new object[] { entity.Id }, cancellationToken);
            if (existingEntity != null)
            {
                context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                dbSet.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            }

            entity.ModifiedDate = DateTime.UtcNow;
            // Không gọi SaveChangesAsync ở đây, để UnitOfWork xử lý
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await dbSet.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                dbSet.Remove(entity);
                // Không gọi SaveChangesAsync ở đây, để UnitOfWork xử lý
            }
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await dbSet.Where(predicate).ToListAsync(cancellationToken);
        }
    }
}