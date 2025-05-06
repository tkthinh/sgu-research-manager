using Application.Shared.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.Factors
{
    public class FactorService : GenericCachedService<FactorDto, Factor>, IFactorService
    {
        protected override TimeSpan defaultCacheTime => TimeSpan.FromHours(24);

        public FactorService(
            IUnitOfWork unitOfWork,
            IGenericMapper<FactorDto, Factor> mapper,
            IDistributedCache cache,
            ILogger<FactorService> logger
        ) : base(unitOfWork, mapper, cache, logger)
        {
        }

        public async Task<IEnumerable<FactorDto>> GetFactorsByWorkTypeIdAsync(Guid workTypeId, CancellationToken cancellationToken = default)
        {
            if (workTypeId == Guid.Empty)
            {
                throw new ArgumentException("WorkTypeId không hợp lệ", nameof(workTypeId));
            }

            Expression<Func<Factor, bool>> filter = f => f.WorkTypeId == workTypeId;
            
            // Lấy tất cả factors theo workTypeId
            var factors = await unitOfWork.Repository<Factor>().FindAsync(filter);
            
            if (factors == null || !factors.Any())
            {
                return Enumerable.Empty<FactorDto>();
            }

            // Load các đối tượng liên quan
            await LoadRelatedEntitiesAsync(factors, cancellationToken);
            
            return mapper.MapToDtos(factors);
        }

        // Override để load các đối tượng liên quan khi lấy tất cả
        public override async Task<IEnumerable<FactorDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            // Lấy tất cả factors từ base class
            var factors = await base.GetAllAsync(cancellationToken);
            
            // Nếu đã có factors, load thêm các đối tượng liên quan
            if (factors != null && factors.Any())
            {
                // Lấy lại danh sách entities từ repository
                var factorEntities = await unitOfWork.Repository<Factor>().GetAllAsync();
                
                // Load các đối tượng liên quan
                await LoadRelatedEntitiesAsync(factorEntities, cancellationToken);
                
                // Ánh xạ lại từ entities sang DTOs
                return mapper.MapToDtos(factorEntities);
            }
            
            return factors;
        }

        private async Task LoadRelatedEntitiesAsync(IEnumerable<Factor> factors, CancellationToken cancellationToken = default)
        {
            // Lấy tất cả workTypes, workLevels, purposes, authorRoles một lần để tránh nhiều lần truy vấn
            var workTypes = await unitOfWork.Repository<WorkType>().GetAllAsync();
            var workLevels = await unitOfWork.Repository<WorkLevel>().GetAllAsync();
            var purposes = await unitOfWork.Repository<Purpose>().GetAllAsync();
            var authorRoles = await unitOfWork.Repository<AuthorRole>().GetAllAsync();
            
            var workTypeDict = workTypes.ToDictionary(wt => wt.Id);
            var workLevelDict = workLevels.ToDictionary(wl => wl.Id);
            var purposeDict = purposes.ToDictionary(p => p.Id);
            var authorRoleDict = authorRoles.ToDictionary(ar => ar.Id);
            
            // Gán các đối tượng liên quan cho từng Factor entity
            foreach (var factor in factors)
            {
                if (factor.WorkType == null && workTypeDict.TryGetValue(factor.WorkTypeId, out var workType))
                {
                    factor.WorkType = workType;
                }
                
                if (factor.WorkLevelId.HasValue && factor.WorkLevel == null && 
                    workLevelDict.TryGetValue(factor.WorkLevelId.Value, out var workLevel))
                {
                    factor.WorkLevel = workLevel;
                }
                
                if (factor.Purpose == null && purposeDict.TryGetValue(factor.PurposeId, out var purpose))
                {
                    factor.Purpose = purpose;
                }
                
                if (factor.AuthorRoleId.HasValue && factor.AuthorRole == null && 
                    authorRoleDict.TryGetValue(factor.AuthorRoleId.Value, out var authorRole))
                {
                    factor.AuthorRole = authorRole;
                }
            }
        }
    }
}
