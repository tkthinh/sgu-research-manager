namespace Application.AuthorRoles
{
    public class CreateAuthorRoleRequestDto
    {
        public required string Name { get; set; }
        public bool IsMainAuthor { get; set; }
        public Guid WorkTypeId { get; set; }
    }
}
