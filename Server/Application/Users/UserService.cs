using Application.Shared.Services;
using Domain.Interfaces;
using Domain.Enums;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Application.Works;
using System.Text.Json;

namespace Application.Users
{
    public class UserService : GenericCachedService<UserDto, User>, IUserService
    {
        private readonly IUserRepository repository;

        public UserService(
            IUnitOfWork unitOfWork,
            IGenericMapper<UserDto, User> mapper,
            IUserRepository repository,
            IDistributedCache cache,
            ILogger<UserService> logger
            )
            : base(unitOfWork, mapper, cache, logger)
        {
            this.repository = repository;
        }

        public override async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (isCacheAvailable)
            {
                string cacheKey = $"{cacheKeyPrefix}_{id}";
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
                        return JsonSerializer.Deserialize<UserDto>(cachedData)!;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error deserializing cached data for key {CacheKey}", cacheKey);
                    }
                }
            }

            // If cache is unavailable or empty, get from database
            var user = await repository.GetUserByIdWithDetailsAsync(id);

            if (user is not null)
            {
                var dto = mapper.MapToDto(user);

                // Only try to cache if it was available
                if (isCacheAvailable)
                {
                    await SafeSetCacheAsync(
                        $"{cacheKeyPrefix}_user_{id}",
                        JsonSerializer.Serialize(dto),
                        TimeSpan.FromMinutes(30),
                        cancellationToken);
                }

                return dto;
            }

            return null;
        }

        public override async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            if (isCacheAvailable)
            {
                string cacheKey = $"{cacheKeyPrefix}_all";
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
                        return JsonSerializer.Deserialize<IEnumerable<UserDto>>(cachedData)!;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error deserializing cached data for key {CacheKey}", cacheKey);
                    }
                }
            }

            // If cache is unavailable or empty, get from database
            var users = await repository.GetUsersWithDetailsAsync();
            var dtos = mapper.MapToDtos(users);

            if (isCacheAvailable)
            {
                await SafeSetCacheAsync(
                    $"{cacheKeyPrefix}_user_all",
                    JsonSerializer.Serialize(dtos),
                    TimeSpan.FromMinutes(30),
                    cancellationToken);
            }
            return dtos;
        }

        public async Task<UserDto?> GetUserByIdentityIdAsync(string identityId)
        {
            var user = await repository.GetUserByIdentityIdAsync(identityId);

            if (user is not null)
            {
                return mapper.MapToDto(user);
            }

            return null;
        }

        public async Task<UserConversionResultRequestDto> GetUserConversionResultAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("UserId không hợp lệ", nameof(userId));
            }

            // Lấy danh sách Purpose từ cơ sở dữ liệu
            var purposes = await unitOfWork.Repository<Purpose>().GetAllAsync();
            var dutyPurposeIds = purposes
                .Where(p => p.Name == "Quy đổi giờ nghĩa vụ")
                .Select(p => p.Id)
                .ToList();
            var overLimitPurposeIds = purposes
                .Where(p => p.Name == "Quy đổi vượt định mức")
                .Select(p => p.Id)
                .ToList();
            var researchPurposeIds = purposes
                .Where(p => p.Name == "Sản phẩm của đề tài NCKH")
                .Select(p => p.Id)
                .ToList();

            // Lấy danh sách Author của user với ProofStatus = HopLe
            var authors = await unitOfWork.Repository<Author>()
                .FindAsync(a => a.UserId == userId && a.ProofStatus == ProofStatus.HopLe);

            if (authors == null || !authors.Any())
            {
                return new UserConversionResultRequestDto
                {
                    UserId = userId,
                    UserName = await GetUserNameAsync(userId),
                    ConversionResults = new ConversionDetailsRequestDto
                    {
                        DutyHourConversion = new ConversionItemRequestDto { TotalWorks = 0, TotalConvertedHours = 0m, TotalCalculatedHours = 0m },
                        OverLimitConversion = new ConversionItemRequestDto { TotalWorks = 0, TotalConvertedHours = 0m, TotalCalculatedHours = 0m },
                        ResearchProductConversion = new ConversionItemRequestDto { TotalWorks = 0, TotalConvertedHours = 0m, TotalCalculatedHours = 0m },
                        TotalWorks = 0,
                        TotalCalculatedHours = 0m
                    }
                };
            }

            // Lấy danh sách WorkId duy nhất từ các Author
            var workIds = authors.Select(a => a.WorkId).Distinct().ToList();

            // Lấy danh sách Work tương ứng
            var works = await unitOfWork.Repository<Work>()
                .FindAsync(w => workIds.Contains(w.Id));

            // Lọc các Author theo mục đích
            var dutyAuthors = authors.Where(a => dutyPurposeIds.Contains(a.PurposeId)).ToList();
            var overLimitAuthors = authors.Where(a => overLimitPurposeIds.Contains(a.PurposeId)).ToList();
            //var overLimitMarkedAuthors = overLimitAuthors
            //    .Where(a => a.AuthorRegistration != null && a.AuthorRegistration.AcademicYearId != Guid.Empty)
            //    .ToList();
            var overLimitMarkedAuthors = overLimitAuthors.Where(a => a.MarkedForScoring).ToList();
            var researchAuthors = authors.Where(a => researchPurposeIds.Contains(a.PurposeId)).ToList();

            // Tính số lượng công trình (Work) duy nhất cho từng mục đích
            var dutyWorkIds = dutyAuthors.Select(a => a.WorkId).Distinct().ToList();
            var overLimitWorkIds = overLimitAuthors.Select(a => a.WorkId).Distinct().ToList();
            var researchWorkIds = researchAuthors.Select(a => a.WorkId).Distinct().ToList();

            // Tính toán giờ (sử dụng decimal)
            var dutyConvertedHours = dutyAuthors.Sum(a => a.AuthorHour); // AuthorHour giờ là decimal
            var dutyCalculatedHours = Math.Min(dutyConvertedHours, 80m); // Giới hạn 80 giờ, sử dụng 80m (decimal)

            var overLimitConvertedHours = overLimitAuthors.Sum(a => a.AuthorHour);
            var overLimitCalculatedHours = overLimitMarkedAuthors.Sum(a => a.AuthorHour);

            var researchConvertedHours = 0m; // Mặc định = 0 cho Sản phẩm NCKH (decimal)
            var researchCalculatedHours = 0m;

            // Tổng số công trình duy nhất
            var allWorkIds = authors.Select(a => a.WorkId).Distinct().ToList();

            var result = new UserConversionResultRequestDto
            {
                UserId = userId,
                UserName = await GetUserNameAsync(userId),
                ConversionResults = new ConversionDetailsRequestDto
                {
                    DutyHourConversion = new ConversionItemRequestDto
                    {
                        TotalWorks = dutyWorkIds.Count,
                        TotalConvertedHours = dutyConvertedHours,
                        TotalCalculatedHours = dutyCalculatedHours
                    },
                    OverLimitConversion = new ConversionItemRequestDto
                    {
                        TotalWorks = overLimitWorkIds.Count,
                        TotalConvertedHours = overLimitConvertedHours,
                        TotalCalculatedHours = overLimitCalculatedHours
                    },
                    ResearchProductConversion = new ConversionItemRequestDto
                    {
                        TotalWorks = researchWorkIds.Count,
                        TotalConvertedHours = researchConvertedHours,
                        TotalCalculatedHours = researchCalculatedHours
                    },
                    TotalWorks = allWorkIds.Count,
                    TotalCalculatedHours = dutyCalculatedHours + overLimitCalculatedHours + researchCalculatedHours
                }
            };

            return result;
        }

        private async Task<string> GetUserNameAsync(Guid userId)
        {
            var user = await unitOfWork.Repository<User>().FirstOrDefaultAsync(u => u.Id == userId);
            return user?.UserName ?? "Unknown";
        }

        public async Task<IEnumerable<UserSearchDto>> SearchUsersAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            var normalizedSearchTerm = searchTerm.ToLower();

            var query = unitOfWork.Repository<User>()
                .Include(u => u.Department)
                .Where(u =>
                    u.FullName.ToLower().Contains(normalizedSearchTerm) ||
                    u.UserName.ToLower().Contains(normalizedSearchTerm));

            var users = await query.ToListAsync();

            return users.Select(u => new UserSearchDto
            {
                Id = u.Id,
                FullName = u.FullName,
                UserName = u.UserName,
                DepartmentName = u.Department?.Name ?? "Chưa có phòng ban"
            });
        }

        public async Task<IEnumerable<UserDto>> GetUsersByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            // Kiểm tra DepartmentId hợp lệ
            if (departmentId == Guid.Empty)
            {
                throw new ArgumentException("DepartmentId không hợp lệ", nameof(departmentId));
            }

            // Truy vấn người dùng theo DepartmentId, bao gồm thông tin Department và Field
            var users = await unitOfWork.Repository<User>()
                .Include(u => u.Department)
                .Include(u => u.Field)
                .Where(u => u.DepartmentId == departmentId)
                .ToListAsync(cancellationToken);

            // Ánh xạ sang UserDto
            return mapper.MapToDtos(users);
        }
    }
}
