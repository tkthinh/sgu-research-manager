using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.Purposes
{
   public class PurposeService : GenericCachedService<PurposeDto, Purpose>, IPurposeService
   {
        protected override TimeSpan defaultCacheTime => TimeSpan.FromHours(24);

        public PurposeService(
          IUnitOfWork unitOfWork,
          IGenericMapper<PurposeDto, Purpose> mapper,
          IDistributedCache cache,
          ILogger<PurposeService> logger
          )
          : base(unitOfWork, mapper, cache, logger)
      {

      }

      public async Task<IEnumerable<PurposeDto>> GetPurposesByWorkTypeIdAsync(Guid workTypeId)
      {
          if (workTypeId == Guid.Empty)
          {
              throw new ArgumentException("WorkTypeId không hợp lệ", nameof(workTypeId));
          }

          // Lấy danh sách purpose theo workTypeId và include WorkType
          var repo = unitOfWork.Repository<Purpose>();
          
          // Nếu repository hỗ trợ eager loading, sử dụng includeProperties
          Expression<Func<Purpose, bool>> filter = p => p.WorkTypeId == workTypeId;
          var purposes = await repo.FindAsync(filter);

          // Nếu không có kết quả
          if (purposes == null || !purposes.Any())
          {
              return Enumerable.Empty<PurposeDto>();
          }

          // Load WorkType cho mỗi purpose nếu cần
          foreach (var purpose in purposes)
          {
              // Load WorkType manually if not already loaded
              if (purpose.WorkType == null)
              {
                  purpose.WorkType = await unitOfWork.Repository<WorkType>().GetByIdAsync(purpose.WorkTypeId);
              }
          }

          return mapper.MapToDtos(purposes);
      }

      // Override để load WorkType khi lấy tất cả
      public override async Task<IEnumerable<PurposeDto>> GetAllAsync(CancellationToken cancellationToken = default)
      {
          // Lấy tất cả purposes từ base class
          var purposes = await base.GetAllAsync(cancellationToken);
          
          // Nếu đã có purposes, load thêm WorkTypes cho từng purpose
          if (purposes != null && purposes.Any())
          {
              // Lấy tất cả workTypes một lần để tránh nhiều lần truy vấn
              var workTypes = await unitOfWork.Repository<WorkType>().GetAllAsync();
              var workTypeDict = workTypes.ToDictionary(wt => wt.Id);
              
              // Lấy lại danh sách entities từ repository
              var purposeEntities = await unitOfWork.Repository<Purpose>().GetAllAsync();
              
              // Gán WorkType cho từng Purpose entity
              foreach (var purpose in purposeEntities)
              {
                  if (purpose.WorkType == null && workTypeDict.TryGetValue(purpose.WorkTypeId, out var workType))
                  {
                      purpose.WorkType = workType;
                  }
              }
              
              // Ánh xạ lại từ entities sang DTOs
              return mapper.MapToDtos(purposeEntities);
          }
          
          return purposes;
      }
   }
}
