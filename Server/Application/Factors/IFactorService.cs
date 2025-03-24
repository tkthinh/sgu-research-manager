using Domain.Enums;
using Domain.Interfaces;

namespace Application.Factors
{
    public interface IFactorService : IGenericService<FactorDto>
    {
        Task<IEnumerable<FactorDto>> GetFactorsByWorkTypeIdAsync(Guid workTypeId, CancellationToken cancellationToken = default);
    }
}
