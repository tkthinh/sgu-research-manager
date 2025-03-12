using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.SystemConfigs
{
    public class SystemConfigService : ISystemConfigService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper<SystemConfigDto, SystemConfig> _mapper;
        private readonly ILogger<SystemConfigService> _logger;

        public SystemConfigService(
            IUnitOfWork unitOfWork,
            IGenericMapper<SystemConfigDto, SystemConfig> mapper,
            ILogger<SystemConfigService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SystemConfigDto> GetSystemConfigAsync(CancellationToken cancellationToken = default)
        {
            var configs = await _unitOfWork.Repository<SystemConfig>().FindAsync(c => true);
            var config = configs.FirstOrDefault();

            if (config == null)
            {
                _logger.LogWarning("No SystemConfig found. Admin needs to configure the system.");
                throw new Exception("System configuration is not set. Please contact admin to configure.");
            }

            return _mapper.MapToDto(config);
        }

        public async Task UpdateSystemConfigAsync(UpdateSystemConfigRequestDto config, CancellationToken cancellationToken = default)
        {
            var configs = await _unitOfWork.Repository<SystemConfig>().FindAsync(c => true);
            var existingConfig = configs.FirstOrDefault();

            if (existingConfig == null)
            {
                _logger.LogWarning("No SystemConfig found for update. Creating a new one.");
                throw new Exception("No existing configuration found. Please create a configuration first.");
            }

            // Cập nhật các trường từ DTO
            existingConfig.StartDate = config.StartDate;
            existingConfig.EndDate = config.EndDate;
            existingConfig.IsClosed = config.IsClosed;
            existingConfig.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.Repository<SystemConfig>().UpdateAsync(existingConfig);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("System configuration updated: StartDate={StartDate}, EndDate={EndDate}, IsClosed={IsClosed}",
                existingConfig.StartDate, existingConfig.EndDate, existingConfig.IsClosed);
        }

        public async Task CreateSystemConfigAsync(CreateSystemConfigRequestDto config, CancellationToken cancellationToken = default)
        {
            var configs = await _unitOfWork.Repository<SystemConfig>().FindAsync(c => true);
            if (configs.Any())
            {
                _logger.LogWarning("SystemConfig already exists. Use update instead.");
                throw new Exception("System configuration already exists. Please use update instead.");
            }

            var newConfig = new SystemConfig
            {
                Id = Guid.NewGuid(),
                StartDate = config.StartDate,
                EndDate = config.EndDate,
                IsClosed = config.IsClosed,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Repository<SystemConfig>().CreateAsync(newConfig);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("System configuration created: StartDate={StartDate}, EndDate={EndDate}, IsClosed={IsClosed}",
                newConfig.StartDate, newConfig.EndDate, newConfig.IsClosed);
        }

        public async Task<bool> IsSystemOpenAsync(CancellationToken cancellationToken = default)
        {
            var configs = await _unitOfWork.Repository<SystemConfig>().FindAsync(c => true);
            var config = configs.FirstOrDefault();

            if (config == null)
            {
                _logger.LogWarning("No SystemConfig found. Assuming system is closed.");
                return false;
            }

            var now = DateTime.UtcNow;
            return now >= config.StartDate && now <= config.EndDate && !config.IsClosed;
        }
    }
}