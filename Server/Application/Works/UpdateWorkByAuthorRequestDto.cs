using Application.Authors;

namespace Application.Works
{
    public class UpdateWorkByAuthorRequestDto
    {
        public UpdateWorkRequestDto? WorkRequest { get; set; }
        public UpdateAuthorByAuthorRequestDto? AuthorRequest { get; set; }
    }
}