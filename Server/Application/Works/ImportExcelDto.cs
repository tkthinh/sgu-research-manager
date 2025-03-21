using Domain.Enums;

namespace Application.ExcelOperations.Dtos
{
    public class ImportExcelDto
    {
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? DepartmentName { get; set; }
        public string? Title { get; set; }
        public string? WorkTypeName { get; set; }
        public string? WorkLevelName { get; set; }
        public DateOnly? TimePublished { get; set; }
        public int? TotalAuthors { get; set; }
        public int? TotalMainAuthors { get; set; }
        public Dictionary<string, string> Details { get; set; } = new Dictionary<string, string>();
        public int? Position { get; set; }
        public string? AuthorRoleName { get; set; }
        public string? PurposeName { get; set; }
        public string? SCImagoFieldName { get; set; }
        public string? ScoringFieldName { get; set; }
        public ScoreLevel? ScoreLevel { get; set; }
    }
}