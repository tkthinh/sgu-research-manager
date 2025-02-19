using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.UnitOfWork
{
   public class UnitOfWork : IUnitOfWork
   {
      private readonly ApplicationDbContext context;
      private readonly Dictionary<Type, object> repositories = new();

      public UnitOfWork(ApplicationDbContext context)
      {
         this.context = context;
      }

      public IGenericRepository<T> Repository<T>() where T : BaseEntity
      {
         if (!repositories.ContainsKey(typeof(T)))
            repositories[typeof(T)] = new GenericRepository<T>(context);

         return (IGenericRepository<T>)repositories[typeof(T)];
      }

      public Task<int> SaveChangesAsync()
      {
         return context.SaveChangesAsync();
      }

      public void Dispose()
      {
         context.Dispose();
      }
   }
}
