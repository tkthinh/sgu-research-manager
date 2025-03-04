using Application.BookExtraOptions;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookExtraOptionsController : ControllerBase
    {
        private readonly IBookExtraOptionService bookExtraOptionService;
        private readonly ILogger<BookExtraOptionsController> logger;

        public BookExtraOptionsController(IBookExtraOptionService bookExtraOptionService, ILogger<BookExtraOptionsController> logger)
        {
            this.bookExtraOptionService = bookExtraOptionService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<BookExtraOptionDto>>>> GetBookExtraOptions()
        {
            var options = await bookExtraOptionService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<BookExtraOptionDto>>(
                true,
                "Lấy dữ liệu extra options thành công",
                options
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BookExtraOptionDto>>> GetBookExtraOption([FromRoute] Guid id)
        {
            var option = await bookExtraOptionService.GetByIdAsync(id);
            if (option is null)
            {
                return NotFound(new ApiResponse<BookExtraOptionDto>(false, "Không tìm thấy extra option"));
            }
            return Ok(new ApiResponse<BookExtraOptionDto>(
                true,
                "Lấy dữ liệu extra option thành công",
                option
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<BookExtraOptionDto>>> CreateBookExtraOption([FromBody] CreateBookExtraOptionRequestDto request)
        {
            try
            {
                var dto = new BookExtraOptionDto
                {
                    Name = request.Name,
                    WorkTypeId = request.WorkTypeId
                };

                var created = await bookExtraOptionService.CreateAsync(dto);
                var response = new ApiResponse<BookExtraOptionDto>(
                    true,
                    "Tạo extra option thành công",
                    created
                );

                return CreatedAtAction(nameof(GetBookExtraOption), new { id = created.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error creating book extra option");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateBookExtraOption([FromRoute] Guid id, [FromBody] UpdateBookExtraOptionRequestDto request)
        {
            try
            {
                var existingOption = await bookExtraOptionService.GetByIdAsync(id);
                if (existingOption is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy extra option"));
                }

                existingOption.Name = request.Name;
                existingOption.WorkTypeId = request.WorkTypeId;

                await bookExtraOptionService.UpdateAsync(existingOption);
                return Ok(new ApiResponse<object>(true, "Cập nhật extra option thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error updating book extra option");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteBookExtraOption([FromRoute] Guid id)
        {
            try
            {
                var existingOption = await bookExtraOptionService.GetByIdAsync(id);
                if (existingOption is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy extra option"));
                }

                await bookExtraOptionService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa extra option thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting book extra option");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi đã xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
