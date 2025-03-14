using Application.Authors;

namespace Application.Works
{
    public class UpdateWorkByAdminRequestDto
    {
        public UpdateWorkRequestDto? WorkRequest { get; set; }
        public UpdateAuthorByAdminRequestDto? AuthorRequest { get; set; }
    }
}