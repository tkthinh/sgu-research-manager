using Domain.Enums;

namespace Application.Works
{
    public class ExportExcelDto
    {
        public required string UserName { get; set; }   // Tên đăng nhập
        public required string FullName { get; set; }   // Họ và tên
        public required string Email { get; set; }      // Email
        public required string AcademicTitle { get; set; }  // Học hàm, học vị  
        public required string OfficerRank { get; set; }    // Ngạch công chức
        public required string DepartmentName { get; set; } // Đơn vị công tác
        public required string FieldName { get; set; }      // Ngành - Field
        //public required string MajorName { get; set; }      // Chuyên ngành
        //public required string PhoneNumber { get; set; }    // Số điện thoại
        public required string Title { get; set; }          // Tên công trình
        public required string WorkTypeName { get; set; }   // Loại công trình
        public string? WorkLevelName { get; set; }          // Cấp công trình
        public DateOnly? TimePublished { get; set; }        // Thời gian công bố
        public int? TotalAuthors { get; set; }              // Tổng số tác giả
        public int? TotalMainAuthors { get; set; }          // Tổng số tác giả chính
        public int? Position { get; set; }                  // Vị trí tác giả
        public string? AuthorRoleName { get; set; }         // Vai trò tác giả
        public List<Guid> CoAuthorUserIds { get; set; } = new List<Guid>(); // Danh sách UserId của đồng tác giả
        public string CoAuthorNames { get; set; } = string.Empty; // Tên của các đồng tác giả (được truy vấn từ User)
        public Dictionary<string, string> Details { get; set; } = new Dictionary<string, string>();
        public string? PurposeName { get; set; }            // Mục đích quy đổi
        public string? SCImagoFieldName { get; set; }       // Ngành SCImago do người dùng chọn từ SCImagoField
        public string? ScoringFieldName { get; set; }       // Ngành tính điểm do người dùng chọn từ Field
        public ScoreLevel? ScoreLevel { get; set; }         // Mức điểm của công trình
        public int? AuthorHour { get; set; }                // Giờ quy đổi của tác giả
        public ProofStatus? ProofStatus { get; set; }       // Trạng thái
        public string? Note { get; set; }                   // Ghi chú

    }
}
