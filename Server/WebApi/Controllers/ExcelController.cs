using Application.Shared.Response;
using Application.Works;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Authorize]
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

        [HttpGet("export")]
        public async Task<IActionResult> ExportWorksToExcel()
        {
            try
            {
                // Log tất cả claims để debug
                foreach (var claim in User.Claims)
                {
                    _logger.LogInformation("Claim: {Type} = {Value}", claim.Type, claim.Value);
                }

                // Lấy userId từ token
                var userIdClaim = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    _logger.LogError("Không tìm thấy claim 'id' trong token");
                    return BadRequest(new ApiResponse<object>(false, "Không thể xác định người dùng"));
                }

                if (!Guid.TryParse(userIdClaim, out var userId))
                {
                    _logger.LogError("UserId không hợp lệ: {UserId}", userIdClaim);
                    return BadRequest(new ApiResponse<object>(false, "ID người dùng không hợp lệ"));
                }

                _logger.LogInformation("Bắt đầu xuất Excel cho userId: {UserId}", userId);

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
                _logger.LogError(ex, "Lỗi khi export công trình");
                return BadRequest(new ApiResponse<object>(false, ex.Message));
            }
        }
    }
}