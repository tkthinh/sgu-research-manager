using Domain.Enums;

namespace Domain.Entities
{
    public class Factor : BaseEntity
    {
        public Guid WorkTypeId { get; set; }
        public Guid? WorkLevelId { get; set; }
        public Guid PurposeId { get; set; }
        public Guid? AuthorRoleId { get; set; }
        public required string Name { get; set; }
        public ScoreLevel? ScoreLevel { get; set; } // Mức điểm của công trình
        public int ConvertHour { get; set; } // Giờ được quy đổi theo mục đích: Vượt/Nghĩa vụ
        public int? MaxAllowed { get; set; } // Số lượng tối đa công trình được phép đánh dấu cho quy đổi

        public virtual WorkType? WorkType { get; set; }
        public virtual WorkLevel? WorkLevel { get; set; }
        public virtual Purpose? Purpose { get; set; }
        public virtual AuthorRole? AuthorRole { get; set; }
    }
}
