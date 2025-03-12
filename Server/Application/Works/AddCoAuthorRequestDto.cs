using Application.Authors;

namespace Application.Works
{
    public class AddCoAuthorRequestDto
    {
        public UpdateWorkRequestDto WorkRequest { get; set; }
        public CreateAuthorRequestDto AuthorRequest { get; set; }
    }
}