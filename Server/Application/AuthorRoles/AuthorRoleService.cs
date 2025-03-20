using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.AuthorRoles
{
    public class AuthorRoleService : GenericCachedService<AuthorRoleDto, AuthorRole>, IAuthorRoleService
    {
        public AuthorRoleService(
            IUnitOfWork unitOfWork,
            IGenericMapper<AuthorRoleDto, AuthorRole> mapper,
            IDistributedCache cache,
            ILogger<AuthorRoleService> logger
            )
            : base(unitOfWork, mapper, cache, logger)
        {
        }

        public async Task<IEnumerable<AuthorRoleDto>> GetAuthorRolesByWorkTypeIdAsync(Guid workTypeId)
        {
            if (workTypeId == Guid.Empty)
            {
                throw new ArgumentException("WorkTypeId không hợp lệ", nameof(workTypeId));
            }

            // Lấy danh sách authorRole theo workTypeId và include WorkType
            var repo = unitOfWork.Repository<AuthorRole>();

            // Nếu repository hỗ trợ eager loading, sử dụng includeProperties
            Expression<Func<AuthorRole, bool>> filter = p => p.WorkTypeId == workTypeId;
            var authorRoles = await repo.FindAsync(filter);

            // Nếu không có kết quả
            if (authorRoles == null || !authorRoles.Any())
            {
                return Enumerable.Empty<AuthorRoleDto>();
            }

            // Load WorkType cho mỗi authorRole nếu cần
            foreach (var authorRole in authorRoles)
            {
                // Load WorkType manually if not already loaded
                if (authorRole.WorkType == null)
                {
                    authorRole.WorkType = await unitOfWork.Repository<WorkType>().GetByIdAsync(authorRole.WorkTypeId);
                }
            }

            return mapper.MapToDtos(authorRoles);
        }

        // Override để load WorkType khi lấy tất cả
        public override async Task<IEnumerable<AuthorRoleDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            // Lấy tất cả AuthorRole từ base class
            var authorRoles = await base.GetAllAsync(cancellationToken);
            
            // Nếu đã có AuthorRole, load thêm WorkTypes cho từng role
            if (authorRoles != null && authorRoles.Any())
            {
                // Lấy tất cả workTypes một lần để tránh nhiều lần truy vấn
                var workTypes = await unitOfWork.Repository<WorkType>().GetAllAsync();
                var workTypeDict = workTypes.ToDictionary(wt => wt.Id);
                
                // Lấy lại danh sách entities từ repository
                var authorRoleEntities = await unitOfWork.Repository<AuthorRole>().GetAllAsync();
                
                // Gán WorkType cho từng AuthorRole entity
                foreach (var role in authorRoleEntities)
                {
                    if (role.WorkType == null && workTypeDict.TryGetValue(role.WorkTypeId, out var workType))
                    {
                        role.WorkType = workType;
                    }
                }
                
                // Ánh xạ lại từ entities sang DTOs
                return mapper.MapToDtos(authorRoleEntities);
            }
            
            return authorRoles;
        }
    }
}
