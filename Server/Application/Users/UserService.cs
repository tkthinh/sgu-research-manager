using System.Security.Cryptography.X509Certificates;
using Application.Shared.Services;
using Domain.Interfaces;
using Domain.Enums;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Users
{
    public class UserService : GenericService<UserDto, User>, IUserService
    {
        public UserService(
            IUnitOfWork unitOfWork,
            IGenericMapper<UserDto, User> mapper,
            ILogger<UserService> logger
            )
            : base(unitOfWork, mapper, logger)
        {
        }

        public async Task<UserDto?> GetUserByIdentityIdAsync(string IdentityId)
        {
            var user = await unitOfWork.Repository<User>()
                .FirstOrDefaultAsync(u => u.IdentityId == IdentityId);

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

            // Lấy danh sách Author của user
            var allAuthors = await unitOfWork.Repository<Author>()
                .FindAsync(a => a.UserId == userId);

            if (allAuthors == null || !allAuthors.Any())
            {
                return new UserConversionResultRequestDto
                {
                    UserId = userId,
                    UserName = await GetUserNameAsync(userId),
                    ConversionResults = new ConversionDetailsRequestDto
                    {
                        DutyHourConversion = new ConversionItemRequestDto { TotalWorks = 0, TotalConvertedHours = 0, TotalCalculatedHours = 0 },
                        OverLimitConversion = new ConversionItemRequestDto { TotalWorks = 0, TotalConvertedHours = 0, TotalCalculatedHours = 0 },
                        ResearchProductConversion = new ConversionItemRequestDto { TotalWorks = 0, TotalConvertedHours = 0, TotalCalculatedHours = 0 },
                        TotalWorks = 0,
                        TotalCalculatedHours = 0
                    }
                };
            }

            // Lấy danh sách WorkId từ các Author
            var workIds = allAuthors.Select(a => a.WorkId).Distinct().ToList();

            // Lấy danh sách Work có ProofStatus = HopLe
            var works = await unitOfWork.Repository<Work>()
                .FindAsync(w => workIds.Contains(w.Id) && w.ProofStatus == ProofStatus.HopLe);

            // Lọc các Author có Work thỏa mãn điều kiện ProofStatus = HopLe
            var validWorkIds = works.Select(w => w.Id).ToList();
            var authors = allAuthors.Where(a => validWorkIds.Contains(a.WorkId)).ToList();

            if (!authors.Any())
            {
                return new UserConversionResultRequestDto
                {
                    UserId = userId,
                    UserName = await GetUserNameAsync(userId),
                    ConversionResults = new ConversionDetailsRequestDto
                    {
                        DutyHourConversion = new ConversionItemRequestDto { TotalWorks = 0, TotalConvertedHours = 0, TotalCalculatedHours = 0 },
                        OverLimitConversion = new ConversionItemRequestDto { TotalWorks = 0, TotalConvertedHours = 0, TotalCalculatedHours = 0 },
                        ResearchProductConversion = new ConversionItemRequestDto { TotalWorks = 0, TotalConvertedHours = 0, TotalCalculatedHours = 0 },
                        TotalWorks = 0,
                        TotalCalculatedHours = 0
                    }
                };
            }

            // Lọc theo mục đích
            var dutyAuthors = authors.Where(a => dutyPurposeIds.Contains(a.PurposeId)).ToList();
            var overLimitAuthors = authors.Where(a => overLimitPurposeIds.Contains(a.PurposeId)).ToList();
            var overLimitMarkedAuthors = overLimitAuthors.Where(a => a.MarkedForScoring).ToList();
            var researchAuthors = authors.Where(a => researchPurposeIds.Contains(a.PurposeId)).ToList();

            // Tính toán
            var dutyConvertedHours = dutyAuthors.Sum(a => a.FinalAuthorHour);
            var dutyCalculatedHours = Math.Min(dutyConvertedHours, 80);

            var overLimitConvertedHours = overLimitAuthors.Sum(a => a.FinalAuthorHour);
            var overLimitCalculatedHours = overLimitMarkedAuthors.Sum(a => a.FinalAuthorHour);

            var researchConvertedHours = 0; // Mặc định = 0 cho Sản phẩm NCKH
            var researchCalculatedHours = 0;

            var result = new UserConversionResultRequestDto
            {
                UserId = userId,
                UserName = await GetUserNameAsync(userId),
                ConversionResults = new ConversionDetailsRequestDto
                {
                    DutyHourConversion = new ConversionItemRequestDto
                    {
                        TotalWorks = dutyAuthors.Count(),
                        TotalConvertedHours = dutyConvertedHours,
                        TotalCalculatedHours = dutyCalculatedHours
                    },
                    OverLimitConversion = new ConversionItemRequestDto
                    {
                        TotalWorks = overLimitAuthors.Count(),
                        TotalConvertedHours = overLimitConvertedHours,
                        TotalCalculatedHours = overLimitCalculatedHours
                    },
                    ResearchProductConversion = new ConversionItemRequestDto
                    {
                        TotalWorks = researchAuthors.Count(),
                        TotalConvertedHours = researchConvertedHours,
                        TotalCalculatedHours = researchCalculatedHours
                    },
                    TotalWorks = dutyAuthors.Count() + overLimitAuthors.Count() + researchAuthors.Count(),
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
    }
}
