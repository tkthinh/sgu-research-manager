using Application.AcademicYears;
using Application.Authors;
using Application.Shared.Messages;
using Application.Shared.Services;
using Application.SystemConfigs;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Works
{
    public class WorkCalculateService : IWorkCalculateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper<WorkDto, Work> _mapper;
        private readonly IGenericMapper<AuthorDto, Author> _authorMapper;
        private readonly IGenericRepository<AuthorRole> _authorRoleRepository;
        private readonly IGenericRepository<Factor> _factorRepository;
        private readonly ILogger<WorkService> _logger;

        public WorkCalculateService(
            IUnitOfWork unitOfWork,
            IGenericMapper<WorkDto, Work> mapper,
            IGenericMapper<AuthorDto, Author> authorMapper,
            ILogger<WorkService> logger,
            IWorkRepository workRepository,
            ISystemConfigService systemConfigService,
            IUserRepository userRepository,
            ICurrentUserService currentUserService,
            IAcademicYearService academicYearService,
            IAuthorService authorService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authorMapper = authorMapper;
            _authorRoleRepository = unitOfWork.Repository<AuthorRole>();
            _factorRepository = unitOfWork.Repository<Factor>();
            _logger = logger;
        }

        public int CalculateWorkHour(ScoreLevel? scoreLevel, Factor factor)
        {
            // Nếu factor không tồn tại, trả về 0
            if (factor is null)
            {
                _logger.LogWarning("CalculateWorkHour - Factor is null, returning 0");
                return 0;
            }

            // Kiểm tra xem ScoreLevel có khớp không
            if ((scoreLevel.HasValue != factor.ScoreLevel.HasValue) ||
                (scoreLevel.HasValue && factor.ScoreLevel.HasValue && scoreLevel.Value != factor.ScoreLevel.Value))
            {
                _logger.LogWarning("CalculateWorkHour - ScoreLevel mismatch: Request={RequestLevel}, Factor={FactorLevel}, nhưng vẫn trả về ConvertHour={ConvertHour}",
                    scoreLevel.HasValue ? scoreLevel.Value.ToString() : "null",
                    factor.ScoreLevel.HasValue ? factor.ScoreLevel.Value.ToString() : "null",
                    factor.ConvertHour);

                // Vẫn trả về ConvertHour của Factor nếu Factor được tìm thấy
                return factor.ConvertHour;
            }

            // Nếu mọi thứ khớp hoặc cả hai đều null, trả về ConvertHour
            _logger.LogInformation("CalculateWorkHour - Returning ConvertHour={ConvertHour} cho ScoreLevel={ScoreLevel}",
                factor.ConvertHour,
                scoreLevel.HasValue ? scoreLevel.Value.ToString() : "null");
            return factor.ConvertHour;
        }

        public async Task<decimal> CalculateAuthorHour(int workHour, int totalAuthors, int totalMainAuthors, Guid? authorRoleId, CancellationToken cancellationToken = default)
        {
            // Hằng số tỷ lệ phân bổ giờ chuẩn
            const double MAIN_AUTHOR_RATIO = 1.0 / 3;
            const double REGULAR_RATIO = 2.0 / 3;

            // Kiểm tra các điều kiện không hợp lệ
            if (workHour <= 0 || authorRoleId == null)
            {
                return 0;
            }

            // Lấy thông tin vai trò tác giả
            var authorRole = await _authorRoleRepository.GetByIdAsync(authorRoleId.Value);
            if (authorRole is null)
            {
                throw new Exception(ErrorMessages.AuthorRoleNotFound);
            }

            // Lấy thông tin công trình để kiểm tra loại
            var work = await _unitOfWork.Repository<Work>()
                .FirstOrDefaultAsync(w => w.Authors.Any(a => a.AuthorRoleId == authorRoleId));

            // Nếu là công trình hội thảo, hội nghị thì trả về workHour
            if (work?.WorkTypeId == Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"))
            {
                return workHour;
            }

            // Kiểm tra các điều kiện không hợp lệ cho các loại công trình khác
            if (totalAuthors <= 0 || totalMainAuthors <= 0 || totalMainAuthors > totalAuthors)
            {
                return 0;
            }

            decimal authorHour;

            // Tính toán giờ chuẩn dựa trên vai trò tác giả (chính/thường)
            if (authorRole.IsMainAuthor)
            {
                // Công thức cho tác giả chính
                authorHour = (decimal)(MAIN_AUTHOR_RATIO * (workHour / totalMainAuthors) +
                                      REGULAR_RATIO * (workHour / totalAuthors));
            }
            else
            {
                // Công thức cho tác giả thường
                authorHour = (decimal)(REGULAR_RATIO * (workHour / totalAuthors));
            }

            // Làm tròn đến 1 chữ số thập phân để đảm bảo tính nhất quán
            return Math.Round(authorHour, 1);
        }

        public bool ShouldRecalculateAuthorHours(Authors.UpdateAuthorRequestDto authorRequest, UpdateWorkRequestDto? workRequest)
        {
            return authorRequest.ScoreLevel.HasValue ||
                (workRequest is not null && (workRequest.TotalAuthors.HasValue || workRequest.TotalMainAuthors.HasValue));
        }

        public async Task<Factor> FindFactorAsync(Guid workTypeId, Guid? workLevelId, Guid purposeId, Guid? authorRoleId, ScoreLevel? scoreLevel, CancellationToken cancellationToken = default)
        {
            // Truy vấn dựa trên các điều kiện cơ bản
            Expression<Func<Factor, bool>> baseCondition = f => 
                f.WorkTypeId == workTypeId && 
                f.PurposeId == purposeId;
            
            var baseQuery = await _factorRepository.FindAsync(baseCondition);

            // Lọc thêm theo các điều kiện tùy chọn
            var filtered = baseQuery.Where(f => 
                (workLevelId.HasValue ? f.WorkLevelId == workLevelId : f.WorkLevelId == null) &&
                (authorRoleId.HasValue ? (f.AuthorRoleId == null || f.AuthorRoleId == authorRoleId) : f.AuthorRoleId == null) &&
                (scoreLevel.HasValue ? f.ScoreLevel == scoreLevel : f.ScoreLevel == null)
            ).ToList();

            // Ưu tiên các factor có AuthorRoleId khớp chính xác
            var factor = filtered.FirstOrDefault(f => f.AuthorRoleId == authorRoleId) ?? filtered.FirstOrDefault();

            if (factor == null)
            {
                _logger.LogWarning(
                    "FindFactorAsync - Không tìm thấy factor phù hợp: WorkTypeId={WorkTypeId}, WorkLevelId={WorkLevelId}, PurposeId={PurposeId}, AuthorRoleId={AuthorRoleId}, ScoreLevel={ScoreLevel}",
                    workTypeId, workLevelId, purposeId, authorRoleId, scoreLevel);
            }
            else
            {
                _logger.LogInformation(
                    "FindFactorAsync - Đã tìm thấy factor: Id={FactorId}, ConvertHour={ConvertHour}, MaxAllowed={MaxAllowed}",
                    factor.Id, factor.ConvertHour, factor.MaxAllowed);
            }

            return factor;
        }
    }
}
