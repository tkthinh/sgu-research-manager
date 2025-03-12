namespace Application.SystemConfigs
{
    public interface ISystemConfigService
    {
        Task<SystemConfigDto> GetSystemConfigAsync(CancellationToken cancellationToken = default);
        Task UpdateSystemConfigAsync(UpdateSystemConfigRequestDto config, CancellationToken cancellationToken = default);
        Task CreateSystemConfigAsync(CreateSystemConfigRequestDto config, CancellationToken cancellationToken = default);
        Task<bool> IsSystemOpenAsync(CancellationToken cancellationToken = default);
    }
}
