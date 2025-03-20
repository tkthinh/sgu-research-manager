using Application.WorkLevels;
using Domain.Interfaces;

namespace Application.WorkTypes
{
    public interface IWorkTypeService : IGenericService<WorkTypeDto>
    {
      Task<IEnumerable<WorkTypeWithDetailsCountDto>> GetWorkTypesWithDetailsCountAsync(CancellationToken cancellationToken = default);
    }
}
