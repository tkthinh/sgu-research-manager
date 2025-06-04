using Application.Shared.Messages;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Globalization;
using Application.Shared.Services;
using Application.AcademicYears;

namespace Application.Works
{
    public class WorkImportService : IWorkImportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkCalculateService _workCalculateService;
        private readonly IAcademicYearService _academicYearService;

        public WorkImportService(
            IUnitOfWork unitOfWork,
            IWorkCalculateService workCalculateService,
            IAcademicYearService academicYearService)
        {
            _unitOfWork = unitOfWork;
            _workCalculateService = workCalculateService;
            _academicYearService = academicYearService;
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
                            TimePublished = ParseMonthYear(worksheet.Cells[row, 7].Text.Trim()),
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

            // Lấy năm học hiện tại
            var currentAcademicYear = await _academicYearService.GetCurrentAcademicYearAsync();
            if (currentAcademicYear == null)
            {
                throw new Exception("Không tìm thấy năm học hiện tại");
            }

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
                DateOnly? timePublished = null;
                if (dto.TimePublished.HasValue)
                {
                    timePublished = new DateOnly(dto.TimePublished.Value.Year, dto.TimePublished.Value.Month, 1);
                }

                var existingWork = await workRepo.FirstOrDefaultAsync(w =>
                    w.Title.ToLower() == dto.Title.ToLower() &&
                    w.WorkTypeId == workType.Id &&
                    (workLevelId == null ? w.WorkLevelId == null : w.WorkLevelId == workLevelId) &&
                    (timePublished == null ? w.TimePublished == null :
                     w.TimePublished.Value.Year == timePublished.Value.Year &&
                     w.TimePublished.Value.Month == timePublished.Value.Month)
                );


                Work work;
                if (existingWork is null)
                {
                    // => Chưa có công trình này, tạo mới
                    work = new Work
                    {
                        Title = dto.Title,
                        TimePublished = timePublished,
                        TotalAuthors = dto.TotalAuthors,
                        TotalMainAuthors = dto.TotalMainAuthors,
                        Details = dto.Details,
                        WorkTypeId = workType.Id,
                        WorkLevelId = workLevel?.Id,
                        Source = WorkSource.QuanLyNhap,
                        AcademicYearId = currentAcademicYear.Id
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

        private DateOnly? ParseMonthYear(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            // Thử parse các định dạng khác nhau
            string[] formats = {
                "MM/yyyy", "M/yyyy", "yyyy-MM", "MM-yyyy", "M-yyyy",
                "d/M/yyyy", "dd/MM/yyyy", "d-M-yyyy", "dd-MM-yyyy",
                "yyyy/MM/dd", "yyyy-MM-dd"
            };

            foreach (var format in formats)
            {
                if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                {
                    // Luôn trả về ngày 1 của tháng đó
                    return new DateOnly(result.Year, result.Month, 1);
                }
            }

            // Thử parse bằng DateTime.Parse nếu các format trên không khớp
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return new DateOnly(parsedDate.Year, parsedDate.Month, 1);
            }

            return null;
        }

        // Sử dụng phương thức từ WorkCalculateService để tính toán
        private int CalculateWorkHour(ScoreLevel? scoreLevel, Factor factor)
        {
            return _workCalculateService.CalculateWorkHour(scoreLevel, factor);
        }

        private async Task<decimal> CalculateAuthorHour(int workHour, int totalAuthors, int totalMainAuthors, Guid? authorRoleId, CancellationToken cancellationToken = default)
        {
            return await _workCalculateService.CalculateAuthorHour(workHour, totalAuthors, totalMainAuthors, authorRoleId, cancellationToken);
        }
    }
}
