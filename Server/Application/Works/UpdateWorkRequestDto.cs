using Domain.Enums;

namespace Application.Works
{
    public class UpdateWorkRequestDto
    {
        public required string Title { get; set; }
        public DateOnly? TimePublished { get; set; }
        public int? TotalAuthors { get; set; }
        public int? TotalMainAuthors { get; set; }
        //public int FinalWorkHour { get; set; }
        //public ProofStatus ProofStatus { get; set; }
        public string? Note { get; set; }
        public Dictionary<string, string>? Details { get; set; }
        public WorkSource Source { get; set; } = WorkSource.NguoiDungKeKhai;

        public Guid WorkTypeId { get; set; }
        public Guid? WorkLevelId { get; set; }
        public Guid? SCImagoFieldId { get; set; }
        public Guid? ScoringFieldId { get; set; }
    }
}
