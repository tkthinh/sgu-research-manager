//using Domain.Entities;

//namespace Application.Works
//{
//    public class CreateWorkRequestDto
//    {
//        public required string Title { get; set; }
//        public DateOnly? TimePublished { get; set; }
//        public int? TotalAuthors { get; set; }
//        public int? TotalMainAuthors { get; set; }
//        public int FinalWorkHour { get; set; }
//        public string? Note { get; set; }
//        public Dictionary<string, string>? Details { get; set; }
//        public WorkSource Source { get; set; }
//        public Guid WorkTypeId { get; set; }
//        public Guid WorkLevelId { get; set; }
//        public Guid SCImagoFieldId { get; set; }
//        public Guid ScoringFieldId { get; set; }
//        public Guid ProofStatusId { get; set; }
//    }
//}

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
        public string? Note { get; set; }
        public Dictionary<string, string>? Details { get; set; }
        public WorkSource Source { get; set; }  
        public ProofStatus ProofStatus { get; set; }
        public Guid WorkTypeId { get; set; }
        public Guid WorkLevelId { get; set; }
        public Guid SCImagoFieldId { get; set; }
        public Guid ScoringFieldId { get; set; }
        public Guid ProofStatusId { get; set; } // Mặc định do admin set sau

        public CreateAuthorRequestDto? Author { get; set; }
    }
}