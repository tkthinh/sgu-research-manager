using Application.Works;
using Application.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
        public async Task<ActionResult<ApiResponse<WorkDto>>> CreateWork([FromBody] CreateWorkRequestDto request)
        {
            try
            {
                var dto = new WorkDto
                {
                    Title = request.Title,
                    TimePublished = request.TimePublished,
                    Source = request.Source,
                    Note = request.Note,
                    Details = request.Details,
                    WorkTypeId = request.WorkTypeId,
                    WorkLevelId = request.WorkLevelId,
                    WorkStatusId = request.WorkStatusId,
                    ScoringFieldId = request.ScoringFieldId,
                    WorkProofId = request.WorkProofId,
                    ManagerWorkScore = request.ManagerWorkScore,
                    TotalAuthors = request.TotalAuthors,
                    TotalHours = request.TotalHours,
                    MainAuthorCount = request.MainAuthorCount
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
                existingWork.Source = request.Source;
                existingWork.Note = request.Note;
                existingWork.Details = request.Details;
                existingWork.WorkTypeId = request.WorkTypeId;
                existingWork.WorkLevelId = request.WorkLevelId;
                existingWork.WorkStatusId = request.WorkStatusId;
                existingWork.ScoringFieldId = request.ScoringFieldId;
                existingWork.WorkProofId = request.WorkProofId;
                existingWork.ManagerWorkScore = request.ManagerWorkScore;
                existingWork.TotalAuthors = request.TotalAuthors;
                existingWork.TotalHours = request.TotalHours;
                existingWork.MainAuthorCount = request.MainAuthorCount;

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
