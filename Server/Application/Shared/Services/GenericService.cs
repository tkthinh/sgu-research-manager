using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Shared.Services
{
   public abstract class GenericService<TDto, T> : IGenericService<TDto> where T : BaseEntity
   {
      protected readonly IUnitOfWork unitOfWork;
      protected readonly IGenericMapper<TDto, T> mapper;
      protected readonly ILogger logger;

      public GenericService(
         IUnitOfWork unitOfWork,
         IGenericMapper<TDto, T> mapper,
         ILogger logger
         )
      {
         this.unitOfWork = unitOfWork;
         this.mapper = mapper;
         this.logger = logger;
      }

      public virtual async Task<TDto> CreateAsync(TDto dto, CancellationToken cancellationToken = default)
      {
         var entity = mapper.MapToEntity(dto);
         entity.CreatedDate = DateTime.UtcNow;

         await unitOfWork.Repository<T>().CreateAsync(entity);
         await unitOfWork.SaveChangesAsync();

         return mapper.MapToDto(entity);
      }

      public virtual async Task<IEnumerable<TDto>> GetAllAsync(CancellationToken cancellationToken = default)
      {
         var entities = await unitOfWork.Repository<T>().GetAllAsync();

         return mapper.MapToDtos(entities);
      }

      public virtual async Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
      {
         var entity = await unitOfWork.Repository<T>().GetByIdAsync(id);

         return entity != null ? mapper.MapToDto(entity) : default;
      }

      public virtual async Task UpdateAsync(TDto dto, CancellationToken cancellationToken = default)
      {
         var entity = mapper.MapToEntity(dto);
         entity.ModifiedDate = DateTime.UtcNow;

         await unitOfWork.Repository<T>().UpdateAsync(entity);
         await unitOfWork.SaveChangesAsync();
      }

      public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
      {
         await unitOfWork.Repository<T>().DeleteAsync(id);
         await unitOfWork.SaveChangesAsync();
      }
   }
}
