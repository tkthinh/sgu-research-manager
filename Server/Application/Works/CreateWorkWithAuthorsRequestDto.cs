using Application.Authors;
using Domain.Entities;

namespace Application.Works
{
    public class CreateWorkWithAuthorsRequestDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public DateOnly? TimePublished { get; set; }
        public int? TotalAuthors { get; set; }
        public int? TotalMainAuthors { get; set; }
        public int FinalWorkHour { get; set; }
        public string? Note { get; set; }
        public Dictionary<string, string>? Details { get; set; }
        public WorkSource Source { get; set; }
        public Guid WorkTypeId { get; set; }
        public Guid WorkLevelId { get; set; }
        public Guid SCImagoFieldId { get; set; }
        public Guid ScoringFieldId { get; set; }
        public Guid ProofStatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Danh sách tác giả (với các trường cần thiết)
        public List<CreateAuthorRequestDto> Authors { get; set; } = new List<CreateAuthorRequestDto>();
    }
}
