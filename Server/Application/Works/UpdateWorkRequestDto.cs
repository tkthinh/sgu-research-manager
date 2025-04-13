using Domain.Enums;

namespace Application.Works
{
    public class UpdateWorkRequestDto
    {
        public string? Title { get; set; }
        public DateOnly? TimePublished { get; set; }
        public int? TotalAuthors { get; set; }
        public int? TotalMainAuthors { get; set; }
        public Dictionary<string, string>? Details { get; set; }
        public WorkSource Source { get; set; } = WorkSource.NguoiDungKeKhai;

        public Guid? WorkTypeId { get; set; }
        public Guid? WorkLevelId { get; set; }
        public Guid? SystemConfigId { get; set; }
        public List<Guid> CoAuthorUserIds { get; set; } = new();
    }
}