namespace Application.Users
{
    public class UserSearchDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
    }
} 