using Application.Authors;

namespace Application.Works
{
    public class UpdateWorkWithAuthorRequestDto
    {
        public UpdateWorkRequestDto? WorkRequest { get; set; }
        public UpdateAuthorRequestDto? AuthorRequest { get; set; }
    }
}
