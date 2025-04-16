using Domain.Enums;

namespace Application.Works
{
    /// <summary>
    /// Lớp bộ lọc để tổng quát hóa các điều kiện tìm kiếm công trình
    /// </summary>
    public class WorkFilter
    {
        /// <summary>
        /// ID của người dùng cần tìm công trình
        /// </summary>
        public Guid? UserId { get; set; }
        
        /// <summary>
        /// ID của phòng ban/khoa cần tìm công trình
        /// </summary>
        public Guid? DepartmentId { get; set; }
        
        /// <summary>
        /// ID của năm học cần tìm công trình
        /// </summary>
        public Guid? AcademicYearId { get; set; }
        
        /// <summary>
        /// Trạng thái xác minh của công trình
        /// </summary>
        public ProofStatus? ProofStatus { get; set; }
        
        /// <summary>
        /// Nguồn của công trình (Import hay người dùng kê khai)
        /// </summary>
        public WorkSource? Source { get; set; }
        
        /// <summary>
        /// Chỉ lấy công trình đã đăng ký quy đổi
        /// </summary>
        public bool OnlyRegisteredWorks { get; set; }
        
        /// <summary>
        /// Chỉ lấy công trình có thể đăng ký quy đổi
        /// </summary>
        public bool OnlyRegisterableWorks { get; set; }
        
        /// <summary>
        /// Đánh dấu là người dùng hiện tại
        /// </summary>
        public bool IsCurrentUser { get; set; }
    }
} 