//using System.ComponentModel.DataAnnotations.Schema;

//namespace Domain.Entities
//{
//    public class Author : BaseEntity
//    {
//        //public Guid WorkId { get; set; }
//        //public Guid UserId { get; set; }
//        public Guid AuthorRoleId { get; set; }
//        public Guid PurposeId { get; set; }
//        public int Position { get; set; }
//        public float TempScore { get; set; }
//        public float FinalScore { get; set; }
//        public int TempHours { get; set; }
//        public int FinalHours { get; set; }
//        public bool IsNotMatch { get; set; }
//        public float TempWorkScore { get; set; } // Điểm công trình do người dùng nhập vào

//        // Khóa ngoại
//        public virtual Employee? Employee { get; set; }
//        public virtual AuthorRole? AuthorRole { get; set; }
//        //public virtual Work Work { get; set; }
//        public virtual Purpose? Purpose { get; set; }


//    }
//}
