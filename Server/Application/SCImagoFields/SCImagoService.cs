using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.SCImagoFields
{
    public class SCImagoFieldService : GenericCachedService<SCImagoFieldDto, SCImagoField>, ISCImagoFieldService
    {
        public SCImagoFieldService(
            IUnitOfWork unitOfWork,
            IGenericMapper<SCImagoFieldDto, SCImagoField> mapper,
            IDistributedCache cache,
            ILogger<SCImagoFieldService> logger
            )
            : base(unitOfWork, mapper, cache, logger)
        {
        }

        public async Task<IEnumerable<SCImagoFieldDto>> GetSCImagoFieldsByWorkTypeIdAsync(Guid workTypeId)
        {
            if (workTypeId == Guid.Empty)
            {
                throw new ArgumentException("WorkTypeId không hợp lệ", nameof(workTypeId));
            }

            // Lấy danh sách SCImago field theo workTypeId và include WorkType
            var repo = unitOfWork.Repository<SCImagoField>();
            
            // Nếu repository hỗ trợ eager loading, sử dụng includeProperties
            Expression<Func<SCImagoField, bool>> filter = sf => sf.WorkTypeId == workTypeId;
            var scImagoFields = await repo.FindAsync(filter);

            // Nếu không có kết quả
            if (scImagoFields == null || !scImagoFields.Any())
            {
                return Enumerable.Empty<SCImagoFieldDto>();
            }

            // Load WorkType cho mỗi SCImago field nếu cần
            foreach (var field in scImagoFields)
            {
                // Load WorkType manually if not already loaded
                if (field.WorkType == null)
                {
                    field.WorkType = await unitOfWork.Repository<WorkType>().GetByIdAsync(field.WorkTypeId);
                }
            }

            return mapper.MapToDtos(scImagoFields);
        }

        // Override để load WorkType khi lấy tất cả
        public override async Task<IEnumerable<SCImagoFieldDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            // Lấy tất cả SCImago fields từ base class
            var scImagoFields = await base.GetAllAsync(cancellationToken);
            
            // Nếu đã có SCImago fields, load thêm WorkTypes cho từng field
            if (scImagoFields != null && scImagoFields.Any())
            {
                // Lấy tất cả workTypes một lần để tránh nhiều lần truy vấn
                var workTypes = await unitOfWork.Repository<WorkType>().GetAllAsync();
                var workTypeDict = workTypes.ToDictionary(wt => wt.Id);
                
                // Lấy lại danh sách entities từ repository
                var scImagoFieldEntities = await unitOfWork.Repository<SCImagoField>().GetAllAsync();
                
                // Gán WorkType cho từng SCImagoField entity
                foreach (var field in scImagoFieldEntities)
                {
                    if (field.WorkType == null && workTypeDict.TryGetValue(field.WorkTypeId, out var workType))
                    {
                        field.WorkType = workType;
                    }
                }
                
                // Ánh xạ lại từ entities sang DTOs
                return mapper.MapToDtos(scImagoFieldEntities);
            }
            
            return scImagoFields;
        }
    }
}
