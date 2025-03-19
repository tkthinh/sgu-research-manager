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
        protected IQueryable<T> Query => dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public virtual IQueryable<T> Include(Expression<Func<T, object>> includeExpression)
        {
            return Query.Include(includeExpression);
        }

        public virtual async Task CreateAsync(T entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            await dbSet.AddAsync(entity);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await dbSet.FindAsync(new object[] { id });
        }

        public virtual async Task UpdateAsync(T entity)
        {
            var existingEntity = await dbSet.FindAsync(new object[] { entity.Id });
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

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await dbSet.FindAsync(new object[] { id });
            if (entity != null)
            {
                dbSet.Remove(entity);
                // Không gọi SaveChangesAsync ở đây, để UnitOfWork xử lý
            }
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await Query.Where(predicate).ToListAsync();
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await dbSet.AnyAsync(predicate, cancellationToken);
        }
    }
}