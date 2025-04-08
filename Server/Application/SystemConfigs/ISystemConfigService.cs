using Domain.Interfaces;

namespace Application.SystemConfigs
{
    public interface ISystemConfigService : IGenericService<SystemConfigDto>
    {
        Task<IEnumerable<SystemConfigDto>> GetSystemConfigsOfYear(Guid academicYearId);
        Task<bool> IsSystemOpenAsync(DateTime date, CancellationToken cancellationToken = default);
    }
}
