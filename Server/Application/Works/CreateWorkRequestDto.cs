using Application.Authors;
using Domain.Enums;

namespace Application.Works
{
    public class CreateWorkRequestDto
    {
        public required string Title { get; set; }
        public DateOnly? TimePublished { get; set; }
        public int? TotalAuthors { get; set; }
        public int? TotalMainAuthors { get; set; }
        public Dictionary<string, string>? Details { get; set; }
        public WorkSource Source { get; set; } = WorkSource.NguoiDungKeKhai;

        public Guid WorkTypeId { get; set; }
        public Guid? WorkLevelId { get; set; }
        public Guid AcademicYearId { get; set; }

        public CreateAuthorRequestDto Author { get; set; } = new();
        public List<Guid> CoAuthorUserIds { get; set; } = new();
    }
}