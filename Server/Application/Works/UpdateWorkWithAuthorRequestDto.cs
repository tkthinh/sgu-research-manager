using Application.Authors;

namespace Application.Works
{
    public class UpdateWorkWithAuthorRequestDto
    {
        public UpdateWorkRequestDto WorkRequest { get; set; }
        public CreateAuthorRequestDto AuthorRequest { get; set; }
    }
}