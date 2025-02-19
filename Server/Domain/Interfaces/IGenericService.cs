namespace Domain.Interfaces
{
   public interface IGenericService<TDto>
   {
      Task<IEnumerable<TDto>> GetAllAsync(CancellationToken cancellationToken = default);
      Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
      Task<TDto> CreateAsync(TDto dto, CancellationToken cancellationToken = default);
      Task UpdateAsync(TDto dto, CancellationToken cancellationToken = default);
      Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
   }
}
