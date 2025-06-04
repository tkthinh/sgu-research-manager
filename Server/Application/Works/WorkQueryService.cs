using Application.Authors;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Application.Works
{
    public class WorkQueryService : GenericCachedService<WorkDto, Work>, IWorkQueryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper<WorkDto, Work> _mapper;
        private readonly IGenericMapper<AuthorDto, Author> _authorMapper;
        private readonly IWorkRepository _workRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<WorkQueryService> _logger;

        public WorkQueryService(
            IUnitOfWork unitOfWork,
            IGenericMapper<WorkDto, Work> mapper,
            IGenericMapper<AuthorDto, Author> authorMapper,
            IWorkRepository workRepository,
            ICurrentUserService currentUserService,
            ILogger<WorkQueryService> logger,
            IDistributedCache cache)
            : base(unitOfWork, mapper, cache, logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authorMapper = authorMapper;
            _workRepository = workRepository;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<WorkDto?> GetWorkByIdWithAuthorsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (isCacheAvailable)
            {
                string cacheKey = $"{cacheKeyPrefix}_with_authors_{id}";
                string? cachedData = null;

                try
                {
                    cachedData = await cache.GetStringAsync(cacheKey, cancellationToken);
                }
                catch (Exception ex)
                {
                    HandleCacheException(ex, $"Error reading cache for key {cacheKey}");
                }

                if (!string.IsNullOrEmpty(cachedData))
                {
                    try
                    {
                        return JsonSerializer.Deserialize<WorkDto>(cachedData);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error deserializing cached data for key {CacheKey}", cacheKey);
                    }
                }
            }

            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(id);
            if (work is null)
            {
                return null;
            }

            var dto = mapper.MapToDto(work);
            await FillCoAuthorUserIdsAsync(dto, cancellationToken);

            if (isCacheAvailable)
            {
                await SafeSetCacheAsync(
                    $"{cacheKeyPrefix}_with_authors_{id}",
                    JsonSerializer.Serialize(dto),
                    defaultCacheTime,
                    cancellationToken);
            }

            return dto;
        }

        public async Task<IEnumerable<WorkDto>> GetWorksAsync(WorkFilter filter, CancellationToken cancellationToken = default)
        {
            try
            {
                // Tự động lấy userId hiện tại nếu IsCurrentUser = true và không có UserId
                if (filter.IsCurrentUser && filter.UserId == null)
                {
                    var (isSuccess, userId, _) = _currentUserService.GetCurrentUser();
                    if (isSuccess)
                    {
                        filter.UserId = userId;
                    }
                }
                
                // Xác định ngày hiện tại cho việc kiểm tra deadline nếu cần
                var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
                    
                // Sử dụng predicate builder để xây dựng truy vấn
                Expression<Func<Work, bool>> predicate = BuildWorkPredicate(filter);
                
                // Thực hiện một truy vấn duy nhất với tham số asNoTracking=true để tăng hiệu suất
                var works = await _workRepository.GetWorksByFilterAsync(predicate, cancellationToken);
                var worksList = works.ToList();
                
                // Xử lý kết quả sau khi truy vấn
                var workDtos = ProcessWorksResult(worksList, filter);
                
                // Điền thông tin đồng tác giả cho các công trình
                await FillCoAuthorUserIdsEfficientlyAsync(workDtos, cancellationToken);
                
                return workDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        /// Xây dựng predicate từ WorkFilter để tạo truy vấn
        private Expression<Func<Work, bool>> BuildWorkPredicate(WorkFilter filter)
        {
            // Bắt đầu với predicate true (lấy tất cả)
            Expression<Func<Work, bool>> predicate = w => true;
            
            // Lọc theo AcademicYearId nếu có
            if (filter.AcademicYearId.HasValue)
            {
                var academicYearId = filter.AcademicYearId.Value;
                predicate = CombinePredicates(predicate, w => w.AcademicYearId == academicYearId);
            }
            
            // Lọc theo Source nếu có
            if (filter.Source.HasValue)
            {
                var source = filter.Source.Value;
                predicate = CombinePredicates(predicate, w => w.Source == source);
            }
            
            // Lọc theo UserId nếu có
            if (filter.UserId.HasValue)
            {
                var userId = filter.UserId.Value;
                
                if (filter.OnlyRegisteredWorks)
                {
                    // Chỉ lấy công trình đã đăng ký quy đổi
                    predicate = CombinePredicates(predicate, 
                        w => w.Authors != null && w.Authors.Any(a => a.UserId == userId && 
                                                               a.AuthorRegistration != null && 
                                                               (filter.AcademicYearId == null || 
                                                                a.AuthorRegistration.AcademicYearId == filter.AcademicYearId)));
                }
                else if (filter.OnlyRegisterableWorks)
                {
                    // Xác định ngày hiện tại
                    var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
                    
                    // Chỉ lấy công trình mà người dùng có thể đăng ký quy đổi
                    predicate = CombinePredicates(predicate,
                        w => w.Authors != null && 
                             w.Authors.Any(a => a.UserId == userId && 
                                           a.AuthorRegistration == null) &&
                             w.ExchangeDeadline.HasValue && 
                             w.ExchangeDeadline.Value >= currentDate);
                }
                else
                {
                    // Lấy tất cả công trình của user (là tác giả hoặc đồng tác giả)
                    predicate = CombinePredicates(predicate, 
                        w => (w.Authors != null && w.Authors.Any(a => a.UserId == userId)) || 
                             (w.WorkAuthors != null && w.WorkAuthors.Any(wa => wa.UserId == userId)));
                }
                
                // Lọc thêm theo ProofStatus nếu có
                if (filter.ProofStatus.HasValue)
                {
                    var proofStatus = filter.ProofStatus.Value;
                    predicate = CombinePredicates(predicate, 
                        w => w.Authors != null && w.Authors.Any(a => a.UserId == userId && a.ProofStatus == proofStatus));
                }
            }
            // Lọc theo DepartmentId nếu có
            else if (filter.DepartmentId.HasValue)
            {
                var departmentId = filter.DepartmentId.Value;
                predicate = CombinePredicates(predicate, 
                    w => w.Authors != null && w.Authors.Any(a => a.User != null && a.User.DepartmentId == departmentId));
            }
            
            return predicate;
        }
        
        /// Kết hợp hai predicate với nhau sử dụng phép AND
        private Expression<Func<T, bool>> CombinePredicates<T>(
            Expression<Func<T, bool>> expr1, 
            Expression<Func<T, bool>> expr2)
        {
            // Tạo một param mới để thay thế param trong expr2
            var param = Expression.Parameter(typeof(T), "x");
            
            // Thay thế param trong expr2 bằng param mới
            var visitor = new ParameterReplacer(expr2.Parameters[0], param);
            var body2 = visitor.Visit(expr2.Body);
            
            // Thay thế param trong expr1 bằng param mới
            visitor = new ParameterReplacer(expr1.Parameters[0], param);
            var body1 = visitor.Visit(expr1.Body);
            
            // Kết hợp body1 và body2 bằng phép AND
            var combinedBody = Expression.AndAlso(body1, body2);
            
            // Tạo và trả về lambda expression mới
            return Expression.Lambda<Func<T, bool>>(combinedBody, param);
        }
        
        /// Lớp để thay thế tham số trong expression
        private class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParam;
            private readonly ParameterExpression _newParam;
            
            public ParameterReplacer(ParameterExpression oldParam, ParameterExpression newParam)
            {
                _oldParam = oldParam;
                _newParam = newParam;
            }
            
            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _oldParam ? _newParam : base.VisitParameter(node);
            }
        }
        
        /// Xử lý kết quả truy vấn và áp dụng các bộ lọc bổ sung
        private IEnumerable<WorkDto> ProcessWorksResult(List<Work> works, WorkFilter filter)
        {
            // Chuyển đổi thành DTO
            var workDtos = _mapper.MapToDtos(works).ToList();
            
            // Xử lý trường hợp đặc biệt nếu cần
            if (filter.UserId.HasValue)
            {
                // Tạo bản đồ để lưu trữ CoAuthorUserIds
                var coAuthorUserIdsMap = workDtos.ToDictionary(
                    w => w.Id, 
                    w => new List<Guid>(w.CoAuthorUserIds ?? new List<Guid>())
                );
                
                // Xử lý khi cần lọc các tác giả
                foreach (var workDto in workDtos)
                {
                    // Tìm công trình tương ứng
                    var work = works.FirstOrDefault(w => w.Id == workDto.Id);
                    if (work == null) continue;
                    
                    // Trả về tất cả thông tin tác giả
                    if (work.Authors != null)
                    {
                        workDto.Authors = work.Authors.Select(a => _authorMapper.MapToDto(a)).ToList();
                    }
                }
                
                // Khôi phục CoAuthorUserIds từ map đã lưu trữ
                foreach (var workDto in workDtos)
                {
                    if (coAuthorUserIdsMap.TryGetValue(workDto.Id, out var coAuthorIds))
                    {
                        workDto.CoAuthorUserIds = coAuthorIds;
                    }
                }
            }
            
            return workDtos;
        }

        private async Task FillCoAuthorUserIdsAsync(WorkDto workDto, CancellationToken cancellationToken = default)
        {
            if (workDto is null)
                return;

            // Lấy userId từ service
            var (isSuccess, currentUserId, _) = _currentUserService.GetCurrentUser();
            
            try {
                // Thực hiện truy vấn hiệu quả
                var authorUserIds = await unitOfWork.Repository<Author>()
                    .FindAsync(a => a.WorkId == workDto.Id)
                    .ContinueWith(t => t.Result.Select(a => a.UserId).ToList(), cancellationToken);

                var coAuthorUserIds = await unitOfWork.Repository<WorkAuthor>()
                    .FindAsync(wa => wa.WorkId == workDto.Id)
                    .ContinueWith(t => t.Result.Select(wa => wa.UserId).ToList(), cancellationToken);

                // Kết hợp và loại bỏ trùng lặp
                var combinedUserIds = authorUserIds.Union(coAuthorUserIds);
                if (isSuccess)
                {
                    combinedUserIds = combinedUserIds.Where(uid => uid != currentUserId);
                }
                workDto.CoAuthorUserIds = combinedUserIds.Distinct().ToList();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error filling CoAuthorUserIds for WorkDto {WorkId}", workDto.Id);
                workDto.CoAuthorUserIds = new List<Guid>();
            }
        }

        /// Phiên bản tối ưu hơn của FillCoAuthorUserIdsForMultipleWorks
        private async Task FillCoAuthorUserIdsEfficientlyAsync(IEnumerable<WorkDto> workDtos, CancellationToken cancellationToken = default)
        {
            if (workDtos is null || !workDtos.Any())
                return;

            var (isSuccess, currentUserId, _) = _currentUserService.GetCurrentUser();
            var workIds = workDtos.Select(w => w.Id).Distinct().ToList();

            try
            {
                // Thực hiện truy vấn hiệu quả
                var allAuthors = await _unitOfWork.Repository<Author>()
                    .FindAsync(a => workIds.Contains(a.WorkId));

                var allWorkAuthors = await _unitOfWork.Repository<WorkAuthor>()
                    .FindAsync(wa => workIds.Contains(wa.WorkId));

                // Nhóm dữ liệu theo WorkId
                var authorsByWorkId = allAuthors
                    .GroupBy(a => a.WorkId)
                    .ToDictionary(g => g.Key, g => g.Select(a => a.UserId).ToList());

                var workAuthorsByWorkId = allWorkAuthors
                    .GroupBy(wa => wa.WorkId)
                    .ToDictionary(g => g.Key, g => g.Select(wa => wa.UserId).ToList());

                // Điền dữ liệu cho từng WorkDto một cách hiệu quả
                foreach (var workDto in workDtos)
                {
                    if (workDto == null) continue;
                    
                    // Lấy danh sách userIds
                    var authorUserIds = authorsByWorkId.TryGetValue(workDto.Id, out var authors) 
                        ? authors : new List<Guid>();

                    var coAuthorUserIds = workAuthorsByWorkId.TryGetValue(workDto.Id, out var workAuthors) 
                        ? workAuthors : new List<Guid>();

                    // Kết hợp và xử lý
                    var combinedUserIds = authorUserIds.Union(coAuthorUserIds);
                    if (isSuccess)
                    {
                        combinedUserIds = combinedUserIds.Where(uid => uid != currentUserId);
                    }
                    
                    workDto.CoAuthorUserIds = combinedUserIds.Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi điền CoAuthorUserIds cho nhiều WorkDto");
                
                // Đảm bảo không có null
                foreach (var workDto in workDtos.Where(w => w != null && w.CoAuthorUserIds == null))
                {
                    workDto.CoAuthorUserIds = new List<Guid>();
                }
            }
        }
    }
} 