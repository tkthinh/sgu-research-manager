using Application.AcademicYears;
using Application.Shared.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademicYearsController : ControllerBase
    {
        private readonly IAcademicYearService academicYearService;
        private readonly ILogger<AcademicYearsController> logger;

        public AcademicYearsController(IAcademicYearService academicYearService, ILogger<AcademicYearsController> logger)
        {
            this.academicYearService = academicYearService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AcademicYearDto>>>> GetAcademicYears()
        {
            try
            {
                var academicYears = await academicYearService.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<AcademicYearDto>>(
                    true,
                    "Lấy dữ liệu năm học thành công",
                    academicYears
                ));
            }
            catch
            {
                logger.LogError("Error getting academic years");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra khi lấy dữ liệu năm học"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AcademicYearDto>>> GetAcademicYear([FromRoute] Guid id)
        {
            try
            {
                var academicYear = await academicYearService.GetByIdAsync(id);

                return Ok(new ApiResponse<AcademicYearDto>(
                    true,
                    "Lấy dữ liệu năm học thành công",
                    academicYear
                ));
            }
            catch
            {
                logger.LogError("Error getting academic year");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra khi lấy dữ liệu năm học"));
            }
        }

        [HttpGet("current")]
        public async Task<ActionResult<ApiResponse<AcademicYearDto>>> GetCurrentAcademicYear()
        {
            try
            {
                var academicYear = await academicYearService.GetCurrentAcademicYear();

                return Ok(new ApiResponse<AcademicYearDto>(
                    true,
                    "Lấy dữ liệu năm học hiện tại thành công",
                    academicYear
                ));
            }
            catch
            {
                logger.LogError("Error getting academic year");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra khi lấy dữ liệu năm học"));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<AcademicYearDto>>> CreateAcademicYear([FromBody] CreateAcademicYearRequestDto requestDto)
        {
            try
            {
                var academicYearDto = new AcademicYearDto
                {
                    Name = requestDto.Name,
                    StartDate = requestDto.StartDate,
                    EndDate = requestDto.EndDate
                };

                var academicYear = await academicYearService.CreateAsync(academicYearDto);

                return Ok(new ApiResponse<AcademicYearDto>(
                    true,
                    "Tạo năm học thành công",
                    academicYear
                ));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error creating academic year");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra khi tạo năm học"));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateAcademicYear([FromRoute] Guid id, [FromBody] UpdateAcademicYearRequestDto requestDto)
        {
            try
            {
                var existingAcademicYear = await academicYearService.GetByIdAsync(id);
                if (existingAcademicYear is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy dữ liệu năm học"));
                }

                existingAcademicYear.Name = requestDto.Name;
                existingAcademicYear.StartDate = requestDto.StartDate;
                existingAcademicYear.EndDate = requestDto.EndDate;

                await academicYearService.UpdateAsync(existingAcademicYear);
                return Ok(new ApiResponse<object>(true, "Cập nhật năm học thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error updating academic year");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra khi cập nhật năm học"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteAcademicYear([FromRoute] Guid id)
        {
            try
            {
                var existingAcademicYear = await academicYearService.GetByIdAsync(id);
                if (existingAcademicYear is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy dữ liệu năm học"));
                }

                await academicYearService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Đã xóa năm học"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting academic year");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra khi xóa năm học"));
            }
        }
    }
}
