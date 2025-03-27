using Domain.Enums;

namespace Application.ScoreLevels
{
    public interface IScoreLevelService
    {
        /// <summary>
        /// Lấy danh sách mức điểm dựa trên các bộ lọc
        /// </summary>
        /// <param name="workTypeId">ID loại công trình (bắt buộc)</param>
        /// <param name="workLevelId">ID cấp công trình (không bắt buộc)</param>
        /// <param name="authorRoleId">ID vai trò tác giả (không bắt buộc)</param>
        /// <param name="purposeId">ID mục đích quy đổi (không bắt buộc)</param>
        /// <returns>Danh sách giá trị enum ScoreLevel</returns>
        Task<IEnumerable<int>> GetScoreLevelsByFiltersAsync(
            Guid workTypeId, 
            Guid? workLevelId = null, 
            Guid? authorRoleId = null, 
            Guid? purposeId = null);
    }
} 