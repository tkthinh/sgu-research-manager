namespace Domain.Entities
{
    public class Author : BaseEntity
    {
        public Guid WorkId { get; set; }
        public Guid UserId { get; set; }
        public Guid AuthorRoleId { get; set; }
        public Guid PurposeId { get; set; }
        public int? Position { get; set; }
        public float DeclaredScore { get; set; }
        public int FinalAuthorHour { get; set; }    // Giờ chính thức do admin chấm
        public int TempAuthorHour { get; set; }     // Giờ dự kiến
        public int TempWorkHour { get; set; }       // Giờ dự kiến của công trình (tính từ DeclaredScore)
        public bool IsNotMatch { get; set; }        // Cờ kiểm tra TempAuthorHour và FinalAuthorHour
        public bool MarkedForScoring { get; set; }  // Cờ đánh dấu công trình được chọn để quy đổi
        public string? CoAuthors { get; set; }

        public virtual User? User { get; set; }
        public virtual AuthorRole? AuthorRole { get; set; }
        public virtual Work? Work { get; set; }
        public virtual Purpose? Purpose { get; set; }
    }
}