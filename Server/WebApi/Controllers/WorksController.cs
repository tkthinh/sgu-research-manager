using Application.Works;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorksController : ControllerBase
    {
        private readonly IWorkService workService;
        private readonly ILogger<WorksController> logger;

        public WorksController(IWorkService workService, ILogger<WorksController> logger)
        {
            this.workService = workService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkDto>>>> GetWorks()
        {
            var works = await workService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<WorkDto>>(
                true,
                "Lấy dữ liệu công trình thành công",
                works
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WorkDto>>> GetWork([FromRoute] Guid id)
        {
            var work = await workService.GetByIdAsync(id);
            if (work is null)
            {
                return NotFound(new ApiResponse<WorkDto>(false, "Không tìm thấy công trình"));
            }
            return Ok(new ApiResponse<WorkDto>(
                true,
                "Lấy dữ liệu công trình thành công",
                work
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<WorkDto>>> CreateWork([FromBody] CreateWorkRequestDto requestDto)
        {
            try
            {
                var dto = new WorkDto
                {
                    Title = requestDto.Title,
                    TimePublished = requestDto.TimePublished,
                    TotalAuthors = requestDto.TotalAuthors,
                    TotalMainAuthors = requestDto.TotalMainAuthors,
                    FinalWorkHour = requestDto.FinalWorkHour,
                    Note = requestDto.Note,
                    Details = requestDto.Details,
                    Source = requestDto.Source,
                    WorkTypeId = requestDto.WorkTypeId,
                    WorkLevelId = requestDto.WorkLevelId,
                    SCImagoFieldId = requestDto.SCImagoFieldId,
                    ScoringFieldId = requestDto.ScoringFieldId,
                    ProofStatusId = requestDto.ProofStatusId
                };

                var createdWork = await workService.CreateAsync(dto);
                var response = new ApiResponse<WorkDto>(
                    true,
                    "Tạo công trình thành công",
                    createdWork
                );

                return CreatedAtAction(nameof(GetWork), new { id = createdWork.Id }, response);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Lỗi khi tạo công trình");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateWork([FromRoute] Guid id, [FromBody] UpdateWorkRequestDto request)
        {
            try
            {
                var existingWork = await workService.GetByIdAsync(id);
                if (existingWork is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy công trình"));
                }

                existingWork.Title = request.Title;
                existingWork.TimePublished = request.TimePublished;
                existingWork.TotalAuthors = request.TotalAuthors;
                existingWork.TotalMainAuthors = request.TotalMainAuthors;
                existingWork.FinalWorkHour = request.FinalWorkHour;
                existingWork.Note = request.Note;
                existingWork.Details = request.Details;
                existingWork.Source = request.Source;
                existingWork.WorkTypeId = request.WorkTypeId;
                existingWork.WorkLevelId = request.WorkLevelId;
                existingWork.SCImagoFieldId = request.SCImagoFieldId;
                existingWork.ScoringFieldId = request.ScoringFieldId;
                existingWork.ProofStatusId = request.ProofStatusId;

                await workService.UpdateAsync(existingWork);
                return Ok(new ApiResponse<object>(true, "Cập nhật công trình thành công"));
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Lỗi khi cập nhật công trình");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteWork([FromRoute] Guid id)
        {
            try
            {
                var existingWork = await workService.GetByIdAsync(id);
                if (existingWork is null)
                {
                    return NotFound(new ApiResponse<object>(false, "Không tìm thấy công trình"));
                }

                await workService.DeleteAsync(id);
                return Ok(new ApiResponse<object>(true, "Xóa công trình thành công"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Lỗi khi xóa công trình");
                return BadRequest(new ApiResponse<object>(false, "Có lỗi xảy ra trong quá trình thực hiện"));
            }
        }
    }
}
