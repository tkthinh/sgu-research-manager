namespace Application.AuthorRoles
{
    public class UpdateAuthorRoleRequestDto
    {
        public required string Name { get; set; }
        public bool IsMainAuthor { get; set; }
        public Guid WorkTypeId { get; set; }
    }
}
