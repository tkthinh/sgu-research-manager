using System;

namespace Application.Works
{
    /// <summary>
    /// Lớp chứa các thông báo lỗi sử dụng trong WorkService
    /// </summary>
    public static class ErrorMessages
    {
        // System errors
        public const string SystemClosed = "Hệ thống đã đóng. Không thể thực hiện thao tác.";
        public const string SystemClosedNewWork = "Hệ thống đã đóng. Không thể tạo công trình mới.";
        public const string SystemClosedMarkingWork = "Hệ thống đã đóng. Không thể đánh dấu công trình.";
        public const string SystemClosedEditWork = "Hệ thống đã đóng. Chỉ được chỉnh sửa công trình không hợp lệ.";
        public const string SystemClosedDeleteWork = "Hệ thống đã đóng. Không thể xóa công trình";
        
        // Work errors
        public const string WorkNotFound = "Công trình không tồn tại";
        public const string WorkAlreadyExists = "Công trình đã tồn tại";
        
        // Author errors
        public const string AuthorNotFound = "Không tìm thấy tác giả";
        public const string AuthorNotFoundInWork = "Không tìm thấy thông tin tác giả trong công trình này";
        public const string AuthorInfoRequired = "Thông tin tác giả là bắt buộc khi thêm mới";
        public const string UserIdNotDetermined = "Không thể xác định UserId của người dùng hiện tại.";
        public const string AuthorRoleNotFound = "Không tìm thấy vai trò tác giả";
        
        // Factor errors
        public const string FactorNotFound = "Không tìm thấy Factor phù hợp";
        
        // Import/Export errors
        public const string InvalidFile = "File không hợp lệ";
        public const string ImportFailed = "Nhập dữ liệu thất bại";
        public const string NoDataToExport = "Không có dữ liệu để export";
        
        // User errors
        public const string UserNotFound = "Không tìm thấy người dùng";
        
        // Score limit error template
        public const string ScoreLimitExceeded = "Đã vượt quá giới hạn quy đổi ({0} công trình) cho loại công trình '{1}', cấp '{2}' với mức điểm {3}";
    }
} 