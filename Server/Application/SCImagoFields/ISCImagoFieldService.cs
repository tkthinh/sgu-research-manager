using Domain.Interfaces;

namespace Application.SCImagoFields
{
    public interface ISCImagoFieldService : IGenericService<SCImagoFieldDto>
    {
        Task<IEnumerable<SCImagoFieldDto>> GetSCImagoFieldsByWorkTypeIdAsync(Guid workTypeId);
    }
}
