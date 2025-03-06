using Application.WorkLevels;
using Domain.Interfaces;

namespace Application.WorkTypes
{
    public interface IWorkTypeService : IGenericService<WorkTypeDto>
    {
      Task<IEnumerable<WorkTypeWithLevelCountDto>> GetWorkTypesWithCountAsync(CancellationToken cancellationToken = default);
    }
}
