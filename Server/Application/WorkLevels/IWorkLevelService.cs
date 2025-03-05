using Domain.Interfaces;

namespace Application.WorkLevels
{
    public interface IWorkLevelService : IGenericService<WorkLevelDto>
    {
        Task<IEnumerable<WorkLevelDto>> GetWorkLevelsByWorkTypeIdAsync(Guid workTypeId);
    }
}
