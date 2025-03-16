using Application.ExcelOperations.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;
using Application.Services;
using System.Globalization;

namespace Application.ExcelOperations
{
    public class ExcelService : IExcelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExcelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ImportAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File không hợp lệ");

            var importRows = new List<ExcelImportRowDto>();

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

                        var dto = new ExcelImportRowDto
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

        private async Task ProcessImportRows(List<ExcelImportRowDto> importRows)
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
                if (user == null) continue;

                // 2) Tìm WorkType, WorkLevel
                var workType = await workTypeRepo.FirstOrDefaultAsync(wt =>
                    wt.Name.ToLower() == dto.WorkTypeName.ToLower());
                if (workType == null) continue;

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
                if (existingWork == null)
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
                if (authorRole == null) continue;

                var purpose = await purposeRepo.FirstOrDefaultAsync(p =>
                    p.Name.ToLower() == dto.PurposeName.ToLower());
                if (purpose == null) continue;

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
                if (existingAuthor != null)
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

                if (factor != null)
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


        //private async Task ProcessImportRows(List<ExcelImportRowDto> importRows)
        //{
        //    // Lấy repository cho các bảng liên quan
        //    var userRepo = _unitOfWork.Repository<User>();
        //    var authorRepo = _unitOfWork.Repository<Author>();
        //    var authorRoleRepo = _unitOfWork.Repository<AuthorRole>();
        //    var purposeRepo = _unitOfWork.Repository<Purpose>();
        //    var scImagoRepo = _unitOfWork.Repository<SCImagoField>();
        //    var fieldRepo = _unitOfWork.Repository<Field>(); // Repository cho ScoringField (hoặc Field)
        //    var workRepo = _unitOfWork.Repository<Work>();
        //    var workTypeRepo = _unitOfWork.Repository<WorkType>();
        //    var workLevelRepo = _unitOfWork.Repository<WorkLevel>();
        //    // Nếu cần, bạn có thể lấy DepartmentRepo để xử lý DepartmentName

        //    foreach (var dto in importRows)
        //    {
        //        // 1) Tìm User đã có sẵn (Username chắc chắn tồn tại)
        //        var user = await userRepo.FirstOrDefaultAsync(u =>
        //            u.UserName.ToLower() == dto.Username.ToLower());
        //        if (user == null)
        //        {
        //            // Nếu không tìm thấy, bạn có thể skip hoặc throw error
        //            continue;
        //        }

        //        // 2) Tìm WorkType theo WorkTypeName
        //        var workType = await workTypeRepo.FirstOrDefaultAsync(wt =>
        //            wt.Name.ToLower() == dto.WorkTypeName.ToLower());
        //        if (workType == null)
        //        {
        //            // Nếu không tìm thấy, skip dòng này hoặc throw exception
        //            continue;
        //        }

        //        // 3) Tìm WorkLevel theo WorkLevelName
        //        WorkLevel? workLevel = null;
        //        if (!string.IsNullOrWhiteSpace(dto.WorkLevelName))
        //        {
        //            workLevel = await workLevelRepo.FirstOrDefaultAsync(wl =>
        //                wl.Name.ToLower() == dto.WorkLevelName.ToLower());
        //        }

        //        // 4) Kiểm tra xem Work đã tồn tại hay chưa (so sánh theo Title)
        //        var work = await workRepo.FirstOrDefaultAsync(w =>
        //            w.Title.ToLower() == dto.Title.ToLower());
        //        if (work == null)
        //        {
        //            // Tạo mới Work
        //            work = new Work
        //            {
        //                Title = dto.Title,
        //                TimePublished = dto.TimePublished, // Nếu giá trị trong Excel không đúng định dạng, sẽ là null
        //                TotalAuthors = dto.TotalAuthors,
        //                TotalMainAuthors = dto.TotalMainAuthors,
        //                Details = dto.Details,
        //                Source = WorkSource.QuanLyNhap, // Ví dụ: nguồn nhập từ quản lý nhập
        //                WorkTypeId = workType.Id,
        //                WorkLevelId = workLevel?.Id
        //            };
        //            await workRepo.CreateAsync(work);
        //        }

        //        // 5) Tìm AuthorRole theo AuthorRoleName
        //        var authorRole = await authorRoleRepo.FirstOrDefaultAsync(ar =>
        //            ar.Name.ToLower() == dto.AuthorRoleName.ToLower());
        //        if (authorRole == null)
        //        {
        //            // Nếu không tìm thấy, skip dòng này hoặc gán một default role
        //            continue;
        //        }

        //        // 6) Tìm Purpose theo PurposeName
        //        var purpose = await purposeRepo.FirstOrDefaultAsync(p =>
        //            p.Name.ToLower() == dto.PurposeName.ToLower());
        //        if (purpose == null)
        //        {
        //            // Nếu không tìm thấy, skip dòng này
        //            continue;
        //        }

        //        // 7) Tìm SCImagoField theo SCImagoFieldName (nếu có)
        //        SCImagoField? scImagoField = null;
        //        if (!string.IsNullOrWhiteSpace(dto.SCImagoFieldName))
        //        {
        //            scImagoField = await scImagoRepo.FirstOrDefaultAsync(s =>
        //                s.Name.ToLower() == dto.SCImagoFieldName.ToLower());
        //        }

        //        // 8) Tìm ScoringField theo ScoringFieldName (nếu có)
        //        Field? field = null;
        //        if (!string.IsNullOrWhiteSpace(dto.ScoringFieldName))
        //        {
        //            field = await fieldRepo.FirstOrDefaultAsync(sf =>
        //                sf.Name.ToLower() == dto.ScoringFieldName.ToLower());
        //        }

        //        // 9) Kiểm tra xem Author đã tồn tại chưa (dựa trên User và Work)
        //        var existingAuthor = await authorRepo.FirstOrDefaultAsync(a =>
        //            a.UserId == user.Id && a.WorkId == work.Id);
        //        if (existingAuthor == null)
        //        {
        //            // Tạo mới Author
        //            var newAuthor = new Author
        //            {
        //                UserId = user.Id,
        //                WorkId = work.Id,
        //                AuthorRoleId = authorRole.Id,
        //                PurposeId = purpose.Id,
        //                SCImagoFieldId = scImagoField?.Id,
        //                ScoringFieldId = field?.Id,
        //                Position = dto.Position,
        //                ScoreLevel = dto.ScoreLevel,
        //                MarkedForScoring = false,
        //                ProofStatus = ProofStatus.ChuaXuLy
        //            };

        //            // 10) Nếu muốn tính toán workHour và authorHour dựa trên bảng Factor
        //            // Tra cứu Factor phù hợp dựa trên WorkTypeId, WorkLevelId, PurposeId, (AuthorRoleId nếu có) và ScoreLevel
        //            var factorRepo = _unitOfWork.Repository<Factor>();
        //            var factor = await factorRepo.FirstOrDefaultAsync(f =>
        //                f.WorkTypeId == work.WorkTypeId &&
        //                f.WorkLevelId == work.WorkLevelId &&
        //                f.PurposeId == purpose.Id &&
        //                (f.AuthorRoleId == null || f.AuthorRoleId == authorRole.Id) &&
        //                f.ScoreLevel == newAuthor.ScoreLevel);

        //            if (factor != null)
        //            {
        //                // Tính workHour: nếu ScoreLevel của Author khớp với Factor, lấy ConvertHour; nếu không thì 0.
        //                int workHour = CalculateWorkHour(newAuthor.ScoreLevel, factor);
        //                newAuthor.WorkHour = workHour;

        //                // Lấy tổng số tác giả và số tác giả chính từ Work (đã được map từ Excel)
        //                int totalAuthors = work.TotalAuthors ?? 0;
        //                int totalMainAuthors = work.TotalMainAuthors ?? 0;
        //                // Tính authorHour dựa vào workHour, tổng số tác giả, số tác giả chính và vai trò tác giả
        //                decimal authorHour = await CalculateAuthorHour(workHour, totalAuthors, totalMainAuthors, authorRole.Id);
        //                newAuthor.AuthorHour = authorHour;
        //            }
        //            else
        //            {
        //                newAuthor.WorkHour = 0;
        //                newAuthor.AuthorHour = 0;
        //            }

        //            await authorRepo.CreateAsync(newAuthor);
        //        }
        //    }

        //    // Sau khi xử lý hết các dòng, lưu tất cả thay đổi vào cơ sở dữ liệu.
        //    await _unitOfWork.SaveChangesAsync();
        //}

        public int CalculateWorkHour(ScoreLevel? scoreLevel, Factor factor)
        {
            return scoreLevel == factor.ScoreLevel ? factor.ConvertHour : 0;
        }

        public async Task<decimal> CalculateAuthorHour(int workHour, int totalAuthors, int totalMainAuthors, Guid authorRoleId, CancellationToken cancellationToken = default)
        {
            if (totalAuthors == 0 || totalMainAuthors == 0)
                return 0;

            var authorRole = await _unitOfWork.Repository<AuthorRole>().GetByIdAsync(authorRoleId);
            if (authorRole == null)
                throw new Exception("Không tìm thấy vai trò tác giả");

            decimal authorHour;
            if (authorRole.IsMainAuthor)
            {
                authorHour = (decimal)((1.0 / 3) * (workHour / (double)totalMainAuthors) +
                                      (2.0 / 3) * (workHour / (double)totalAuthors));
            }
            else
            {
                authorHour = (decimal)((2.0 / 3) * (workHour / (double)totalAuthors));
            }

            return Math.Round(authorHour, 1);
        }
    }
}
