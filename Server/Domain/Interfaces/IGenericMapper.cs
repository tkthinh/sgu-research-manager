namespace Domain.Interfaces
{
   public interface IGenericMapper<TDto, T>
   {
      TDto MapToDto(T entity);
      T MapToEntity(TDto dto);
      IEnumerable<TDto> MapToDtos(IEnumerable<T> entities);
      IEnumerable<T> MapToEntities(IEnumerable<TDto> dtos);
   }

}
