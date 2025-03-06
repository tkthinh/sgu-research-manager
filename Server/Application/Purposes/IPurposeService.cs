using Application.WorkLevels;
using Domain.Interfaces;

namespace Application.Purposes
{
   public interface IPurposeService : IGenericService<PurposeDto>
   {
        Task<IEnumerable<PurposeDto>> GetPurposesByWorkTypeIdAsync(Guid workTypeId);
    }
}
