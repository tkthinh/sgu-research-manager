using Application.Shared.Response;
using Application.Works;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Shared.Services;
using Domain.Enums;
using System;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly IWorkExportService _workExportService;
        private readonly IWorkImportService _workImportService;
        private readonly ILogger<WorksController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public ExcelController(
            IWorkExportService workExportService, 
            ILogger<WorksController> logger,
            ICurrentUserService currentUserService,
            IWorkImportService workImportService)
        {
            _workExportService = workExportService;
            _logger = logger;
            _currentUserService = currentUserService;
            _workImportService = workImportService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportExcel([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File không hợp lệ");

            // Kiểm tra định dạng file
            string[] allowedExtensions = { ".xlsx", ".xls" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest("Chỉ chấp nhận file Excel (.xlsx hoặc .xls)");
            }

            await _workImportService.ImportAsync(file);
            return Ok("Import thành công");
        }

        [HttpGet("export-by-user")]
        public async Task<IActionResult> ExportWorksByUser(
            [FromQuery] string? academicYearId,
            [FromQuery] int? proofStatus,
            [FromQuery] int? source,
            [FromQuery] bool? onlyRegisteredWorks,
            [FromQuery] bool? onlyRegisterableWorks)
        {
            try
            {
                // Lấy userId từ service
                var (isSuccess, userId, _) = _currentUserService.GetCurrentUser();
                if (!isSuccess)
                {
                    _logger.LogError("Không thể xác định người dùng từ token");
                    return BadRequest(new ApiResponse<object>(false, "Không thể xác định người dùng"));
                }

                _logger.LogInformation("Bắt đầu xuất Excel cho userId: {UserId}", userId);

                // Tạo filter từ các tham số
                var filter = new WorkFilter
                {
                    UserId = userId,
                    AcademicYearId = academicYearId != null ? Guid.Parse(academicYearId) : (Guid?)null,
                    ProofStatus = proofStatus.HasValue ? (ProofStatus)proofStatus.Value : (ProofStatus?)null,
                    Source = source.HasValue ? (WorkSource)source.Value : (WorkSource?)null,
                    OnlyRegisteredWorks = onlyRegisteredWorks ?? false,
                    OnlyRegisterableWorks = onlyRegisterableWorks ?? false
                };

                // Lấy dữ liệu export với filter
                var exportData = await _workExportService.GetExportExcelDataAsync(filter);

                // Kiểm tra exportData có dữ liệu không
                if (exportData == null || !exportData.Any())
                {
                    _logger.LogWarning("Không có dữ liệu để export cho user {UserId}", userId);
                    return BadRequest(new ApiResponse<object>(false, "Không có dữ liệu để export"));
                }

                // Tạo file Excel từ WorkService
                var fileBytes = await _workExportService.ExportWorksByUserAsync(exportData);

                // Kiểm tra fileBytes có dữ liệu không
                if (fileBytes == null || fileBytes.Length == 0)
                {
                    _logger.LogError("File Excel không được tạo thành công cho user {UserId}", userId);
                    return BadRequest(new ApiResponse<object>(false, "Không thể tạo file Excel"));
                }

                // Tạo tên file với timestamp để tránh trùng lặp
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var fileName = $"KeKhaiCongTrinh_{timestamp}.xlsx";

                return File(
                    fileContents: fileBytes,
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: fileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi export công trình");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }

        [HttpGet("export-by-admin")]
        public async Task<IActionResult> ExportWorksByAdmin(
            [FromQuery] Guid userId,
            [FromQuery] string? academicYearId,
            [FromQuery] int? proofStatus)
        {
            try
            {
                _logger.LogInformation("Bắt đầu xuất Excel cho userId: {UserId}", userId);

                // Tạo filter từ các tham số
                var filter = new WorkFilter
                {
                    UserId = userId,
                    AcademicYearId = academicYearId != null ? Guid.Parse(academicYearId) : (Guid?)null,
                    ProofStatus = proofStatus.HasValue ? (ProofStatus)proofStatus.Value : (ProofStatus?)null,
                    Source = WorkSource.NguoiDungKeKhai
                };

                // Lấy dữ liệu export với filter
                var exportData = await _workExportService.GetExportExcelDataAsync(filter);

                // Xuất file Excel
                var excelBytes = await _workExportService.ExportWorksByAdminAsync(exportData, userId);

                // Trả về file Excel
                return File(
                    excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"export_by_admin_{userId}_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xuất Excel cho userId: {UserId}", userId);
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }
    }
}