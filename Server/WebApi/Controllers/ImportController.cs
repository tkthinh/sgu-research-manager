using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IExcelService _excelService;

        public ImportController(IExcelService excelService)
        {
            _excelService = excelService;
        }

        [HttpPost("import-excel")]
        public async Task<IActionResult> ImportExcel([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File không hợp lệ");

            await _excelService.ImportAsync(file);
            return Ok("Import thành công");
        }
    }
}