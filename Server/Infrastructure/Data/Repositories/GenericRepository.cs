using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
   public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
   {
      private readonly ApplicationDbContext context;
      private readonly DbSet<T> dbSet;

      public GenericRepository(ApplicationDbContext context)
      {
         this.context = context;
         dbSet = context.Set<T>();
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
         return await dbSet.FindAsync(id);
      }

      public virtual async Task UpdateAsync(T entity)
      {
         var existingEntity = await dbSet.FindAsync(entity.Id);
         if (existingEntity is not null)
         {
            context.Entry(existingEntity).CurrentValues.SetValues(entity);
         }
         else
         {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
         }

         entity.ModifiedDate = DateTime.UtcNow;
         await context.SaveChangesAsync();
      }

      public virtual async Task DeleteAsync(Guid id)
      {
         var entity = await dbSet.FindAsync(id);
         if (entity is not null)
         {
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
         }
      }

   }
}
