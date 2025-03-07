namespace Domain.Entities
{
    public class Author : BaseEntity
    {
        public Guid WorkId { get; set; }
        public Guid UserId { get; set; }
        public Guid AuthorRoleId { get; set; }
        public Guid PurposeId { get; set; }
        public int? Position { get; set; }          // Vị trí của tác giả thuộc công trình đó
        public float DeclaredScore { get; set; }    // Điểm của công trình mà tác giả này đã kê khai
        public int FinalAuthorHour { get; set; }    // Số giờ chính thức của tác giả được quy đổi
        public int TempAuthorHour { get; set; }     // Số giờ dự kiến của tác giả được quy đổi
        public int TempWorkHour { get; set; }       // Giờ dự kiến của công trình được quy đổi
        public bool IsNotMatch { get; set; }        // Cờ đánh dấu xem FinalAuthorHour có khớp với TempAuthorHour không
        public bool MarkedForScoring { get; set; }  // Cờ đánh dấu xem công trình này của tác giả này đã được tick để chấm điểm chưa?
        public string? CoAuthors { get; set; }      // Danh sách đồng tác giả của công trình này

        public virtual Employee? User { get; set; }
        public virtual AuthorRole? AuthorRole { get; set; }
        public virtual Work? Work { get; set; }
        public virtual Purpose? Purpose { get; set; }
    }
}
