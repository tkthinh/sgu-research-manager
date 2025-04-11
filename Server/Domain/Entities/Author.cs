using Domain.Enums;

namespace Domain.Entities
{
    public class Author : BaseEntity
    {
        public Guid WorkId { get; set; }
        public Guid UserId { get; set; }
        public Guid? AuthorRoleId { get; set; }
        public Guid PurposeId { get; set; }
        public Guid? SCImagoFieldId { get; set; }
        public Guid? FieldId { get; set; }

        public int? Position { get; set; }
        public ScoreLevel? ScoreLevel { get; set; } // Mức điểm do người dùng chọn
        public decimal AuthorHour { get; set; }     // Giờ quy đổi mà tác giả nhận được
        public int WorkHour { get; set; }       // Giờ của công trình mà tác giả kê khai theo ScoreLevel
        public bool MarkedForScoring { get; set; }  // Cờ đánh dấu công trình được chọn để quy đổi
        public ProofStatus ProofStatus { get; set; } = ProofStatus.ChuaXuLy;
        public string? Note { get; set; }

        public virtual Field? Field { get; set; }
        public virtual SCImagoField? SCImagoField { get; set; }
        public virtual User? User { get; set; }
        public virtual AuthorRole? AuthorRole { get; set; }
        public virtual Work? Work { get; set; }
        public virtual Purpose? Purpose { get; set; }

        public virtual ICollection<WorkAuthor>? WorkAuthors { get; set; }
        public virtual AuthorRegistration? AuthorRegistration { get; set; }
    }
}