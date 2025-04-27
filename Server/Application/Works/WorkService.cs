using Application.Authors;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Domain.Enums;
using Application.SystemConfigs;
using System.Data;
using Application.Shared.Messages;
using Application.AcademicYears;
using Microsoft.EntityFrameworkCore;

namespace Application.Works
{
    public class WorkService : GenericCachedService<WorkDto, Work>, IWorkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper<WorkDto, Work> _mapper;
        private readonly IWorkRepository _workRepository;
        private readonly ISystemConfigService _systemConfigService;
        private readonly ILogger<WorkService> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAcademicYearService _academicYearService;
        private readonly IWorkCalculateService _workCalculateService;

        public WorkService(
            IUnitOfWork unitOfWork,
            IGenericMapper<WorkDto, Work> mapper,
            IGenericMapper<AuthorDto, Author> authorMapper,
            IDistributedCache cache,
            ILogger<WorkService> logger,
            IWorkRepository workRepository,
            ISystemConfigService systemConfigService,
            IUserRepository userRepository,
            ICurrentUserService currentUserService,
            IAcademicYearService academicYearService,
            IAuthorService authorService,
            IWorkCalculateService workCalculateService)
            : base(unitOfWork, mapper, cache, logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _workRepository = workRepository;
            _systemConfigService = systemConfigService;
            _logger = logger;
            _currentUserService = currentUserService;
            _academicYearService = academicYearService;
            _workCalculateService = workCalculateService;
        }
        public async Task<WorkDto> CreateWorkWithAuthorAsync(CreateWorkRequestDto request, CancellationToken cancellationToken = default)
        {
            // Kiểm tra trạng thái hệ thống
            if (!await _systemConfigService.IsSystemOpenAsync(DateTime.UtcNow, cancellationToken))
                throw new Exception(ErrorMessages.SystemClosedNewWork);

            var existingWork = await _unitOfWork.Repository<Work>()
                .FirstOrDefaultAsync(w => w.Title == request.Title &&
                    w.WorkTypeId == request.WorkTypeId &&
                    w.WorkLevelId == request.WorkLevelId, cancellationToken);
            if (existingWork != null)
                throw new Exception(ErrorMessages.WorkAlreadyExists);

            var work = new Work
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                TimePublished = request.TimePublished,
                TotalAuthors = request.TotalAuthors,
                TotalMainAuthors = request.TotalMainAuthors,
                Details = request.Details,
                Source = WorkSource.NguoiDungKeKhai,
                WorkTypeId = request.WorkTypeId,
                WorkLevelId = request.WorkLevelId,
                AcademicYearId = request.AcademicYearId,
                ExchangeDeadline = request.TimePublished.HasValue ? request.TimePublished.Value.AddMonths(18) : null,
                CreatedDate = DateTime.UtcNow,
                IsLocked = false // Công trình mới luôn có IsLocked = false
            };

            var factor = await FindFactorAsync(
                work.WorkTypeId,
                work.WorkLevelId,
                request.Author.PurposeId,
                request.Author.AuthorRoleId,
                request.Author.ScoreLevel,
                cancellationToken);

            if (factor == null)
                throw new Exception(ErrorMessages.FactorNotFound);

            var workHour = _workCalculateService.CalculateWorkHour(request.Author.ScoreLevel, factor);

            await _unitOfWork.Repository<Work>().CreateAsync(work);

            // Lấy UserId từ service
            var (isSuccess, userId, _) = _currentUserService.GetCurrentUser();
            if (!isSuccess)
            {
                throw new Exception(ErrorMessages.UserIdNotDetermined);
            }

            var author = new Author
            {
                Id = Guid.NewGuid(),
                WorkId = work.Id,
                UserId = userId,
                AuthorRoleId = request.Author.AuthorRoleId,
                PurposeId = request.Author.PurposeId,
                Position = request.Author.Position,
                ScoreLevel = request.Author.ScoreLevel,
                WorkHour = workHour,
                SCImagoFieldId = request.Author.SCImagoFieldId,
                FieldId = request.Author.FieldId,
                ProofStatus = ProofStatus.ChuaXuLy,
                CreatedDate = DateTime.UtcNow
            };

            var authorHour = await _workCalculateService.CalculateAuthorHour(workHour, request.TotalAuthors ?? 0,
                request.TotalMainAuthors ?? 0, request.Author.AuthorRoleId);
            author.AuthorHour = authorHour;

            // Loại bỏ userId của tác giả chính khỏi request.CoAuthorUserIds để tránh trùng lặp
            var coAuthorUserIds = request.CoAuthorUserIds
                .Where(uid => uid != userId) // Loại bỏ userId nếu có
                .Distinct() // Đảm bảo không có trùng lặp trong danh sách
                .ToList();

            // Lưu thông tin đồng tác giả vào WorkAuthor
            foreach (var coAuthorUserId in coAuthorUserIds)
            {
                var workAuthor = new WorkAuthor
                {
                    Id = Guid.NewGuid(),
                    WorkId = work.Id,
                    UserId = coAuthorUserId
                };
                await _unitOfWork.Repository<WorkAuthor>().CreateAsync(workAuthor);
            }

            // Thêm tác giả chính vào WorkAuthor (nếu cần thiết)
            var existingWorkAuthor = await _unitOfWork.Repository<WorkAuthor>()
                .FirstOrDefaultAsync(wa => wa.WorkId == work.Id && wa.UserId == userId, cancellationToken);
            if (existingWorkAuthor == null) // Chỉ thêm nếu chưa tồn tại
            {
                var workAuthor = new WorkAuthor
                {
                    Id = Guid.NewGuid(),
                    WorkId = work.Id,
                    UserId = userId
                };
                await _unitOfWork.Repository<WorkAuthor>().CreateAsync(workAuthor);
            }

            await _unitOfWork.Repository<Author>().CreateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var workDto = _mapper.MapToDto(work);
            // Gán danh sách đồng tác giả không bao gồm tác giả chính
            workDto.CoAuthorUserIds = coAuthorUserIds;
            await SafeInvalidateCacheAsync(work.Id);

            // Nếu là đề tài, tạo bài báo khoa học tự động
            if (work.WorkTypeId == Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348"))
            {
                try
                {
                    await CreateScientificArticleFromProjectAsync(work, userId, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi tạo bài báo khoa học tự động từ đề tài {ProjectId}", work.Id);
                    // Không throw exception ở đây để không ảnh hưởng đến việc tạo đề tài
                    // Thay vào đó, ghi log lỗi và tiếp tục
                }
            }

            return workDto;
        }


        public async Task DeleteWorkAsync(Guid workId, Guid userId, CancellationToken cancellationToken = default)
        {
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(workId);
            if (work is null)
                throw new Exception(ErrorMessages.WorkNotFound);

            if (!await _systemConfigService.IsSystemOpenAsync(DateTime.UtcNow, cancellationToken))
                throw new Exception(ErrorMessages.SystemClosedDeleteWork);

            // Xóa các tác giả liên quan
            var authors = await _unitOfWork.Repository<Author>().FindAsync(a => a.WorkId == workId);
            foreach (var author in authors)
            {
                await _unitOfWork.Repository<Author>().DeleteAsync(author.Id);
            }

            await _unitOfWork.Repository<Work>().DeleteAsync(work.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await SafeInvalidateCacheAsync(workId);
        }

        public async Task RegisterWorkByAuthorAsync(Guid authorId, bool registered, CancellationToken cancellationToken = default)
        {
            // Kiểm tra trạng thái hệ thống
            if (!await _systemConfigService.IsSystemOpenAsync(DateTime.UtcNow, cancellationToken))
                throw new Exception(ErrorMessages.SystemClosedMarkingWork);

            // Lấy thông tin tác giả và công trình
            var author = await _unitOfWork.Repository<Author>()
                .Include(a => a.Work)
                .FirstOrDefaultAsync(a => a.Id == authorId);

            if (author == null)
                throw new Exception(ErrorMessages.AuthorNotFound);

            var work = author.Work;
            if (work == null)
                throw new Exception(ErrorMessages.WorkNotFound);

            // Lấy năm học hiện tại
            var currentAcademicYear = await _academicYearService.GetCurrentAcademicYear(cancellationToken);

            if (registered)
            {
                // 1. Lấy Factor để kiểm tra giới hạn
                var factor = await FindFactorAsync(
                    work.WorkTypeId,
                    work.WorkLevelId,
                    author.PurposeId,
                    author.AuthorRoleId,
                    author.ScoreLevel,
                    cancellationToken);

                // 2. Đếm số lượng công trình đã được đánh dấu với cùng điều kiện
                var authorRegistrations = await _unitOfWork.Repository<AuthorRegistration>()
                    .Include(ar => ar.Author)
                    .Where(ar => 
                        ar.Author.UserId == author.UserId &&
                        ar.AcademicYearId == currentAcademicYear.Id &&
                        ar.Author.PurposeId == author.PurposeId && 
                        ar.Author.ScoreLevel == author.ScoreLevel)
                    .ToListAsync(cancellationToken);

                // 3. Lấy thông tin WorkType và WorkLevel để hiển thị thông báo lỗi
                var workType = await _unitOfWork.Repository<WorkType>().GetByIdAsync(work.WorkTypeId);
                WorkLevel? workLevel = null;
                if (work.WorkLevelId.HasValue)
                {
                    workLevel = await _unitOfWork.Repository<WorkLevel>().GetByIdAsync(work.WorkLevelId.Value);
                }

                // 4. Kiểm tra giới hạn nếu có MaxAllowed
                if (factor?.MaxAllowed != null)
                {
                    if (authorRegistrations.Count() >= factor.MaxAllowed)
                    {
                        var workTypeInfo = workType?.Name ?? "Không xác định";
                        var workLevelInfo = workLevel?.Name ?? "Không xác định";
                        var scoreLevelInfo = author.ScoreLevel.HasValue ? author.ScoreLevel.ToString() : "Không xác định";

                        throw new Exception(string.Format(ErrorMessages.ScoreLimitExceeded,
                            factor.MaxAllowed,
                            workTypeInfo,
                            workLevelInfo,
                            scoreLevelInfo));
                    }
                }

                // 5. Tạo bản ghi AuthorRegistration nếu chưa có
                var existingRegistration = await _unitOfWork.Repository<AuthorRegistration>()
                    .FirstOrDefaultAsync(ar => ar.AuthorId == authorId && ar.AcademicYearId == currentAcademicYear.Id);

                if (existingRegistration == null)
                {
                    var authorRegistration = new AuthorRegistration
                    {
                        AuthorId = authorId,
                        AcademicYearId = currentAcademicYear.Id,
                    };
                    await _unitOfWork.Repository<AuthorRegistration>().CreateAsync(authorRegistration);
                }
            }
            else
            {
                // Nếu bỏ đánh dấu, xóa bản ghi AuthorRegistration nếu có
                var existingRegistration = await _unitOfWork.Repository<AuthorRegistration>()
                    .FirstOrDefaultAsync(ar => ar.AuthorId == authorId && ar.AcademicYearId == currentAcademicYear.Id);

                if (existingRegistration != null)
                {
                    await _unitOfWork.Repository<AuthorRegistration>().DeleteAsync(existingRegistration.Id);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<WorkDto> UpdateWorkByAdminAsync(Guid workId, Guid userId, UpdateWorkWithAuthorRequestDto request, CancellationToken cancellationToken = default)
        {
            // Truy vấn tuần tự để tránh xung đột DbContext
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(workId);
            if (work is null) throw new Exception(ErrorMessages.WorkNotFound);

            // Tìm Author tương ứng với UserId và WorkId
            var author = await _unitOfWork.Repository<Author>()
                .FirstOrDefaultAsync(a => a.WorkId == workId && a.UserId == userId, cancellationToken);
            if (author is null) throw new Exception(ErrorMessages.AuthorNotFoundInWork);

            // Cập nhật thông tin công trình (Work) nếu có WorkRequest
            if (request.WorkRequest is not null)
            {
                await UpdateWorkDetailsByAdmin(work, request.WorkRequest);
            }

            // Cập nhật thông tin tác giả (Author) nếu có AuthorRequest
            if (request.AuthorRequest is not null)
            {
                await UpdateAuthorDetailsByAdminAsync(author, work, request.AuthorRequest, request.WorkRequest, cancellationToken);
            }

            // Lưu thay đổi vào database - cập nhật tuần tự
            await _unitOfWork.Repository<Work>().UpdateAsync(work);
            await _unitOfWork.Repository<Author>().UpdateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Invalidate cache
            await SafeInvalidateCacheAsync(workId);

            return _mapper.MapToDto(work);
        }

        public async Task<WorkDto> UpdateWorkByAuthorAsync(Guid workId, UpdateWorkWithAuthorRequestDto request, Guid userId, CancellationToken cancellationToken = default)
        {
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(workId);
            if (work is null)
                throw new Exception(ErrorMessages.WorkNotFound);

            // Lấy coAuthorUserIds từ WorkRequest
            List<Guid> coAuthorUserIds = request.WorkRequest?.CoAuthorUserIds ?? new List<Guid>();

            await ValidateSystemStateAsync(work, userId, cancellationToken);

            if (request.WorkRequest is not null)
            {
                await UpdateWorkDetailsByAuthor(work, request.WorkRequest);
                await UpdateCoAuthorsAsync(work, coAuthorUserIds, userId, cancellationToken);
            }

            var author = await GetOrCreateAuthorAsync(work, userId, request, cancellationToken);

            // Lưu các thay đổi vào database
            await SaveChangesAsync(work, author, cancellationToken);

            // Ánh xạ sang DTO và điền thông tin đồng tác giả
            var workDto = _mapper.MapToDto(work);

            await FillCoAuthorUserIdsAsync(workDto, cancellationToken);

            return workDto;
        }

        private async Task UpdateWorkDetailsByAdmin(Work work, UpdateWorkRequestDto workRequest)
        {
            // Cập nhật isLocked dựa trên trạng thái của các tác giả
            var authors = await _unitOfWork.Repository<Author>().FindAsync(a => a.WorkId == work.Id);
            work.IsLocked = authors.Any(a => a.ProofStatus == ProofStatus.HopLe);

            work.Title = workRequest.Title ?? work.Title;
            work.TimePublished = workRequest.TimePublished ?? work.TimePublished;
            work.TotalAuthors = workRequest.TotalAuthors ?? work.TotalAuthors;
            work.TotalMainAuthors = workRequest.TotalMainAuthors ?? work.TotalMainAuthors;
            work.Details = workRequest.Details ?? work.Details;
            work.Source = workRequest.Source;
            work.WorkTypeId = workRequest.WorkTypeId ?? work.WorkTypeId;
            work.WorkLevelId = workRequest.WorkLevelId ?? work.WorkLevelId;
            if (workRequest.AcademicYearId.HasValue)
                work.AcademicYearId = workRequest.AcademicYearId.Value;
            work.ExchangeDeadline = work.TimePublished.HasValue ? work.TimePublished.Value.AddMonths(18) : null;
            work.ModifiedDate = DateTime.UtcNow;
        }

        private async Task UpdateAuthorDetailsByAdminAsync(
            Author author, 
            Work work, 
            UpdateAuthorRequestDto authorRequest, 
            UpdateWorkRequestDto? workRequest,
            CancellationToken cancellationToken)
        {
            // Cập nhật các thuộc tính cơ bản
            UpdateAuthorBasicProperties(author, authorRequest);

                // Tính lại WorkHour và AuthorHour nếu có thay đổi liên quan
            if (_workCalculateService.ShouldRecalculateAuthorHours(authorRequest, workRequest))
            {
                await UpdateAuthorHoursAsync(author, work, authorRequest.ScoreLevel, workRequest, cancellationToken);
            }

            // Xử lý ProofStatus và Note
            UpdateAuthorProofStatus(author, authorRequest);

            author.ModifiedDate = DateTime.UtcNow;
        }

        private void UpdateAuthorBasicProperties(Author author, UpdateAuthorRequestDto authorRequest)
        {
            author.AuthorRoleId = authorRequest.AuthorRoleId ?? author.AuthorRoleId;
            author.PurposeId = authorRequest.PurposeId ?? author.PurposeId;
            author.Position = authorRequest.Position ?? author.Position;
            author.ScoreLevel = authorRequest.ScoreLevel ?? author.ScoreLevel;
            author.SCImagoFieldId = authorRequest.SCImagoFieldId ?? author.SCImagoFieldId;
            author.FieldId = authorRequest.FieldId ?? author.FieldId;
        }

        private async Task UpdateAuthorHoursAsync(
            Author author,
            Work work,
            ScoreLevel? newScoreLevel,
            UpdateWorkRequestDto? workRequest,
            CancellationToken cancellationToken)
        {
            var effectiveScoreLevel = newScoreLevel ?? author.ScoreLevel;

            // Ghi log thông tin trước khi tính toán
            _logger.LogInformation(
                "UpdateAuthorHoursAsync - Bắt đầu tính giờ quy đổi: WorkTypeId={WorkTypeId}, WorkLevelId={WorkLevelId}, PurposeId={PurposeId}, AuthorRoleId={AuthorRoleId}, ScoreLevel={ScoreLevel}",
                work.WorkTypeId, work.WorkLevelId, author.PurposeId, author.AuthorRoleId, effectiveScoreLevel);

            // Sử dụng phương thức mới để tìm factor
            var factor = await FindFactorAsync(
                work.WorkTypeId,
                work.WorkLevelId,
                author.PurposeId,
                author.AuthorRoleId,
                effectiveScoreLevel,
                cancellationToken);

            if (factor is null)
            {
                _logger.LogError("UpdateAuthorHoursAsync - Không tìm thấy Factor phù hợp");
                throw new Exception(ErrorMessages.FactorNotFound);
            }

            // Tính toán WorkHour và AuthorHour
            author.WorkHour = _workCalculateService.CalculateWorkHour(effectiveScoreLevel, factor);
            
            // Sử dụng các giá trị từ workRequest nếu có, nếu không thì dùng các giá trị từ work
            int totalAuthors = workRequest?.TotalAuthors ?? work.TotalAuthors ?? 0;
            int totalMainAuthors = workRequest?.TotalMainAuthors ?? work.TotalMainAuthors ?? 0;
            
            author.AuthorHour = await _workCalculateService.CalculateAuthorHour(
                author.WorkHour,
                totalAuthors,
                totalMainAuthors,
                author.AuthorRoleId);
                
            _logger.LogInformation(
                "UpdateAuthorHoursAsync - Kết quả: Factor.ConvertHour={ConvertHour}, WorkHour={WorkHour}, AuthorHour={AuthorHour}, TotalAuthors={TotalAuthors}, TotalMainAuthors={TotalMainAuthors}",
                factor.ConvertHour, author.WorkHour, author.AuthorHour, totalAuthors, totalMainAuthors);
        }

        private void UpdateAuthorProofStatus(Author author, UpdateAuthorRequestDto authorRequest)
        {
            if (authorRequest.ProofStatus.HasValue)
            {
                author.ProofStatus = authorRequest.ProofStatus.Value;
                author.Note = authorRequest.Note ?? author.Note;
            }
        }

        private async Task UpdateCoAuthorsAsync(Work work, List<Guid> newCoAuthorUserIds, Guid currentUserId, CancellationToken cancellationToken)
        {
            try {
                // Đảm bảo newCoAuthorUserIds không bao gồm currentUserId
                newCoAuthorUserIds = newCoAuthorUserIds.Where(id => id != currentUserId).Distinct().ToList();


                // Lấy tất cả WorkAuthor hiện có của công trình
                var existingWorkAuthors = await _unitOfWork.Repository<WorkAuthor>()
                    .FindAsync(wa => wa.WorkId == work.Id);
                var existingCoAuthorIds = existingWorkAuthors.Select(wa => wa.UserId).ToList();

                // Lấy thêm tất cả Author của công trình (trừ tác giả chính)
                var existingAuthors = await _unitOfWork.Repository<Author>()
                    .FindAsync(a => a.WorkId == work.Id && a.UserId != currentUserId);
                var existingAuthorUserIds = existingAuthors.Select(a => a.UserId).ToList();

                // Tổng hợp tất cả User IDs hiện có (từ cả WorkAuthor và Author)
                var allExistingUserIds = existingCoAuthorIds.Union(existingAuthorUserIds).Distinct().ToList();

                // 1. Xóa các WorkAuthor không còn trong danh sách mới
                var workAuthorsToRemove = existingWorkAuthors.Where(wa => 
                    wa.UserId != currentUserId && !newCoAuthorUserIds.Contains(wa.UserId)).ToList();

                // 2. Xóa các Author không còn trong danh sách mới
                // * Quan trọng: Không xóa Author của người dùng hiện tại (currentUserId)
                var authorsToRemove = existingAuthors.Where(a => 
                    a.UserId != currentUserId && !newCoAuthorUserIds.Contains(a.UserId)).ToList();
                
                foreach (var author in authorsToRemove)
                {
                    work.Authors.Remove(author);
                    // Xóa Author khỏi cơ sở dữ liệu
                    await _unitOfWork.Repository<Author>().DeleteAsync(author.Id);
                }

                // Tìm các userId cần thêm mới: những userId có trong newCoAuthorUserIds nhưng không có trong allExistingUserIds
                var userIdsToAdd = newCoAuthorUserIds.Where(id => 
                    !allExistingUserIds.Contains(id) && id != currentUserId).ToList();

                // Thêm các đồng tác giả mới
                foreach (var coAuthorId in userIdsToAdd)
                {
                    try 
                    {
                        var workAuthor = new WorkAuthor
                        {
                            Id = Guid.NewGuid(),
                            WorkId = work.Id,
                            UserId = coAuthorId
                        };
                        await _unitOfWork.Repository<WorkAuthor>().CreateAsync(workAuthor);
                        
                        // Thêm vào danh sách WorkAuthors của đối tượng Work
                        work.WorkAuthors.Add(workAuthor);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error adding coauthor {UserId} to work {WorkId}", coAuthorId, work.Id);
                    }
                }
                
                _logger.LogInformation("UpdateCoAuthorsAsync completed for work {WorkId}", work.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateCoAuthorsAsync for work {WorkId}", work.Id);
            }
        }

        private async Task ValidateSystemStateAsync(Work work, Guid userId, CancellationToken cancellationToken)
        {
            var currentAuthor = await _unitOfWork.Repository<Author>()
                .FirstOrDefaultAsync(a => a.WorkId == work.Id && a.UserId == userId, cancellationToken);

            if (!await _systemConfigService.IsSystemOpenAsync(DateTime.UtcNow, cancellationToken))
            {
                if (currentAuthor is null || currentAuthor.ProofStatus != ProofStatus.KhongHopLe)
                    throw new Exception(ErrorMessages.SystemClosedEditWork);
            }
        }

        private async Task UpdateWorkDetailsByAuthor(Work work, UpdateWorkRequestDto workRequest)
        {
            // Cập nhật isLocked dựa trên trạng thái của các tác giả
            var authors = await _unitOfWork.Repository<Author>().FindAsync(a => a.WorkId == work.Id);
            work.IsLocked = authors.Any(a => a.ProofStatus == ProofStatus.HopLe);

            work.Title = workRequest.Title ?? work.Title;
            work.TimePublished = workRequest.TimePublished ?? work.TimePublished;
            work.TotalAuthors = workRequest.TotalAuthors ?? work.TotalAuthors;
            work.TotalMainAuthors = workRequest.TotalMainAuthors ?? work.TotalMainAuthors;
            work.Details = workRequest.Details ?? work.Details;
            work.Source = WorkSource.NguoiDungKeKhai; // Luôn đặt nguồn là NguoiDungKeKhai
            work.WorkTypeId = workRequest.WorkTypeId ?? work.WorkTypeId;
            work.WorkLevelId = workRequest.WorkLevelId ?? work.WorkLevelId;

            work.ExchangeDeadline = work.TimePublished.HasValue ? work.TimePublished.Value.AddMonths(18) : null;
            work.ModifiedDate = DateTime.UtcNow;
        }

        private async Task<Author> GetOrCreateAuthorAsync(Work work, Guid userId, UpdateWorkWithAuthorRequestDto request, CancellationToken cancellationToken)
        {
            var author = await _unitOfWork.Repository<Author>()
                .FirstOrDefaultAsync(a => a.WorkId == work.Id && a.UserId == userId, cancellationToken);

            if (author is null)
            {
                // Tạo mới tác giả
                author = await CreateNewAuthorAsync(work, userId, request, cancellationToken);
            }
            else if (request.AuthorRequest is not null)
            {
                // Cập nhật tác giả đã tồn tại
                await UpdateExistingAuthorAsync(author, work, request, cancellationToken);
            }

            return author;
        }

        private async Task<Author> CreateNewAuthorAsync(Work work, Guid userId, UpdateWorkWithAuthorRequestDto request, CancellationToken cancellationToken)
        {
            if (request.AuthorRequest is null)
            {
                throw new Exception(ErrorMessages.AuthorInfoRequired);
            }

            // Tính toán WorkHour và AuthorHour cho tác giả mới
            var factor = await FindFactorAsync(
                work.WorkTypeId,
                work.WorkLevelId,
                request.AuthorRequest.PurposeId ?? Guid.Empty,
                request.AuthorRequest.AuthorRoleId,
                request.AuthorRequest.ScoreLevel,
                cancellationToken);

            if (factor is null)
                throw new Exception(ErrorMessages.FactorNotFound);

            var workHour = _workCalculateService.CalculateWorkHour(request.AuthorRequest.ScoreLevel, factor);
            var authorHour = await _workCalculateService.CalculateAuthorHour(
                workHour,
                work.TotalAuthors ?? 0,
                work.TotalMainAuthors ?? 0,
                request.AuthorRequest.AuthorRoleId);

            // Tạo mới thông tin tác giả
            return new Author
            {
                UserId = userId,
                WorkId = work.Id,
                AuthorRoleId = request.AuthorRequest.AuthorRoleId,
                PurposeId = request.AuthorRequest.PurposeId ?? Guid.Empty,
                Position = request.AuthorRequest.Position,
                ScoreLevel = request.AuthorRequest.ScoreLevel,
                WorkHour = workHour,
                AuthorHour = authorHour,
                SCImagoFieldId = request.AuthorRequest.SCImagoFieldId,
                FieldId = request.AuthorRequest.FieldId,
                ProofStatus = ProofStatus.ChuaXuLy,
                CreatedDate = DateTime.UtcNow
            };
        }

        private async Task<Factor> FindFactorAsync(Guid workTypeId, Guid? workLevelId, Guid purposeId, Guid? authorRoleId, ScoreLevel? scoreLevel, CancellationToken cancellationToken = default)
        {
            return await _workCalculateService.FindFactorAsync(workTypeId, workLevelId, purposeId, authorRoleId, scoreLevel, cancellationToken);
        }

        private async Task UpdateExistingAuthorAsync(Author author, Work work, UpdateWorkWithAuthorRequestDto request, CancellationToken cancellationToken)
        {
            // Cập nhật thông tin tác giả
            if (request.AuthorRequest is not null)
            {
                // Ghi log trạng thái ban đầu
                _logger.LogInformation(
                    "UpdateExistingAuthorAsync - Trạng thái ban đầu: AuthorId={AuthorId}, WorkHour={WorkHour}, AuthorHour={AuthorHour}, ScoreLevel={ScoreLevel}",
                    author.Id, author.WorkHour, author.AuthorHour, author.ScoreLevel);
                
                // AuthorRoleId và PurposeId là Guid không nullable, gán trực tiếp
                author.AuthorRoleId = request.AuthorRequest.AuthorRoleId;
                author.PurposeId = request.AuthorRequest.PurposeId ?? Guid.Empty;

                author.Position = request.AuthorRequest.Position ?? author.Position;
                
                // Lưu trữ giá trị ScoreLevel ban đầu và mới để so sánh
                var oldScoreLevel = author.ScoreLevel;
                author.ScoreLevel = request.AuthorRequest.ScoreLevel ?? author.ScoreLevel;
                var hasScoreLevelChanged = request.AuthorRequest.ScoreLevel.HasValue;

                // SCImagoFieldId và FieldId là nullable Guid
                author.SCImagoFieldId = request.AuthorRequest.SCImagoFieldId;
                author.FieldId = request.AuthorRequest.FieldId;

                // Tính lại WorkHour và AuthorHour khi có thay đổi liên quan
                // Luôn tính toán lại nếu ScoreLevel thay đổi
                bool shouldRecalculate = hasScoreLevelChanged ||
                    (request.WorkRequest is not null && (request.WorkRequest.TotalAuthors.HasValue || request.WorkRequest.TotalMainAuthors.HasValue));

                if (shouldRecalculate)
                {
                    _logger.LogInformation(
                        "UpdateExistingAuthorAsync - Tính toán lại giờ quy đổi: WorkTypeId={WorkTypeId}, WorkLevelId={WorkLevelId}, PurposeId={PurposeId}, AuthorRoleId={AuthorRoleId}, ScoreLevel={ScoreLevel}",
                        work.WorkTypeId, work.WorkLevelId, author.PurposeId, author.AuthorRoleId, author.ScoreLevel);
                        
                    var factor = await FindFactorAsync(
                        work.WorkTypeId,
                        work.WorkLevelId,
                        author.PurposeId,
                        author.AuthorRoleId,
                        author.ScoreLevel,
                        cancellationToken);

                    if (factor is null)
                    {
                        _logger.LogError(
                            "UpdateExistingAuthorAsync - Không tìm thấy Factor: WorkTypeId={WorkTypeId}, WorkLevelId={WorkLevelId}, PurposeId={PurposeId}, AuthorRoleId={AuthorRoleId}, ScoreLevel={ScoreLevel}",
                            work.WorkTypeId, work.WorkLevelId, author.PurposeId, author.AuthorRoleId, author.ScoreLevel);
                        throw new Exception(ErrorMessages.FactorNotFound);
                    }

                    // Tính giờ quy đổi
                    author.WorkHour = _workCalculateService.CalculateWorkHour(author.ScoreLevel, factor);
                    author.AuthorHour = await _workCalculateService.CalculateAuthorHour(
                        author.WorkHour,
                        work.TotalAuthors ?? 0,
                        work.TotalMainAuthors ?? 0,
                        author.AuthorRoleId);
                        
                    _logger.LogInformation(
                        "UpdateExistingAuthorAsync - Kết quả tính toán: Factor.ConvertHour={ConvertHour}, WorkHour={WorkHour}, AuthorHour={AuthorHour}",
                        factor.ConvertHour, author.WorkHour, author.AuthorHour);
                }
                else
                {
                    _logger.LogInformation("UpdateExistingAuthorAsync - Không cần tính lại giờ quy đổi");
                }

                author.ModifiedDate = DateTime.UtcNow;
                
                _logger.LogInformation(
                    "UpdateExistingAuthorAsync - Trạng thái sau cập nhật: AuthorId={AuthorId}, WorkHour={WorkHour}, AuthorHour={AuthorHour}, ScoreLevel={ScoreLevel}",
                    author.Id, author.WorkHour, author.AuthorHour, author.ScoreLevel);
            }
        }

        private async Task SaveChangesAsync(Work work, Author author, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.Repository<Work>().UpdateAsync(work);

            if (author.Id == Guid.Empty)
            {
                await _unitOfWork.Repository<Author>().CreateAsync(author);
            }
            else
            {
                await _unitOfWork.Repository<Author>().UpdateAsync(author);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SafeInvalidateCacheAsync(work.Id);
        }

        private async Task FillCoAuthorUserIdsAsync(WorkDto workDto, CancellationToken cancellationToken = default)
        {
            if (workDto is null)
                return;

            // Lấy userId từ service
            var (isSuccess, currentUserId, _) = _currentUserService.GetCurrentUser();
            
            _logger.LogInformation("FillCoAuthorUserIdsAsync - WorkDto {WorkId}: Start", workDto.Id);

            try {
                // Lấy những UserId đã có thông tin Author
                var authors = await _unitOfWork.Repository<Author>()
                    .FindAsync(a => a.WorkId == workDto.Id);
                var authorUserIds = authors.Select(a => a.UserId).ToList();

                // Lấy những UserId trong bảng WorkAuthor
                var workAuthors = await _unitOfWork.Repository<WorkAuthor>()
                    .FindAsync(wa => wa.WorkId == workDto.Id);
                var coAuthorUserIds = workAuthors.Select(wa => wa.UserId).ToList();

                _logger.LogInformation("FillCoAuthorUserIdsAsync - Authors found: {Count}", authors.Count());
                _logger.LogInformation("FillCoAuthorUserIdsAsync - WorkAuthors found: {Count}", workAuthors.Count());
                _logger.LogInformation("Authors UserIds: {AuthorIds}", string.Join(", ", authorUserIds));
                _logger.LogInformation("WorkAuthors UserIds: {WorkAuthors}", string.Join(", ", coAuthorUserIds));

                // Kết hợp tất cả UserId, loại bỏ currentUserId (nếu có), và đảm bảo không có trùng lặp
                var combinedUserIds = authorUserIds.Union(coAuthorUserIds);
                if (isSuccess)
                {
                    combinedUserIds = combinedUserIds.Where(uid => uid != currentUserId);
                }
                workDto.CoAuthorUserIds = combinedUserIds.Distinct().ToList();
                
                _logger.LogInformation("Final CoAuthorUserIds: {Count} users - {FinalIds}", 
                    workDto.CoAuthorUserIds.Count,
                    string.Join(", ", workDto.CoAuthorUserIds));
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error filling CoAuthorUserIds for WorkDto {WorkId}", workDto.Id);
                // Đảm bảo ít nhất trả về một danh sách rỗng thay vì null
                workDto.CoAuthorUserIds = new List<Guid>();
            }
        }

        private async Task CreateScientificArticleFromProjectAsync(Work project, Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Bắt đầu tạo bài báo khoa học tự động từ đề tài {ProjectId}", project.Id);
                
                // Lấy nội dung sản phẩm từ details của đề tài
                var productDetails = project.Details?.GetValueOrDefault("Sản phẩm thuộc đề tài");
                _logger.LogInformation("Thông tin sản phẩm từ đề tài: {ProductDetails}", productDetails);
                
                if (string.IsNullOrEmpty(productDetails))
                {
                    _logger.LogWarning("Không tìm thấy thông tin sản phẩm trong details của đề tài {ProjectId}", project.Id);
                    return;
                }

                // Tạo công trình mới (Bài báo khoa học)
                var scientificArticle = new Work
                {
                    Id = Guid.NewGuid(),
                    Title = productDetails,
                    TimePublished = project.TimePublished,
                    TotalAuthors = project.TotalAuthors,
                    TotalMainAuthors = project.TotalMainAuthors,
                    Details = new Dictionary<string, string>
                    {
                        { "Tên tạp chí", "" },
                        { "Tập, số phát hành", "" },
                        { "Số trang", "" },
                        { "Chỉ số xuất bản", "" },
                        { "Cơ quản xuất bản", "" }
                    },
                    Source = WorkSource.NguoiDungKeKhai,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"), // ID của Bài báo khoa học
                    WorkLevelId = null,
                    AcademicYearId = project.AcademicYearId,
                    CreatedDate = DateTime.UtcNow
                };

                _logger.LogInformation("Đang tạo bài báo khoa học với ID: {ArticleId}", scientificArticle.Id);
                await _unitOfWork.Repository<Work>().CreateAsync(scientificArticle);

                // Tạo thông tin tác giả
                var author = new Author
                {
                    Id = Guid.NewGuid(),
                    WorkId = scientificArticle.Id,
                    UserId = userId,
                    AuthorRoleId = null,
                    PurposeId = Guid.Parse("5cf30509-8632-4d62-ad14-55949b9b9336"), // ID của mục đích "Sản phẩm của đề tài NCKH"
                    Position = 1,
                    ScoreLevel = null,
                    WorkHour = 0,
                    SCImagoFieldId = null,
                    FieldId = null,
                    ProofStatus = ProofStatus.ChuaXuLy,
                    CreatedDate = DateTime.UtcNow
                };

                _logger.LogInformation("Đang tạo thông tin tác giả cho bài báo với ID: {AuthorId}", author.Id);
                await _unitOfWork.Repository<Author>().CreateAsync(author);

                // Thêm tác giả vào WorkAuthor
                var workAuthor = new WorkAuthor
                {
                    Id = Guid.NewGuid(),
                    WorkId = scientificArticle.Id,
                    UserId = userId
                };
                _logger.LogInformation("Đang tạo WorkAuthor với ID: {WorkAuthorId}", workAuthor.Id);
                await _unitOfWork.Repository<WorkAuthor>().CreateAsync(workAuthor);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Đã tạo bài báo khoa học tự động từ đề tài {ProjectId} thành công", project.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo bài báo khoa học từ đề tài {ProjectId}: {ErrorMessage}", project.Id, ex.Message);
                throw;
            }
        }
    }
}