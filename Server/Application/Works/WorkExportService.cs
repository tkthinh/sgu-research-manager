using Application.Shared.Messages;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Application.Users;

namespace Application.Works
{
    public class WorkExportService : IWorkExportService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<WorkService> _logger;
        private readonly IWorkQueryService _workQueryService;
        private readonly IUserService _userService;

        public WorkExportService(
            ILogger<WorkService> logger,
            IUserRepository userRepository,
            IWorkQueryService workQueryService,
            IUserService userService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _workQueryService = workQueryService;
            _userService = userService;
        }

        public async Task<byte[]> ExportWorksByUserAsync(List<ExportExcelDto> exportData, CancellationToken cancellationToken = default)
        {
            // Thử tìm file template ở các vị trí khác nhau
            string[] possiblePaths = new[]
            {
                // Đường dẫn từ thư mục bin của WebApi
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Infrastructure", "Templates", "ExportByUserTemplate.xlsx"),
                
                // Đường dẫn từ thư mục gốc của Server project
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Server", "Infrastructure", "Templates", "ExportByUserTemplate.xlsx")),
                
                // Đường dẫn từ thư mục gốc của solution
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Infrastructure", "Templates", "ExportByUserTemplate.xlsx"))
            };

            string? templatePath = null;
            foreach (var path in possiblePaths)
            {
                var normalizedPath = Path.GetFullPath(path);
                _logger.LogInformation("Đang thử tìm file template tại: {Path}", normalizedPath);

                if (File.Exists(normalizedPath))
                {
                    templatePath = normalizedPath;
                    _logger.LogInformation("Đã tìm thấy file template tại: {Path}", normalizedPath);
                    break;
                }
            }

            if (templatePath == null)
            {
                var errorMessage = $"Không tìm thấy file template Excel. Đã thử các đường dẫn sau:\n{string.Join("\n", possiblePaths.Select(p => Path.GetFullPath(p)))}";
                _logger.LogError(errorMessage);
                throw new FileNotFoundException(errorMessage);
            }

            using (var package = new ExcelPackage(new FileInfo(templatePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Lấy sheet đầu tiên

                // Điền thông tin cá nhân từ bản ghi đầu tiên
                var userInfo = exportData.FirstOrDefault();
                if (userInfo is null)
                    throw new Exception(ErrorMessages.NoDataToExport);

                // Điền thông tin cá nhân vào các ô tương ứng
                worksheet.Cells["C2"].Value = userInfo.DepartmentName;

                worksheet.Cells["B8"].Value = userInfo.FullName;
                worksheet.Cells["B9"].Value = userInfo.FieldName;
                worksheet.Cells["B10"].Value = userInfo.PhoneNumber;

                worksheet.Cells["E8"].Value = userInfo.AcademicTitle;
                worksheet.Cells["E9"].Value = userInfo.Specialization;
                worksheet.Cells["E10"].Value = userInfo.Email;

                worksheet.Cells["H8"].Value = userInfo.UserName;
                worksheet.Cells["H9"].Value = userInfo.OfficerRank;

                // Điền dữ liệu công trình vào bảng
                int startRow = 15; // Dòng bắt đầu của dữ liệu
                int currentRow = startRow;

                // Điền thông tin từng công trình
                for (int i = 0; i < exportData.Count; i++)
                {
                    var work = exportData[i];

                    worksheet.Cells[currentRow, 1].Value = i + 1;
                    worksheet.Cells[currentRow, 2].Value = work.Title;
                    worksheet.Cells[currentRow, 3].Value = work.WorkTypeName;
                    worksheet.Cells[currentRow, 4].Value = work.WorkLevelName ?? "";
                    worksheet.Cells[currentRow, 5].Value = work.TimePublished.HasValue ?
                        $"{work.TimePublished.Value.Month:D2}/{work.TimePublished.Value.Year}" :
                        "";
                    worksheet.Cells[currentRow, 6].Value = work.TotalAuthors.HasValue ? work.TotalAuthors.Value : "";
                    worksheet.Cells[currentRow, 7].Value = work.TotalMainAuthors.HasValue ? work.TotalMainAuthors.Value : "";
                    worksheet.Cells[currentRow, 8].Value = work.Position.HasValue ? work.Position.Value : "";
                    worksheet.Cells[currentRow, 9].Value = work.AuthorRoleName ?? "";
                    worksheet.Cells[currentRow, 10].Value = work.CoAuthorNames ?? "";
                    worksheet.Cells[currentRow, 11].Value = work.Details != null && work.Details.Any() ?
                        string.Join("; ", work.Details.Select(kv => $"{kv.Key}: {kv.Value}")) :
                        "";
                    worksheet.Cells[currentRow, 12].Value = work.PurposeName ?? "";

                    // Xử lý hiển thị mức điểm
                    string scoreLevelText = work.ScoreLevel switch
                    {
                        ScoreLevel.BaiBaoTopMuoi => "Bài báo khoa học Top 10%",
                        ScoreLevel.BaiBaoTopBaMuoi => "Bài báo khoa học Top 30%",
                        ScoreLevel.BaiBaoTopNamMuoi => "Bài báo khoa học Top 50%",
                        ScoreLevel.BaiBaoTopConLai => "Bài báo khoa học Top còn lại",
                        ScoreLevel.BaiBaoMotDiem => "Bài báo khoa học 1 điểm",
                        ScoreLevel.BaiBaoNuaDiem => "Bài báo khoa học 0.5 điểm",
                        ScoreLevel.BaiBaoKhongBayNamDiem => "Bài báo khoa học 0.75 điểm",
                        ScoreLevel.HDSVDatGiaiKK => "Hướng dẫn sinh viên đạt giải Khuyến khích",
                        ScoreLevel.HDSVDatGiaiBa => "Hướng dẫn sinh viên đạt giải Ba",
                        ScoreLevel.HDSVDatGiaiNhi => "Hướng dẫn sinh viên đạt giải Nhì",
                        ScoreLevel.HDSVDatGiaiNhat => "Hướng dẫn sinh viên đạt giải Nhất",
                        ScoreLevel.HDSVConLai => "Hướng dẫn sinh viên đạt các giải còn lại",
                        ScoreLevel.TacPhamNgheThuatCapTruong => "Tác phẩm nghệ thuật cấp Trường",
                        ScoreLevel.TacPhamNgheThuatCapTinhThanhPho => "Tác phẩm nghệ thuật cấp Tỉnh/Thành phố",
                        ScoreLevel.TacPhamNgheThuatCapQuocGia => "Tác phẩm nghệ thuật cấp Quốc gia",
                        ScoreLevel.TacPhamNgheThuatCapQuocTe => "Tác phẩm nghệ thuật cấp Quốc tế",
                        ScoreLevel.ThanhTichHuanLuyenCapQuocGia => "Thành tích huấn luyện cấp Quốc gia",
                        ScoreLevel.ThanhTichHuanLuyenCapQuocTe => "Thành tích huấn luyện cấp Quốc tế",
                        ScoreLevel.GiaiPhapHuuIchCapTinhThanhPho => "Giải pháp hữu ích cấp Tỉnh/Thành phố",
                        ScoreLevel.GiaiPhapHuuIchCapQuocGia => "Giải pháp hữu ích cấp Quốc gia",
                        ScoreLevel.GiaiPhapHuuIchCapQuocTe => "Giải pháp hữu ích cấp Quốc tế",
                        ScoreLevel.KetQuaNghienCuu => "Kết quả nghiên cứu",
                        ScoreLevel.Sach => "Sách",
                        _ => ""
                    };

                    worksheet.Cells[currentRow, 13].Value = scoreLevelText;
                    worksheet.Cells[currentRow, 14].Value = work.SCImagoFieldName ?? "";
                    worksheet.Cells[currentRow, 15].Value = work.ScoringFieldName ?? "";

                    // Định dạng giờ quy đổi với 1 chữ số thập phân
                    if (work.AuthorHour.HasValue)
                    {
                        var authorHourValue = (decimal)work.AuthorHour.Value;
                        worksheet.Cells[currentRow, 16].Value = Math.Round(authorHourValue, 1, MidpointRounding.AwayFromZero);
                        worksheet.Cells[currentRow, 16].Style.Numberformat.Format = "#,##0.0";
                    }
                    else
                    {
                        worksheet.Cells[currentRow, 16].Value = "";
                    }

                    // Xử lý hiển thị trạng thái
                    string proofStatusText = work.ProofStatus switch
                    {
                        ProofStatus.ChuaXuLy => "Chưa xử lý",
                        ProofStatus.HopLe => "Hợp lệ",
                        ProofStatus.KhongHopLe => "Không hợp lệ",
                        _ => ""
                    };

                    worksheet.Cells[currentRow, 17].Value = proofStatusText;
                    worksheet.Cells[currentRow, 18].Value = work.Note ?? "";

                    currentRow++;
                }

                // Tính tổng số công trình theo loại
                // Để trống 2 dòng sau danh sách công trình
                var summaryStartRow = currentRow + 2;

                // Chèn dòng trống cho phần tổng hợp
                worksheet.InsertRow(summaryStartRow, exportData.Count + 3); // +3 cho tiêu đề, dòng trống và tổng số

                // Thêm tiêu đề phần tổng hợp
                worksheet.Cells[summaryStartRow, 1, summaryStartRow, 3].Merge = true;
                worksheet.Cells[summaryStartRow, 1].Value = "III. TỔNG HỢP CÔNG TRÌNH THEO LOẠI";
                worksheet.Cells[summaryStartRow, 1].Style.Font.Bold = true;

                // Bắt đầu điền dữ liệu tổng hợp từ dòng tiếp theo
                summaryStartRow++;

                // Danh sách các loại công trình
                var workTypes = new[] {
                    "Bài báo khoa học",
                    "Báo cáo khoa học",
                    "Đề tài",
                    "Giáo trình",
                    "Sách",
                    "Hội thảo, hội nghị",
                    "Hướng dẫn SV NCKH",
                    "Khác"
                };

                // Danh sách các loại công trình thuộc nhóm "Sách"
                var bookWorkTypeNames = new[] {
                    "Chương sách",
                    "Chuyên khảo",
                    "Tham khảo",
                    "Giáo trình - Sách",
                    "Tài liệu hướng dẫn"
                };

                // Tính số lượng công trình theo loại
                var workTypeCounts = exportData
                    .GroupBy(w => {
                        // Nếu là một trong các loại sách, gộp chung vào "Sách"
                        if (bookWorkTypeNames.Contains(w.WorkTypeName))
                            return "Sách";
                        return w.WorkTypeName;
                    })
                    .ToDictionary(g => g.Key, g => g.Count());

                for (int i = 0; i < workTypes.Length; i++)
                {
                    worksheet.Cells[summaryStartRow + i, 1].Value = i + 1;
                    worksheet.Cells[summaryStartRow + i, 2].Value = workTypes[i];
                    worksheet.Cells[summaryStartRow + i, 3].Value = workTypeCounts.ContainsKey(workTypes[i]) ? workTypeCounts[workTypes[i]] : 0;
                }

                // Tổng số sản phẩm
                worksheet.Cells[summaryStartRow + workTypes.Length + 1, 1].Value = "Tổng số sản phẩm:";
                worksheet.Cells[summaryStartRow + workTypes.Length + 1, 2, summaryStartRow + workTypes.Length + 1, 3].Merge = true;
                worksheet.Cells[summaryStartRow + workTypes.Length + 1, 2].Value = exportData.Count;

                return package.GetAsByteArray();
            }
        }

        public async Task<List<ExportExcelDto>> GetExportExcelDataAsync(WorkFilter filter, CancellationToken cancellationToken = default)
        {
            // Lấy danh sách công trình với filter
            var works = await _workQueryService.GetWorksAsync(filter, cancellationToken);

            // Lấy thông tin đồng tác giả với đầy đủ details
            var allCoAuthorUserIds = works.SelectMany(w => w.CoAuthorUserIds).Distinct().ToList();
            var coAuthors = await _userRepository.GetUsersWithDetailsAsync();
            var filteredCoAuthors = coAuthors.Where(u => allCoAuthorUserIds.Contains(u.Id));

            // Ánh xạ dữ liệu vào ExportExcelDto
            var exportData = works.Select(w =>
            {
                var author = w.Authors?.FirstOrDefault();
                var coAuthorNames = string.Join(", ", w.CoAuthorUserIds
                    .Select(uid => filteredCoAuthors.FirstOrDefault(u => u.Id == uid)?.FullName ?? "Không xác định"));

                // Lấy thông tin người dùng từ author
                var user = author != null ? coAuthors.FirstOrDefault(u => u.Id == author.UserId) : null;

                return new ExportExcelDto
                {
                    UserName = user?.UserName ?? "Không xác định",
                    FullName = user?.FullName ?? "Không xác định",
                    Email = user?.Email ?? "Không xác định",
                    AcademicTitle = user?.AcademicTitle.ToString() ?? "Không xác định",
                    OfficerRank = user?.OfficerRank.ToString() ?? "Không xác định",
                    DepartmentName = user?.Department?.Name ?? "Không xác định",
                    FieldName = author?.FieldName ?? "Không xác định",
                    Specialization = user?.Specialization ?? "Không xác định",
                    PhoneNumber = user?.PhoneNumber ?? "Không xác định",
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
                    AuthorHour = (int?)author?.AuthorHour,
                    ProofStatus = author?.ProofStatus,
                    Note = author?.Note
                };
            }).ToList();

            return exportData;
        }

        public async Task<byte[]> ExportWorksByAdminAsync(List<ExportExcelDto> exportData, Guid userId, CancellationToken cancellationToken = default)
        {
            // Thử tìm file template ở các vị trí khác nhau
            string[] possiblePaths = new[]
            {
                // Đường dẫn từ thư mục bin của WebApi
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Infrastructure", "Templates", "ExportByAdminTemplate.xlsx"),
                
                // Đường dẫn từ thư mục gốc của Server project
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Server", "Infrastructure", "Templates", "ExportByAdminTemplate.xlsx")),
                
                // Đường dẫn từ thư mục gốc của solution
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Infrastructure", "Templates", "ExportByAdminTemplate.xlsx"))
            };

            string templatePath = possiblePaths.FirstOrDefault(File.Exists);
            if (templatePath == null)
            {
                throw new FileNotFoundException("Không tìm thấy file template ExportByAdminTemplate.xlsx");
            }

            using var package = new ExcelPackage(new FileInfo(templatePath));
            var worksheet = package.Workbook.Worksheets[0];

            // Lấy thông tin người dùng từ dữ liệu export
            var userInfo = exportData.FirstOrDefault();
            if (userInfo == null)
            {
                throw new Exception("Không có dữ liệu để xuất");
            }

            // Lấy năm học hiện tại (nếu có)
            Guid? academicYearId = null;
            if (exportData.Any() && exportData.First().Details.TryGetValue("AcademicYearId", out var ayIdStr))
            {
                if (Guid.TryParse(ayIdStr, out var ayId)) academicYearId = ayId;
            }

            // Lấy conversion result
            var conversionResult = await _userService.GetUserConversionResultAsync(userId);
            var conv = conversionResult.ConversionResults;

            // Điền thông tin người dùng
            worksheet.Cells["B8"].Value = userInfo.FullName;
            worksheet.Cells["B9"].Value = userInfo.FieldName;
            worksheet.Cells["B10"].Value = userInfo.PhoneNumber;
            worksheet.Cells["E8"].Value = userInfo.AcademicTitle;
            worksheet.Cells["E9"].Value = userInfo.Specialization;
            worksheet.Cells["E10"].Value = userInfo.Email;
            worksheet.Cells["H8"].Value = userInfo.UserName;
            worksheet.Cells["H9"].Value = userInfo.OfficerRank;

            // Điền dữ liệu công trình vào bảng
            int startRow = 14; // Dòng bắt đầu của dữ liệu
            int currentRow = startRow;

            // Điền thông tin từng công trình
            for (int i = 0; i < exportData.Count; i++)
            {
                var work = exportData[i];

                worksheet.Cells[currentRow, 1].Value = i + 1;
                worksheet.Cells[currentRow, 2].Value = work.Title;
                worksheet.Cells[currentRow, 3].Value = work.WorkTypeName;
                worksheet.Cells[currentRow, 4].Value = work.WorkLevelName ?? "";
                worksheet.Cells[currentRow, 5].Value = work.TimePublished.HasValue ?
                    $"{work.TimePublished.Value.Month:D2}/{work.TimePublished.Value.Year}" :
                    "";
                worksheet.Cells[currentRow, 6].Value = work.TotalAuthors.HasValue ? work.TotalAuthors.Value : "";
                worksheet.Cells[currentRow, 7].Value = work.Position.HasValue ? work.Position.Value : "";
                worksheet.Cells[currentRow, 8].Value = work.AuthorRoleName ?? "";
                worksheet.Cells[currentRow, 9].Value = work.CoAuthorNames ?? "";
                worksheet.Cells[currentRow, 10].Value = work.Details != null && work.Details.Any() ?
                    string.Join("; ", work.Details.Select(kv => $"{kv.Key}: {kv.Value}")) :
                    "";
                worksheet.Cells[currentRow, 11].Value = work.PurposeName ?? "";
                worksheet.Cells[currentRow, 12].Value = work.ScoreLevel.HasValue ? GetScoreLevelText(work.ScoreLevel.Value) : "";
                worksheet.Cells[currentRow, 13].Value = work.SCImagoFieldName ?? "";
                worksheet.Cells[currentRow, 14].Value = work.ScoringFieldName ?? "";
                worksheet.Cells[currentRow, 15].Value = work.AuthorHour.HasValue ? work.AuthorHour.Value : "";
                worksheet.Cells[currentRow, 16].Value = work.ProofStatus.HasValue ? GetProofStatusText(work.ProofStatus.Value) : "";

                currentRow++;
            }

            // Thêm một dòng trống
            currentRow++;

            // Thêm phần kết quả quy đổi
            worksheet.Cells[currentRow, 1].Value = "III. KẾT QUẢ QUY ĐỔI";
            currentRow += 2;

            // Thêm tiêu đề cột
            worksheet.Cells[currentRow, 1].Value = "TT";
            worksheet.Cells[currentRow, 2].Value = "Mục đích quy đổi";
            worksheet.Cells[currentRow, 3].Value = "Số CT";
            worksheet.Cells[currentRow, 4].Value = "Số giờ được quy đổi";
            worksheet.Cells[currentRow, 6].Value = "Số giờ được tính";
            currentRow++;

            // Thêm dữ liệu kết quả quy đổi từ conversionResult
            worksheet.Cells[currentRow, 1].Value = "1";
            worksheet.Cells[currentRow, 2].Value = "Thực hiện nghĩa vụ NCKH";
            worksheet.Cells[currentRow, 3].Value = conv?.DutyHourConversion?.TotalWorks ?? 0;
            worksheet.Cells[currentRow, 4].Value = conv?.DutyHourConversion?.TotalConvertedHours ?? 0;
            worksheet.Cells[currentRow, 6].Value = conv?.DutyHourConversion?.TotalCalculatedHours ?? 0;
            currentRow++;

            worksheet.Cells[currentRow, 1].Value = "2";
            worksheet.Cells[currentRow, 2].Value = "Vượt định mức";
            worksheet.Cells[currentRow, 3].Value = conv?.OverLimitConversion?.TotalWorks ?? 0;
            worksheet.Cells[currentRow, 4].Value = conv?.OverLimitConversion?.TotalConvertedHours ?? 0;
            worksheet.Cells[currentRow, 6].Value = conv?.OverLimitConversion?.TotalCalculatedHours ?? 0;
            currentRow++;

            worksheet.Cells[currentRow, 1].Value = "3";
            worksheet.Cells[currentRow, 2].Value = "Sản phẩm của đề tài";
            worksheet.Cells[currentRow, 3].Value = conv?.ResearchProductConversion?.TotalWorks ?? 0;
            worksheet.Cells[currentRow, 4].Value = conv?.ResearchProductConversion?.TotalConvertedHours ?? 0;
            worksheet.Cells[currentRow, 6].Value = conv?.ResearchProductConversion?.TotalCalculatedHours ?? 0;
            currentRow++;

            // Thêm tổng kết
            currentRow++;
            worksheet.Cells[currentRow, 1].Value = "Số công trình được tính quy đổi:";
            worksheet.Cells[currentRow, 3].Value = conv?.TotalWorks ?? 0;
            currentRow++;

            worksheet.Cells[currentRow, 1].Value = "Tổng số giờ viên chức được quy đổi:";
            worksheet.Cells[currentRow, 3].Value = conv?.TotalCalculatedHours ?? 0;

            return package.GetAsByteArray();
        }

        public async Task<byte[]> ExportAllWorksAsync(List<ExportExcelDto> exportData, CancellationToken cancellationToken = default)
        {
            // Thử tìm file template ở các vị trí khác nhau
            string[] possiblePaths = new[]
            {
                // Đường dẫn từ thư mục bin của WebApi
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Infrastructure", "Templates", "ExportAllWorksTemplate.xlsx"),
                
                // Đường dẫn từ thư mục gốc của Server project
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Server", "Infrastructure", "Templates", "ExportAllWorksTemplate.xlsx")),
                
                // Đường dẫn từ thư mục gốc của solution
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Infrastructure", "Templates", "ExportAllWorksTemplate.xlsx"))
            };

            string templatePath = possiblePaths.FirstOrDefault(File.Exists);
            if (templatePath == null)
            {
                throw new FileNotFoundException("Không tìm thấy file template ExportAllWorksTemplate.xlsx");
            }

            using var package = new ExcelPackage(new FileInfo(templatePath));
            var worksheet = package.Workbook.Worksheets[0];

            // Điền dữ liệu công trình vào bảng
            int startRow = 2; // Dòng bắt đầu của dữ liệu
            int currentRow = startRow;

            // Điền thông tin từng công trình
            for (int i = 0; i < exportData.Count; i++)
            {
                var work = exportData[i];

                worksheet.Cells[currentRow, 1].Value = work.DepartmentName;
                worksheet.Cells[currentRow, 2].Value = work.UserName;
                worksheet.Cells[currentRow, 3].Value = work.FullName;
                worksheet.Cells[currentRow, 4].Value = work.Title;
                worksheet.Cells[currentRow, 5].Value = work.WorkTypeName;
                worksheet.Cells[currentRow, 6].Value = work.WorkLevelName ?? "";
                worksheet.Cells[currentRow, 7].Value = work.TimePublished.HasValue ?
                    $"{work.TimePublished.Value.Month:D2}/{work.TimePublished.Value.Year}" :
                    "";
                worksheet.Cells[currentRow, 8].Value = work.Position.HasValue ? work.Position.Value : "";
                worksheet.Cells[currentRow, 9].Value = work.TotalAuthors.HasValue ? work.TotalAuthors.Value : "";
                worksheet.Cells[currentRow, 10].Value = work.AuthorRoleName ?? "";
                worksheet.Cells[currentRow, 11].Value = work.CoAuthorNames ?? "";
                worksheet.Cells[currentRow, 12].Value = work.Details != null && work.Details.Any() ?
                    string.Join("; ", work.Details.Select(kv => $"{kv.Key}: {kv.Value}")) :
                    "";
                worksheet.Cells[currentRow, 13].Value = work.PurposeName ?? "";
                worksheet.Cells[currentRow, 14].Value = work.ScoringFieldName ?? "";
                worksheet.Cells[currentRow, 15].Value = work.ScoreLevel.HasValue ? GetScoreLevelText(work.ScoreLevel.Value) : "";
                worksheet.Cells[currentRow, 16].Value = work.AuthorHour.HasValue ? work.AuthorHour.Value : "";
                worksheet.Cells[currentRow, 17].Value = work.Note ?? "";

                currentRow++;
            }

            return package.GetAsByteArray();
        }

        private string GetScoreLevelText(ScoreLevel scoreLevel)
        {
            return scoreLevel switch
            {
                ScoreLevel.BaiBaoTopMuoi => "Bài báo khoa học Top 10%",
                ScoreLevel.BaiBaoTopBaMuoi => "Bài báo khoa học Top 30%",
                ScoreLevel.BaiBaoTopNamMuoi => "Bài báo khoa học Top 50%",
                ScoreLevel.BaiBaoTopConLai => "Bài báo khoa học Top còn lại",
                ScoreLevel.BaiBaoMotDiem => "Bài báo khoa học 1 điểm",
                ScoreLevel.BaiBaoNuaDiem => "Bài báo khoa học 0.5 điểm",
                ScoreLevel.BaiBaoKhongBayNamDiem => "Bài báo khoa học 0.75 điểm",
                ScoreLevel.HDSVDatGiaiKK => "Hướng dẫn sinh viên đạt giải Khuyến khích",
                ScoreLevel.HDSVDatGiaiBa => "Hướng dẫn sinh viên đạt giải Ba",
                ScoreLevel.HDSVDatGiaiNhi => "Hướng dẫn sinh viên đạt giải Nhì",
                ScoreLevel.HDSVDatGiaiNhat => "Hướng dẫn sinh viên đạt giải Nhất",
                ScoreLevel.HDSVConLai => "Hướng dẫn sinh viên đạt các giải còn lại",
                ScoreLevel.TacPhamNgheThuatCapTruong => "Tác phẩm nghệ thuật cấp Trường",
                ScoreLevel.TacPhamNgheThuatCapTinhThanhPho => "Tác phẩm nghệ thuật cấp Tỉnh/Thành phố",
                ScoreLevel.TacPhamNgheThuatCapQuocGia => "Tác phẩm nghệ thuật cấp Quốc gia",
                ScoreLevel.TacPhamNgheThuatCapQuocTe => "Tác phẩm nghệ thuật cấp Quốc tế",
                ScoreLevel.ThanhTichHuanLuyenCapQuocGia => "Thành tích huấn luyện cấp Quốc gia",
                ScoreLevel.ThanhTichHuanLuyenCapQuocTe => "Thành tích huấn luyện cấp Quốc tế",
                ScoreLevel.GiaiPhapHuuIchCapTinhThanhPho => "Giải pháp hữu ích cấp Tỉnh/Thành phố",
                ScoreLevel.GiaiPhapHuuIchCapQuocGia => "Giải pháp hữu ích cấp Quốc gia",
                ScoreLevel.GiaiPhapHuuIchCapQuocTe => "Giải pháp hữu ích cấp Quốc tế",
                ScoreLevel.KetQuaNghienCuu => "Kết quả nghiên cứu",
                ScoreLevel.Sach => "Sách",
                _ => scoreLevel.ToString()
            };
        }

        private string GetProofStatusText(ProofStatus proofStatus)
        {
            return proofStatus switch
            {
                ProofStatus.HopLe => "Hợp lệ",
                ProofStatus.KhongHopLe => "Không hợp lệ",
                ProofStatus.ChuaXuLy => "Chưa xử lý",
                _ => proofStatus.ToString()
            };
        }
    }
}
