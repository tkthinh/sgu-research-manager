using Application.Authors;
using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Domain.Enums;
using Application.SystemConfigs;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Globalization;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Works
{
    public class WorkService : GenericCachedService<WorkDto, Work>, IWorkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper<WorkDto, Work> _mapper;
        private readonly IGenericMapper<AuthorDto, Author> _authorMapper;
        private readonly IGenericRepository<AuthorRole> _authorRoleRepository;
        private readonly IGenericRepository<Factor> _factorRepository;
        private readonly IWorkRepository _workRepository;
        private readonly ISystemConfigService _systemConfigService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WorkService(
            IUnitOfWork unitOfWork,
            IGenericMapper<WorkDto, Work> mapper,
            IGenericMapper<AuthorDto, Author> authorMapper,
            IDistributedCache cache,
            ILogger<WorkService> logger,
            IWorkRepository workRepository,
            ISystemConfigService systemConfigService,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, mapper, cache, logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authorMapper = authorMapper;
            _authorRoleRepository = unitOfWork.Repository<AuthorRole>();
            _factorRepository = unitOfWork.Repository<Factor>();
            _workRepository = workRepository;
            _systemConfigService = systemConfigService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<WorkDto>> GetAllWorksWithAuthorsAsync(CancellationToken cancellationToken = default)
        {
            var works = await _workRepository.GetWorksWithAuthorsAsync();
            var workDtos = _mapper.MapToDtos(works).ToList();

            await FillCoAuthorUserIdsForMultipleWorksAsync(workDtos, cancellationToken);

            return workDtos;
        }

        public async Task<WorkDto?> GetWorkByIdWithAuthorsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(id);
            if (work is null)
                return null;

            var dto = _mapper.MapToDto(work);
            await FillCoAuthorUserIdsAsync(dto, cancellationToken); // Điền CoAuthorUserIds
            return dto;
        }

        public async Task<WorkDto> CreateWorkWithAuthorAsync(CreateWorkRequestDto request, CancellationToken cancellationToken = default)
        {
            // Kiểm tra trạng thái hệ thống
            if (!await _systemConfigService.IsSystemOpenAsync(cancellationToken))
                throw new Exception(ErrorMessages.SystemClosedNewWork);

            var existingWork = await _unitOfWork.Repository<Work>()
                .FirstOrDefaultAsync(w => w.Title == request.Title, cancellationToken);
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
                CreatedDate = DateTime.UtcNow
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

            var workHour = CalculateWorkHour(request.Author.ScoreLevel, factor);

            await _unitOfWork.Repository<Work>().CreateAsync(work);

            // Lấy UserId
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
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

            var authorHour = await CalculateAuthorHour(workHour, request.TotalAuthors ?? 0,
                request.TotalMainAuthors ?? 0, request.Author.AuthorRoleId);
            author.AuthorHour = authorHour;
            author.MarkedForScoring = false;

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

            return workDto;
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var authors = await _unitOfWork.Repository<Author>()
                .FindAsync(a => a.UserId == userId);

            if (!authors.Any())
            {
                return Enumerable.Empty<WorkDto>();
            }

            var workIds = authors.Select(a => a.WorkId).Distinct();
            var works = await _workRepository.GetWorksWithAuthorsByIdsAsync(workIds, cancellationToken);

            var filteredWorks = works.Select(work =>
            {
                var userAuthor = authors.FirstOrDefault(a => a.WorkId == work.Id);
                if (userAuthor is null)
               {
                  return null;
               }
                
                    var filteredWork = _mapper.MapToDto(work);
                    filteredWork.Authors = new List<AuthorDto> { _authorMapper.MapToDto(userAuthor) };
                    return filteredWork;
            }).Where(w => w is not null).ToList();

            await FillCoAuthorUserIdsForMultipleWorksAsync(filteredWorks!, cancellationToken);

            return filteredWorks!;
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            var works = await _workRepository.GetWorksByDepartmentIdAsync(departmentId, cancellationToken);
            return _mapper.MapToDtos(works);
        }

        public async Task SetMarkedForScoringAsync(Guid authorId, bool marked, CancellationToken cancellationToken = default)
        {
            // Kiểm tra trạng thái hệ thống
            if (!await _systemConfigService.IsSystemOpenAsync(cancellationToken))
                throw new Exception(ErrorMessages.SystemClosedMarkingWork);

            // Lấy thông tin tác giả
            var author = await _unitOfWork.Repository<Author>().GetByIdAsync(authorId);
            if (author is null)
                throw new Exception(ErrorMessages.AuthorNotFound);

            // Lấy thông tin công trình
            var work = await _unitOfWork.Repository<Work>().GetByIdAsync(author.WorkId);
            if (work is null)
                throw new Exception(ErrorMessages.WorkNotFound);

            // Nếu đánh dấu công trình, kiểm tra giới hạn
            if (marked)
            {
                // Thay vì thực hiện song song, chúng ta sẽ thực hiện tuần tự để tránh xung đột DbContext
                
                // 1. Lấy Factor để xác định MaxAllowed
                var factors = await _factorRepository.FindAsync(f =>
                    f.WorkTypeId == work.WorkTypeId &&
                    f.WorkLevelId == work.WorkLevelId &&
                    f.ScoreLevel == author.ScoreLevel);
                var factor = factors.FirstOrDefault();
                
                // 2. Đếm số lượng công trình đã được đánh dấu
                var markedAuthors = await _unitOfWork.Repository<Author>().FindAsync(a =>
                    a.UserId == author.UserId &&
                                       a.MarkedForScoring &&
                                       a.ScoreLevel == author.ScoreLevel);

                // 3. Lấy thông tin WorkType (nếu cần)
                    var workType = await _unitOfWork.Repository<WorkType>().GetByIdAsync(work.WorkTypeId);
                
                // 4. Lấy thông tin WorkLevel (nếu cần)
                WorkLevel? workLevel = null;
                if (work.WorkLevelId.HasValue)
                {
                    workLevel = await _unitOfWork.Repository<WorkLevel>().GetByIdAsync(work.WorkLevelId.Value);
                }
                
                // Xác định MaxAllowed
                int maxAllowed = 1; // Giá trị mặc định
                if (factor is not null && factor.MaxAllowed.HasValue)
                {
                    maxAllowed = factor.MaxAllowed.Value;
                }

                // Kiểm tra giới hạn
                if (markedAuthors.Count() >= maxAllowed)
                {
                    var workTypeInfo = workType?.Name ?? "Không xác định";
                    var workLevelInfo = workLevel?.Name ?? "Không xác định";
                    var scoreLevelInfo = author.ScoreLevel.HasValue ? author.ScoreLevel.ToString() : "Không xác định";

                    throw new Exception(string.Format(ErrorMessages.ScoreLimitExceeded, maxAllowed, workTypeInfo, workLevelInfo, scoreLevelInfo));
                }
            }

            // Cập nhật trạng thái đánh dấu
            author.MarkedForScoring = marked;
            author.ModifiedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<Author>().UpdateAsync(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SafeInvalidateCacheAsync(author.WorkId);
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
                UpdateWorkDetailsByAdmin(work, request.WorkRequest);
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

        private void UpdateWorkDetailsByAdmin(Work work, UpdateWorkRequestDto workRequest)
        {
            work.Title = workRequest.Title ?? work.Title;
            work.TimePublished = workRequest.TimePublished ?? work.TimePublished;
            work.TotalAuthors = workRequest.TotalAuthors ?? work.TotalAuthors;
            work.TotalMainAuthors = workRequest.TotalMainAuthors ?? work.TotalMainAuthors;
            work.Details = workRequest.Details ?? work.Details;
            work.Source = workRequest.Source;
            work.WorkTypeId = workRequest.WorkTypeId ?? work.WorkTypeId;
            work.WorkLevelId = workRequest.WorkLevelId ?? work.WorkLevelId;
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
            if (ShouldRecalculateAuthorHours(authorRequest, workRequest))
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

        private bool ShouldRecalculateAuthorHours(UpdateAuthorRequestDto authorRequest, UpdateWorkRequestDto? workRequest)
        {
            return authorRequest.ScoreLevel.HasValue ||
                (workRequest is not null && (workRequest.TotalAuthors.HasValue || workRequest.TotalMainAuthors.HasValue));
        }

        private async Task UpdateAuthorHoursAsync(
            Author author,
            Work work,
            ScoreLevel? newScoreLevel,
            UpdateWorkRequestDto? workRequest,
            CancellationToken cancellationToken)
        {
            var effectiveScoreLevel = newScoreLevel ?? author.ScoreLevel;

            // Sử dụng phương thức mới để tìm factor
            var factor = await FindFactorAsync(
                work.WorkTypeId,
                work.WorkLevelId,
                author.PurposeId,
                author.AuthorRoleId,
                effectiveScoreLevel,
                cancellationToken);

            if (factor is null)
                throw new Exception(ErrorMessages.FactorNotFound);

            author.WorkHour = CalculateWorkHour(effectiveScoreLevel, factor);
            author.AuthorHour = await CalculateAuthorHour(
                author.WorkHour,
                workRequest?.TotalAuthors ?? work.TotalAuthors ?? 0,
                workRequest?.TotalMainAuthors ?? work.TotalMainAuthors ?? 0,
                author.AuthorRoleId);
        }

        private void UpdateAuthorProofStatus(Author author, UpdateAuthorRequestDto authorRequest)
        {
            if (authorRequest.ProofStatus.HasValue)
            {
                author.ProofStatus = authorRequest.ProofStatus.Value;
                author.Note = authorRequest.Note ?? author.Note;
            }
        }

        public async Task<WorkDto> UpdateWorkByAuthorAsync(Guid workId, UpdateWorkWithAuthorRequestDto request, Guid userId, CancellationToken cancellationToken = default)
        {
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(workId);
            if (work is null)
                throw new Exception(ErrorMessages.WorkNotFound);

            logger.LogInformation("UpdateWorkByAuthorAsync - Received request for workId {WorkId}", workId);
            
            // Xử lý đặc biệt cho coAuthorUserIds - kiểm tra cả hai vị trí
            List<Guid> coAuthorUserIds = new List<Guid>();
            
            // Xác định coAuthorUserIds từ vị trí đúng và vị trí có thể sai
            if (request.WorkRequest?.CoAuthorUserIds != null && request.WorkRequest.CoAuthorUserIds.Any()) 
            {
                // Vị trí 1: Trong workRequest (cấu trúc chính xác)
                coAuthorUserIds.AddRange(request.WorkRequest.CoAuthorUserIds);
                logger.LogInformation("Found CoAuthorUserIds in WorkRequest: {Count} ids - {CoAuthorUserIds}", 
                    request.WorkRequest.CoAuthorUserIds.Count, 
                    string.Join(", ", request.WorkRequest.CoAuthorUserIds));
            }
            
            // Kiểm tra xem có thuộc tính coAuthorUserIds ở gốc của request không (vị trí sai)
            try 
            {
                var property = request.GetType().GetProperty("CoAuthorUserIds") ?? 
                              request.GetType().GetProperty("coAuthorUserIds");
                
                if (property != null)
                {
                    var value = property.GetValue(request);
                    if (value is List<Guid> userIds && userIds.Any())
                    {
                        coAuthorUserIds.AddRange(userIds);
                        logger.LogInformation("Found CoAuthorUserIds at root level: {Count} ids - {CoAuthorUserIds}", 
                            userIds.Count, string.Join(", ", userIds));
                    }
                    else if (value is IEnumerable<Guid> userIdsEnum)
                    {
                        var userIdsList = userIdsEnum.ToList();
                        if (userIdsList.Any())
                        {
                            coAuthorUserIds.AddRange(userIdsList);
                            logger.LogInformation("Found CoAuthorUserIds as IEnumerable at root level: {Count} ids - {CoAuthorUserIds}", 
                                userIdsList.Count, string.Join(", ", userIdsList));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking for root-level CoAuthorUserIds");
            }
            
            // Loại bỏ trùng lặp từ danh sách coAuthorUserIds
            coAuthorUserIds = coAuthorUserIds.Distinct().ToList();
            logger.LogInformation("Final combined CoAuthorUserIds: {Count} ids - {CoAuthorUserIds}", 
                coAuthorUserIds.Count, string.Join(", ", coAuthorUserIds));

            await ValidateSystemStateAsync(work, userId, cancellationToken);

            if (request.WorkRequest is not null)
            {
                UpdateWorkDetailsByAuthor(work, request.WorkRequest);

                // Luôn gọi UpdateCoAuthorsAsync để cập nhật đúng danh sách đồng tác giả
                // Ngay cả khi danh sách rỗng, việc này cũng đảm bảo xóa các đồng tác giả cũ nếu người dùng muốn
                logger.LogInformation("Updating CoAuthors with combined list: {Count} userIds - {UserIds}", 
                    coAuthorUserIds.Count, string.Join(", ", coAuthorUserIds));
                await UpdateCoAuthorsAsync(work, coAuthorUserIds, userId, cancellationToken);
            }

            var author = await GetOrCreateAuthorAsync(work, userId, request, cancellationToken);

            // Lưu các thay đổi vào database
            await SaveChangesAsync(work, author, cancellationToken);
            
            logger.LogInformation("Changes saved successfully for workId {WorkId}", workId);

            // Ánh xạ sang DTO và điền thông tin đồng tác giả
            var workDto = _mapper.MapToDto(work);
            
            logger.LogInformation("WorkDto mapped. Filling CoAuthorUserIds...");
            await FillCoAuthorUserIdsAsync(workDto, cancellationToken);
            
            logger.LogInformation("UpdateWorkByAuthorAsync complete. Returning CoAuthorUserIds: {CoAuthorUserIds}", 
                string.Join(", ", workDto.CoAuthorUserIds));
                
            return workDto;
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
                        logger.LogError(ex, "Error adding coauthor {UserId} to work {WorkId}", coAuthorId, work.Id);
                    }
                }
                
                logger.LogInformation("UpdateCoAuthorsAsync completed for work {WorkId}", work.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in UpdateCoAuthorsAsync for work {WorkId}", work.Id);
            }
        }

        private async Task ValidateSystemStateAsync(Work work, Guid userId, CancellationToken cancellationToken)
        {
            var currentAuthor = await _unitOfWork.Repository<Author>()
                .FirstOrDefaultAsync(a => a.WorkId == work.Id && a.UserId == userId, cancellationToken);

            if (!await _systemConfigService.IsSystemOpenAsync(cancellationToken))
            {
                if (currentAuthor is null || currentAuthor.ProofStatus != ProofStatus.KhongHopLe)
                    throw new Exception(ErrorMessages.SystemClosedEditWork);
            }
        }

        private void UpdateWorkDetailsByAuthor(Work work, UpdateWorkRequestDto workRequest)
        {
            work.Title = workRequest.Title ?? work.Title;
            work.TimePublished = workRequest.TimePublished ?? work.TimePublished;
            work.TotalAuthors = workRequest.TotalAuthors ?? work.TotalAuthors;
            work.TotalMainAuthors = workRequest.TotalMainAuthors ?? work.TotalMainAuthors;
            work.Details = workRequest.Details ?? work.Details;
            work.Source = WorkSource.NguoiDungKeKhai; // Luôn đặt nguồn là NguoiDungKeKhai
            work.WorkTypeId = workRequest.WorkTypeId ?? work.WorkTypeId;
            work.WorkLevelId = workRequest.WorkLevelId ?? work.WorkLevelId;
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

            var workHour = CalculateWorkHour(request.AuthorRequest.ScoreLevel, factor);
            var authorHour = await CalculateAuthorHour(
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
            // Tìm kiếm chính xác theo tất cả điều kiện
            var factors = await _factorRepository.FindAsync(f =>
                f.WorkTypeId == workTypeId &&
                // Xử lý WorkLevelId null
                ((workLevelId == null && f.WorkLevelId == null) || (workLevelId != null && f.WorkLevelId == workLevelId)) &&
                f.PurposeId == purposeId &&
                // Xử lý AuthorRoleId null
                ((authorRoleId == null && f.AuthorRoleId == null) || (authorRoleId != null && f.AuthorRoleId == authorRoleId)) &&
                // Xử lý ScoreLevel null
                ((scoreLevel == null && f.ScoreLevel == null) || (scoreLevel != null && f.ScoreLevel == scoreLevel)));

            var factor = factors.FirstOrDefault();

            // Nếu không tìm thấy và authorRoleId có giá trị, thử tìm với authorRoleId = null
            if (factor == null && authorRoleId.HasValue)
            {
                factors = await _factorRepository.FindAsync(f =>
                    f.WorkTypeId == workTypeId &&
                    ((workLevelId == null && f.WorkLevelId == null) || (workLevelId != null && f.WorkLevelId == workLevelId)) &&
                    f.PurposeId == purposeId &&
                    f.AuthorRoleId == null &&
                    ((scoreLevel == null && f.ScoreLevel == null) || (scoreLevel != null && f.ScoreLevel == scoreLevel)));

                factor = factors.FirstOrDefault();
            }

            // Log thông tin debug
            if (factor != null)
            {
                logger.LogInformation(
                    "Tìm thấy Factor với WorkTypeId={WorkTypeId}, WorkLevelId={WorkLevelId}, PurposeId={PurposeId}, AuthorRoleId={AuthorRoleId}, ScoreLevel={ScoreLevel}. ConvertHour={ConvertHour}",
                    workTypeId, workLevelId, purposeId, authorRoleId, scoreLevel, factor.ConvertHour);
            }
            else
            {
                logger.LogWarning(
                    "Không tìm thấy Factor cho WorkTypeId={WorkTypeId}, WorkLevelId={WorkLevelId}, PurposeId={PurposeId}, AuthorRoleId={AuthorRoleId}, ScoreLevel={ScoreLevel}",
                    workTypeId, workLevelId, purposeId, authorRoleId, scoreLevel);
            }

            return factor;
        }

        private async Task UpdateExistingAuthorAsync(Author author, Work work, UpdateWorkWithAuthorRequestDto request, CancellationToken cancellationToken)
        {
            // Cập nhật thông tin tác giả
            if (request.AuthorRequest is not null)
            {
                // AuthorRoleId và PurposeId là Guid không nullable, gán trực tiếp
                author.AuthorRoleId = request.AuthorRequest.AuthorRoleId;
                author.PurposeId = request.AuthorRequest.PurposeId ?? Guid.Empty;

                author.Position = request.AuthorRequest.Position ?? author.Position;
                author.ScoreLevel = request.AuthorRequest.ScoreLevel ?? author.ScoreLevel;

                // SCImagoFieldId và FieldId là nullable Guid
                author.SCImagoFieldId = request.AuthorRequest.SCImagoFieldId;
                author.FieldId = request.AuthorRequest.FieldId;

                // Tính lại WorkHour và AuthorHour nếu có thay đổi liên quan
                bool shouldRecalculate = request.AuthorRequest.ScoreLevel.HasValue ||
                    (request.WorkRequest is not null && (request.WorkRequest.TotalAuthors.HasValue || request.WorkRequest.TotalMainAuthors.HasValue));

                if (shouldRecalculate)
                {
                    var factor = await FindFactorAsync(
                        work.WorkTypeId,
                        work.WorkLevelId,
                        author.PurposeId,
                        author.AuthorRoleId,
                        author.ScoreLevel,
                        cancellationToken);

                    if (factor is null)
                        throw new Exception(ErrorMessages.FactorNotFound);

                    author.WorkHour = CalculateWorkHour(author.ScoreLevel, factor);
                    author.AuthorHour = await CalculateAuthorHour(
                        author.WorkHour,
                        work.TotalAuthors ?? 0,
                        work.TotalMainAuthors ?? 0,
                        author.AuthorRoleId);
                }

                author.ModifiedDate = DateTime.UtcNow;
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

        public async Task<IEnumerable<WorkDto>> GetWorksByCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            // Lấy danh sách các WorkId mà user là tác giả (có trong bảng Author)
            var authors = await _unitOfWork.Repository<Author>()
                .FindAsync(a => a.UserId == userId);

            var authorWorkIds = authors.Select(a => a.WorkId).Distinct().ToList();

            // Lấy danh sách các WorkId mà user được thêm làm đồng tác giả (có trong bảng WorkAuthor)
            var workAuthors = await _unitOfWork.Repository<WorkAuthor>()
                .FindAsync(wa => wa.UserId == userId);

            var workAuthorWorkIds = workAuthors.Select(wa => wa.WorkId).Distinct().ToList();

            // Kết hợp tất cả WorkId (loại bỏ trùng lặp)
            var allWorkIds = authorWorkIds.Union(workAuthorWorkIds).Distinct().ToList();

            if (!allWorkIds.Any())
            {
                return Enumerable.Empty<WorkDto>();
            }

            // Lấy thông tin đầy đủ của các công trình
            var works = await _workRepository.GetWorksWithAuthorsByIdsAsync(allWorkIds, cancellationToken);

            // Lọc và xử lý dữ liệu
            var filteredWorks = works.Select(work =>
            {
                // Kiểm tra xem user đã có thông tin tác giả trong công trình này chưa
                var userAuthor = authors.FirstOrDefault(a => a.WorkId == work.Id);
                var isCoAuthor = workAuthorWorkIds.Contains(work.Id);

                // Chỉ giữ thông tin tác giả của user hiện tại (nếu có)
                var filteredWork = _mapper.MapToDto(work);
                filteredWork.Authors = userAuthor != null
                    ? new List<AuthorDto> { _authorMapper.MapToDto(userAuthor) }
                    : new List<AuthorDto>();

                return filteredWork;
            }).Where(w => w != null).ToList();

            await FillCoAuthorUserIdsForMultipleWorksAsync(filteredWorks, cancellationToken);

            return filteredWorks;
        }

        private int CalculateWorkHour(ScoreLevel? scoreLevel, Factor factor)
        {
            // Nếu factor không hợp lệ
            if (factor is null)
            {
                logger.LogWarning("CalculateWorkHour - Factor is null, returning 0");
                return 0;
            }

            // Kiểm tra ScoreLevel chỉ khi nó có giá trị và factor.ScoreLevel cũng có giá trị
            if (scoreLevel.HasValue && factor.ScoreLevel.HasValue)
            {
                // So sánh chỉ khi cả hai đều có giá trị
                if (scoreLevel.Value != factor.ScoreLevel.Value)
                {
                    logger.LogWarning("CalculateWorkHour - ScoreLevel mismatch: Request={RequestScoreLevel}, Factor={FactorScoreLevel}, returning 0",
                        scoreLevel, factor.ScoreLevel);
                    return 0;
                }
            }
            else if ((scoreLevel.HasValue && !factor.ScoreLevel.HasValue) ||
                     (!scoreLevel.HasValue && factor.ScoreLevel.HasValue))
            {
                // Một trong hai có giá trị, một không có -> không khớp
                logger.LogWarning("CalculateWorkHour - ScoreLevel nullability mismatch: Request={RequestScoreLevel}, Factor={FactorScoreLevel}, returning 0",
                    scoreLevel.HasValue ? scoreLevel.ToString() : "null",
                    factor.ScoreLevel.HasValue ? factor.ScoreLevel.ToString() : "null");
                return 0;
            }

            // Cả hai đều null hoặc cả hai đều có giá trị và bằng nhau
            logger.LogInformation("CalculateWorkHour - Returning ConvertHour={ConvertHour}", factor.ConvertHour);
            return factor.ConvertHour;
        }

        private async Task<decimal> CalculateAuthorHour(int workHour, int totalAuthors, int totalMainAuthors, Guid? authorRoleId, CancellationToken cancellationToken = default)
        {
            // Hằng số tỷ lệ phân bổ giờ chuẩn
            const double MAIN_AUTHOR_RATIO = 1.0 / 3;
            const double REGULAR_RATIO = 2.0 / 3;
            
            // Kiểm tra các điều kiện không hợp lệ
            if (workHour <= 0 || totalAuthors <= 0 || totalMainAuthors <= 0 || totalMainAuthors > totalAuthors || authorRoleId == null)
            {
                return 0;
            }

            // Lấy thông tin vai trò tác giả
            var authorRole = await _authorRoleRepository.GetByIdAsync(authorRoleId.Value);
            if (authorRole is null)
            {
                throw new Exception(ErrorMessages.AuthorRoleNotFound);
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

        public async Task DeleteWorkAsync(Guid workId, Guid userId, CancellationToken cancellationToken = default)
        {
            var work = await _workRepository.GetWorkWithAuthorsByIdAsync(workId);
            if (work is null)
                throw new Exception(ErrorMessages.WorkNotFound);

            if (!await _systemConfigService.IsSystemOpenAsync(cancellationToken))
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

        private async Task FillCoAuthorUserIdsAsync(WorkDto workDto, CancellationToken cancellationToken = default)
        {
            if (workDto is null)
                return;

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            Guid currentUserId = Guid.Empty;
            if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userId))
            {
                currentUserId = userId;
            }

            logger.LogInformation("FillCoAuthorUserIdsAsync - WorkDto {WorkId}: Start", workDto.Id);

            try {
                // Lấy những UserId đã có thông tin Author
                var authors = await _unitOfWork.Repository<Author>()
                    .FindAsync(a => a.WorkId == workDto.Id);
                var authorUserIds = authors.Select(a => a.UserId).ToList();

                // Lấy những UserId trong bảng WorkAuthor
                var workAuthors = await _unitOfWork.Repository<WorkAuthor>()
                    .FindAsync(wa => wa.WorkId == workDto.Id);
                var coAuthorUserIds = workAuthors.Select(wa => wa.UserId).ToList();

                logger.LogInformation("FillCoAuthorUserIdsAsync - Authors found: {Count}", authors.Count());
                logger.LogInformation("FillCoAuthorUserIdsAsync - WorkAuthors found: {Count}", workAuthors.Count());
                logger.LogInformation("Authors UserIds: {AuthorIds}", string.Join(", ", authorUserIds));
                logger.LogInformation("WorkAuthors UserIds: {WorkAuthors}", string.Join(", ", coAuthorUserIds));

                // Kết hợp tất cả UserId, loại bỏ currentUserId, và đảm bảo không có trùng lặp
                var combinedUserIds = authorUserIds.Union(coAuthorUserIds)
                    .Where(uid => uid != currentUserId)
                    .Distinct()
                    .ToList();
                    
                workDto.CoAuthorUserIds = combinedUserIds;
                
                logger.LogInformation("Final CoAuthorUserIds: {Count} users - {FinalIds}", 
                    workDto.CoAuthorUserIds.Count,
                    string.Join(", ", workDto.CoAuthorUserIds));
            }
            catch (Exception ex) {
                logger.LogError(ex, "Error filling CoAuthorUserIds for WorkDto {WorkId}", workDto.Id);
                // Đảm bảo ít nhất trả về một danh sách rỗng thay vì null
                workDto.CoAuthorUserIds = new List<Guid>();
            }
        }

        private async Task FillCoAuthorUserIdsForMultipleWorksAsync(IEnumerable<WorkDto> workDtos, CancellationToken cancellationToken = default)
        {
            if (workDtos is null || !workDtos.Any())
                return;

            // Lấy userId của người đang đăng nhập
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            Guid currentUserId = Guid.Empty;
            if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userId))
            {
                currentUserId = userId;
            }

            // Lấy tất cả workIds
            var workIds = workDtos.Select(w => w.Id).Distinct().ToList();

            // Thực hiện 2 truy vấn cho tất cả công trình cùng lúc
            var allAuthors = await _unitOfWork.Repository<Author>()
                .FindAsync(a => workIds.Contains(a.WorkId));

            var allWorkAuthors = await _unitOfWork.Repository<WorkAuthor>()
                .FindAsync(wa => workIds.Contains(wa.WorkId));

            // Nhóm dữ liệu theo WorkId để tránh lọc lặp đi lặp lại
            var authorsByWorkId = allAuthors.GroupBy(a => a.WorkId)
                .ToDictionary(g => g.Key, g => g.Select(a => a.UserId).ToList());

            var workAuthorsByWorkId = allWorkAuthors.GroupBy(wa => wa.WorkId)
                .ToDictionary(g => g.Key, g => g.Select(wa => wa.UserId).ToList());

            // Điền dữ liệu cho từng WorkDto
            foreach (var workDto in workDtos)
            {
                // Lấy danh sách userIds của authors (nếu có)
                var authorUserIds = authorsByWorkId.TryGetValue(workDto.Id, out var authors) 
                    ? authors 
                    : new List<Guid>();

                // Lấy danh sách userIds của work authors (nếu có)
                var coAuthorUserIds = workAuthorsByWorkId.TryGetValue(workDto.Id, out var workAuthors) 
                    ? workAuthors 
                    : new List<Guid>();

                // Kết hợp, loại bỏ trùng lặp và loại bỏ userId của người dùng đang đăng nhập
                workDto.CoAuthorUserIds = authorUserIds.Union(coAuthorUserIds)
                    .Where(uid => uid != currentUserId)
                    .Distinct()
                    .ToList();
            }
        }

        public async Task<byte[]> ExportToExcelAsync(List<ExportExcelDto> exportData, CancellationToken cancellationToken = default)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Kê khai công trình");

                // Định dạng tiêu đề
                worksheet.Cells[1, 1].Value = "TRƯỜNG ĐẠI HỌC SÀI GÒN";
                worksheet.Cells[1, 1, 1, 10].Merge = true;
                worksheet.Cells[1, 12].Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                worksheet.Cells[1, 12, 1, 18].Merge = true;
                worksheet.Cells[2, 1].Value = "ĐƠN VỊ:";
                worksheet.Cells[2, 1, 2, 10].Merge = true;
                worksheet.Cells[2, 12].Value = "Độc lập - Tự do - Hạnh phúc";
                worksheet.Cells[2, 12, 2, 18].Merge = true;
                worksheet.Cells[3, 1].Value = "KÊ KHAI SẢN PHẨM, CÔNG TRÌNH CỦA HOẠT ĐỘNG KHOA HỌC CÔNG NGHỆ NĂM HỌC 2023-2024";
                worksheet.Cells[3, 1, 3, 18].Merge = true;

                // Phần I: Thông tin tác giả
                worksheet.Cells[5, 1].Value = "I. THÔNG TIN TÁC GIẢ";
                worksheet.Cells[5, 1, 5, 18].Merge = true;

                // Lấy thông tin cá nhân từ bản ghi đầu tiên (vì thông tin cá nhân giống nhau cho tất cả công trình)
                var userInfo = exportData.FirstOrDefault();
                if (userInfo is null)
                    throw new Exception(ErrorMessages.NoDataToExport);

                worksheet.Cells[6, 1].Value = "1. Họ và tên:";
                worksheet.Cells[6, 2].Value = userInfo.FullName;
                worksheet.Cells[6, 5].Value = "4. Học hàm/học vị:";
                worksheet.Cells[6, 6].Value = userInfo.AcademicTitle;
                worksheet.Cells[6, 9].Value = "7. Mã số viên chức:";
                worksheet.Cells[6, 10].Value = userInfo.UserName; // Dùng UserName làm mã số viên chức

                worksheet.Cells[7, 1].Value = "2. Ngành:";
                worksheet.Cells[7, 2].Value = userInfo.DepartmentName;
                worksheet.Cells[7, 5].Value = "5. Chuyên ngành:";
                //worksheet.Cells[7, 6].Value = userInfo.MajorName;
                worksheet.Cells[7, 9].Value = "8. Ngạch công chức:";
                worksheet.Cells[7, 10].Value = userInfo.OfficerRank;

                worksheet.Cells[8, 1].Value = "3. Số điện thoại:";
                //worksheet.Cells[8, 2].Value = userInfo.PhoneNumber;
                worksheet.Cells[8, 5].Value = "6. Email:";
                worksheet.Cells[8, 6].Value = userInfo.Email;

                // Phần II: Thông tin công trình
                worksheet.Cells[10, 1].Value = "II. THÔNG TIN CÔNG TRÌNH";
                worksheet.Cells[10, 1, 10, 18].Merge = true;

                // Tiêu đề bảng
                var headers = new[]
                {
            "STT", "Tên công trình", "Loại công trình", "Cấp công trình", "Thời điểm công bố/hoàn thành",
            "Tổng số tác giả", "Tổng số tác giả chính", "Vị trí của tác giả", "Vai trò tác giả", "Đồng tác giả",
            "Thông tin chi tiết của công trình", "Mục đích quy đổi", "Mức điểm của công trình", "Ngành theo SCImago",
            "Ngành tính điểm", "Giờ chuẩn quy đổi", "Trạng thái", "Ghi chú"
        };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[11, i + 1].Value = headers[i];
                }

                // Điền dữ liệu công trình
                for (int i = 0; i < exportData.Count; i++)
                {
                    var work = exportData[i];
                    worksheet.Cells[i + 12, 1].Value = i + 1;
                    worksheet.Cells[i + 12, 2].Value = work.Title;
                    worksheet.Cells[i + 12, 3].Value = work.WorkTypeName;
                    worksheet.Cells[i + 12, 4].Value = work.WorkLevelName;
                    worksheet.Cells[i + 12, 5].Value = work.TimePublished.HasValue ? work.TimePublished.Value.ToString("dd/MM/yyyy") : "Không xác định";
                    worksheet.Cells[i + 12, 6].Value = work.TotalAuthors;
                    worksheet.Cells[i + 12, 7].Value = work.TotalMainAuthors;
                    worksheet.Cells[i + 12, 8].Value = work.Position;
                    worksheet.Cells[i + 12, 9].Value = work.AuthorRoleName;
                    worksheet.Cells[i + 12, 10].Value = work.CoAuthorNames;
                    worksheet.Cells[i + 12, 11].Value = work.Details != null ? string.Join("; ", work.Details.Select(kv => $"{kv.Key}: {kv.Value}")) : "Không xác định";
                    worksheet.Cells[i + 12, 12].Value = work.PurposeName;
                    worksheet.Cells[i + 12, 13].Value = work.ScoreLevel;
                    worksheet.Cells[i + 12, 14].Value = work.SCImagoFieldName;
                    worksheet.Cells[i + 12, 15].Value = work.ScoringFieldName;
                    worksheet.Cells[i + 12, 16].Value = work.AuthorHour;
                    worksheet.Cells[i + 12, 17].Value = work.ProofStatus?.ToString();
                    worksheet.Cells[i + 12, 18].Value = work.Note;
                }

                // Phần III: Tổng số công trình
                int startRow = 12 + exportData.Count + 2;
                worksheet.Cells[startRow, 1].Value = "III. TỔNG SỐ CÔNG TRÌNH";
                worksheet.Cells[startRow, 1, startRow, 18].Merge = true;

                startRow += 1;
                worksheet.Cells[startRow, 1].Value = "Tôi xin chịu trách nhiệm về tính xác thực của những thông tin trong bản kê khai này.";
                worksheet.Cells[startRow, 1, startRow, 18].Merge = true;

                startRow += 1;
                worksheet.Cells[startRow, 1].Value = "TỔNG SỐ CÔNG TRÌNH";
                worksheet.Cells[startRow, 1, startRow, 18].Merge = true;

                startRow += 1;
                worksheet.Cells[startRow, 1].Value = "TT";
                worksheet.Cells[startRow, 2].Value = "Loại công trình";
                worksheet.Cells[startRow, 3].Value = "Số lượng";

                // Tính tổng số công trình theo loại
                var workTypeCounts = exportData
                    .GroupBy(w => w.WorkTypeName ?? "Khác")
                    .ToDictionary(g => g.Key, g => g.Count());

                var workTypes = new[] { "Bài báo khoa học", "Báo cáo khoa học", "Đề tài", "Giáo trình", "Sách", "Hội thảo, hội nghị", "Hướng dẫn SV NCKH", "Khác" };
                for (int i = 0; i < workTypes.Length; i++)
                {
                    worksheet.Cells[startRow + i + 1, 1].Value = i + 1;
                    worksheet.Cells[startRow + i + 1, 2].Value = workTypes[i];
                    worksheet.Cells[startRow + i + 1, 3].Value = workTypeCounts.ContainsKey(workTypes[i]) ? workTypeCounts[workTypes[i]] : 0;
                }

                startRow += workTypes.Length + 1;
                worksheet.Cells[startRow, 1].Value = "Tổng số sản phẩm:";
                worksheet.Cells[startRow, 2].Value = exportData.Count;

                // Định dạng cột
                worksheet.Cells.AutoFitColumns();

                // Trả về nội dung file Excel dưới dạng byte[]
                return package.GetAsByteArray();
            }
        }
        public async Task<List<ExportExcelDto>> GetExportExcelDataAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            // Lấy thông tin cá nhân của user
            var user = await _unitOfWork.Repository<User>()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user is null)
                throw new Exception(ErrorMessages.UserNotFound);

            // Lấy danh sách công trình của user
            var works = await GetWorksByCurrentUserAsync(userId, cancellationToken);

            // Lấy thông tin đồng tác giả
            var allCoAuthorUserIds = works.SelectMany(w => w.CoAuthorUserIds).Distinct().ToList();
            var coAuthors = await _unitOfWork.Repository<User>()
                .FindAsync(u => allCoAuthorUserIds.Contains(u.Id));

            // Ánh xạ dữ liệu vào ExportExcelDto
            var exportData = works.Select(w =>
            {
                var author = w.Authors?.FirstOrDefault();
                var coAuthorNames = string.Join(", ", w.CoAuthorUserIds
                    .Select(uid => coAuthors.FirstOrDefault(u => u.Id == uid)?.FullName ?? "Không xác định"));

                return new ExportExcelDto
                {
                    UserName = user.UserName ?? "Không xác định",
                    FullName = user.FullName ?? "Không xác định",
                    Email = user.Email ?? "Không xác định",
                    AcademicTitle = user.AcademicTitle.ToString() ?? "Không xác định", // Học hàm/học vị
                    OfficerRank = user.OfficerRank.ToString() ?? "Không xác định", // Ngạch công chức
                    DepartmentName = user.Department?.Name ?? "Không xác định",
                    FieldName = author?.FieldName ?? "Không xác định", // Ngành - Field
                    //MajorName = user.Major ?? "Không xác định", // Chuyên ngành
                    //PhoneNumber = user.PhoneNumber ?? "Không xác định",
                    Title = w.Title ?? "Không xác định",
                    WorkTypeName = w.WorkTypeName ?? "Không xác định",
                    WorkLevelName = w.WorkLevelName,
                    TimePublished = w.TimePublished,
                    TotalAuthors = w.TotalAuthors,
                    TotalMainAuthors = w.TotalMainAuthors,
                    Position = author?.Position,
                    AuthorRoleName = author?.AuthorRoleName,
                    CoAuthorUserIds = w.CoAuthorUserIds,
                    CoAuthorNames = coAuthorNames,
                    Details = w.Details ?? new Dictionary<string, string>(),
                    PurposeName = author?.PurposeName,
                    SCImagoFieldName = author?.SCImagoFieldName,
                    ScoringFieldName = author?.FieldName,
                    ScoreLevel = author?.ScoreLevel,
                    AuthorHour = (int?)author?.AuthorHour, // Chuyển đổi decimal sang int
                    ProofStatus = author?.ProofStatus,
                    Note = author?.Note
                };
            }).ToList();

            return exportData;
        }

        public async Task ImportAsync(IFormFile file)
        {
            if (file is null || file.Length == 0)
                throw new ArgumentException(ErrorMessages.InvalidFile);

            var importRows = new List<ImportExcelDto>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                // Cấp phép dùng EPPlus trong môi trường non-commercial
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.First();
                    int rowCount = worksheet.Dimension.Rows;

                    // Giả sử dòng đầu tiên là header
                    for (int row = 2; row <= rowCount; row++)
                    {
                        // Parse cột Details (giả sử cột 16)
                        string detailsText = worksheet.Cells[row, 16].Text.Trim();
                        Dictionary<string, string> details = new Dictionary<string, string>();
                        if (!string.IsNullOrEmpty(detailsText))
                        {
                            var lines = detailsText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var line in lines)
                            {
                                var parts = line.Split(new[] { ':' }, 2);
                                if (parts.Length == 2)
                                {
                                    string key = parts[0].Trim();
                                    string value = parts[1].Trim();
                                    if (!string.IsNullOrEmpty(key))
                                        details[key] = value;
                                }
                            }
                        }

                        var dto = new ImportExcelDto
                        {
                            Username = worksheet.Cells[row, 1].Text.Trim(),
                            FullName = worksheet.Cells[row, 2].Text.Trim(),
                            DepartmentName = worksheet.Cells[row, 3].Text.Trim(),
                            Title = worksheet.Cells[row, 4].Text.Trim(),
                            WorkTypeName = worksheet.Cells[row, 5].Text.Trim(),
                            WorkLevelName = worksheet.Cells[row, 6].Text.Trim(),
                            TimePublished = DateOnly.TryParseExact(worksheet.Cells[row, 7].Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date :
                           DateOnly.TryParseExact(worksheet.Cells[row, 7].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) ? date :
                           DateOnly.TryParseExact(worksheet.Cells[row, 7].Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) ? date : null,
                            TotalAuthors = int.TryParse(worksheet.Cells[row, 8].Text.Trim(), out var ta) ? ta : (int?)null,
                            TotalMainAuthors = int.TryParse(worksheet.Cells[row, 9].Text.Trim(), out var tma) ? tma : (int?)null,
                            Position = int.TryParse(worksheet.Cells[row, 10].Text.Trim(), out var pos) ? pos : (int?)null,
                            AuthorRoleName = worksheet.Cells[row, 11].Text.Trim(),
                            PurposeName = worksheet.Cells[row, 12].Text.Trim(),
                            SCImagoFieldName = worksheet.Cells[row, 13].Text.Trim(),
                            ScoringFieldName = worksheet.Cells[row, 14].Text.Trim(),
                            ScoreLevel = Enum.TryParse<ScoreLevel>(worksheet.Cells[row, 15].Text.Trim(), out var scoreLevel)
                                                ? scoreLevel
                                                : (ScoreLevel?)null,
                            Details = details
                        };

                        importRows.Add(dto);
                    }
                }
            }

            await ProcessImportRows(importRows);
        }

        private async Task ProcessImportRows(List<ImportExcelDto> importRows)
        {
            var userRepo = _unitOfWork.Repository<User>();
            var authorRepo = _unitOfWork.Repository<Author>();
            var authorRoleRepo = _unitOfWork.Repository<AuthorRole>();
            var purposeRepo = _unitOfWork.Repository<Purpose>();
            var scImagoRepo = _unitOfWork.Repository<SCImagoField>();
            var fieldRepo = _unitOfWork.Repository<Field>();
            var workRepo = _unitOfWork.Repository<Work>();
            var workTypeRepo = _unitOfWork.Repository<WorkType>();
            var workLevelRepo = _unitOfWork.Repository<WorkLevel>();

            foreach (var dto in importRows)
            {
                // 1) Tìm User
                var user = await userRepo.FirstOrDefaultAsync(u =>
                    u.UserName.ToLower() == dto.Username.ToLower());
                if (user is null) continue;

                // 2) Tìm WorkType, WorkLevel
                var workType = await workTypeRepo.FirstOrDefaultAsync(wt =>
                    wt.Name.ToLower() == dto.WorkTypeName.ToLower());
                if (workType is null) continue;

                WorkLevel? workLevel = null;
                if (!string.IsNullOrWhiteSpace(dto.WorkLevelName))
                {
                    workLevel = await workLevelRepo.FirstOrDefaultAsync(wl =>
                        wl.Name.ToLower() == dto.WorkLevelName.ToLower());
                }

                // 3) Tìm công trình dựa trên 4 trường: Title, WorkTypeId, WorkLevelId, TimePublished
                //    Nếu 4 trường này trùng nhau thì xem là cùng 1 công trình
                // Lấy Id nếu workLevel != null
                Guid? workLevelId = workLevel?.Id;
                DateOnly? timePublished = dto.TimePublished; // Có thể null

                var existingWork = await workRepo.FirstOrDefaultAsync(w =>
                    w.Title.ToLower() == dto.Title.ToLower() &&
                    w.WorkTypeId == workType.Id &&
                    // So sánh WorkLevelId
                    (workLevelId == null ? w.WorkLevelId == null : w.WorkLevelId == workLevelId) &&
                    // So sánh TimePublished
                    (timePublished == null ? w.TimePublished == null : w.TimePublished == timePublished)
                );


                Work work;
                if (existingWork is null)
                {
                    // => Chưa có công trình này, tạo mới
                    work = new Work
                    {
                        Title = dto.Title,
                        TimePublished = dto.TimePublished,
                        TotalAuthors = dto.TotalAuthors,
                        TotalMainAuthors = dto.TotalMainAuthors,
                        Details = dto.Details,
                        WorkTypeId = workType.Id,
                        WorkLevelId = workLevel?.Id,
                        Source = WorkSource.QuanLyNhap
                    };
                    await workRepo.CreateAsync(work);
                }
                else
                {
                    work = existingWork;
                }

                // 4) Tìm AuthorRole, Purpose, SCImagoField, ScoringField
                var authorRole = await authorRoleRepo.FirstOrDefaultAsync(ar =>
                    ar.Name.ToLower() == dto.AuthorRoleName.ToLower());
                if (authorRole is null) continue;

                var purpose = await purposeRepo.FirstOrDefaultAsync(p =>
                    p.Name.ToLower() == dto.PurposeName.ToLower());
                if (purpose is null) continue;

                SCImagoField? scImagoField = null;
                if (!string.IsNullOrWhiteSpace(dto.SCImagoFieldName))
                {
                    scImagoField = await scImagoRepo.FirstOrDefaultAsync(s =>
                        s.Name.ToLower() == dto.SCImagoFieldName.ToLower());
                }

                Field? field = null;
                if (!string.IsNullOrWhiteSpace(dto.ScoringFieldName))
                {
                    field = await fieldRepo.FirstOrDefaultAsync(sf =>
                        sf.Name.ToLower() == dto.ScoringFieldName.ToLower());
                }

                // 5) Kiểm tra Author đã tồn tại chưa (dựa trên UserId + WorkId)
                var existingAuthor = await authorRepo.FirstOrDefaultAsync(a =>
                    a.UserId == user.Id && a.WorkId == work.Id);
                if (existingAuthor is not null)
                {
                    // => Tác giả đã tồn tại, bỏ qua hoặc update
                    continue;
                }

                // 6) Tạo mới Author
                var newAuthor = new Author
                {
                    UserId = user.Id,
                    WorkId = work.Id,
                    AuthorRoleId = authorRole.Id,
                    PurposeId = purpose.Id,
                    SCImagoFieldId = scImagoField?.Id,
                    FieldId = field?.Id,
                    Position = dto.Position,
                    ScoreLevel = dto.ScoreLevel,
                    MarkedForScoring = false,
                    ProofStatus = ProofStatus.ChuaXuLy
                };

                // 7) Tính workHour, authorHour (nếu cần)
                var factorRepo = _unitOfWork.Repository<Factor>();
                var factor = await factorRepo.FirstOrDefaultAsync(f =>
                    f.WorkTypeId == work.WorkTypeId &&
                    f.WorkLevelId == work.WorkLevelId &&
                    f.PurposeId == purpose.Id &&
                    (f.AuthorRoleId == null || f.AuthorRoleId == authorRole.Id) &&
                    f.ScoreLevel == newAuthor.ScoreLevel);

                if (factor is not null)
                {
                    int workHour = CalculateWorkHour(newAuthor.ScoreLevel, factor);
                    newAuthor.WorkHour = workHour;

                    int totalAuthors = work.TotalAuthors ?? 0;
                    int totalMainAuthors = work.TotalMainAuthors ?? 0;
                    decimal authorHour = await CalculateAuthorHour(workHour, totalAuthors, totalMainAuthors, authorRole.Id);
                    newAuthor.AuthorHour = authorHour;
                }
                else
                {
                    newAuthor.WorkHour = 0;
                    newAuthor.AuthorHour = 0;
                }

                await authorRepo.CreateAsync(newAuthor);
            }

            // Cuối cùng, lưu tất cả thay đổi
            await _unitOfWork.SaveChangesAsync();
        }
    }
}