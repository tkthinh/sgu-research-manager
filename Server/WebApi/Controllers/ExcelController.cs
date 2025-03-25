using Application.Shared.Response;
using Application.Works;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly IWorkService _workService;
        private readonly ILogger<WorksController> _logger;


        public ExcelController(IWorkService workService, ILogger<WorksController> logger)
        {
            _workService = workService;
            _logger = logger;
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

            await _workService.ImportAsync(file);
            return Ok("Import thành công");
        }

        [HttpGet("export/{userId}")]
        public async Task<IActionResult> ExportWorksToExcel([FromRoute] Guid userId)
        {
            try
            {
                // Lấy dữ liệu export
                var exportData = await _workService.GetExportExcelDataAsync(userId);

                // Kiểm tra exportData có dữ liệu không
                if (exportData == null || !exportData.Any())
                {
                    _logger.LogWarning("Không có dữ liệu để export cho user {UserId}", userId);
                    return BadRequest(new ApiResponse<object>(false, "Không có dữ liệu để export"));
                }

                // Tạo file Excel từ WorkService
                var fileBytes = await _workService.ExportToExcelAsync(exportData);

                // Kiểm tra fileBytes có dữ liệu không
                if (fileBytes == null || fileBytes.Length == 0)
                {
                    _logger.LogError("File Excel không được tạo thành công cho user {UserId}", userId);
                    return BadRequest(new ApiResponse<object>(false, "Không thể tạo file Excel"));
                }

                // Log kích thước file để kiểm tra
                _logger.LogInformation("File Excel được tạo thành công, kích thước: {FileSize} bytes", fileBytes.Length);

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
                _logger.LogError(ex, "Lỗi khi export công trình của user {UserId}", userId);
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }
    }
}