namespace Application.AuthorRoles
{
    public class AuthorRoleDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public bool IsMainAuthor { get; set; }
        public Guid WorkTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
