using Application.Authors;
using Domain.Enums;

namespace Application.Works
{
    public class WorkDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public DateOnly? TimePublished { get; set; }
        public int? TotalAuthors { get; set; }
        public int? TotalMainAuthors { get; set; }
        public Dictionary<string, string>? Details { get; set; }
        public WorkSource Source { get; set; }

        public Guid WorkTypeId { get; set; }
        public string? WorkTypeName { get; set; }
        public Guid? WorkLevelId { get; set; }
        public string? WorkLevelName { get; set; }

        public DateOnly? ExchangeDeadline { get; set; }

        public IEnumerable<AuthorDto>? Authors { get; set; }
        public List<Guid> CoAuthorUserIds { get; set; } = new List<Guid>();

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}