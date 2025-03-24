using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Application.ScoreLevels
{
    public class ScoreLevelService : IScoreLevelService
    {
        private readonly ILogger<ScoreLevelService> _logger;

        public ScoreLevelService(ILogger<ScoreLevelService> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<int>> GetScoreLevelsByFiltersAsync(
            Guid workTypeId, 
            Guid? workLevelId = null, 
            Guid? authorRoleId = null, 
            Guid? purposeId = null)
        {
            List<ScoreLevel> scoreLevels = new List<ScoreLevel>();

            // Bài báo khoa học
            if (workTypeId == Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"))
            {
                if (workLevelId.HasValue)
                {
                    // Cấp WoS hoặc Scopus
                    if (workLevelId == Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290") || 
                        workLevelId == Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"))
                    {
                        // Các mức top
                        scoreLevels.Add(ScoreLevel.BaiBaoTopMuoi);
                        scoreLevels.Add(ScoreLevel.BaiBaoTopBaMuoi);
                        scoreLevels.Add(ScoreLevel.BaiBaoTopNamMuoi);
                        scoreLevels.Add(ScoreLevel.BaiBaoTopConLai);
                    }
                    // Các cấp khác
                    else
                    {
                        scoreLevels.Add(ScoreLevel.BaiBaoMotDiem);
                        scoreLevels.Add(ScoreLevel.BaiBaoKhongBayNamDiem);
                        scoreLevels.Add(ScoreLevel.BaiBaoNuaDiem);
                    }
                }
            }
            // Báo cáo khoa học - không có ScoreLevel
            else if (workTypeId == Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"))
            {
                // Không có mức điểm nào
            }
            // Hướng dẫn NCKH sinh viên
            else if (workTypeId == Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f"))
            {
                if (workLevelId == Guid.Parse("6bbf7e31-bcca-4078-b894-7c8d3afba607") || 
                    workLevelId == Guid.Parse("08becbaf-2a92-4de1-8908-454c4659ad94"))
                {
                    scoreLevels.Add(ScoreLevel.HDSVDatGiaiNhat);
                    scoreLevels.Add(ScoreLevel.HDSVDatGiaiNhi);
                    scoreLevels.Add(ScoreLevel.HDSVDatGiaiBa);
                    scoreLevels.Add(ScoreLevel.HDSVDatGiaiKK);
                }
                else
                {
                    scoreLevels.Add(ScoreLevel.HDSVConLai);
                }
            }
            // Tác phẩm nghệ thuật/Giải pháp hữu ích/các loại khác
            else if (workTypeId == Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"))
            {
                if (workLevelId.HasValue)
                {
                    // Cấp tỉnh/thành phố
                    if (workLevelId == Guid.Parse("d84ac5f8-d533-48d6-b829-9cf3556ce5bb"))
                    {
                        scoreLevels.Add(ScoreLevel.GiaiPhapHuuIchCapTinhThanhPho);
                        scoreLevels.Add(ScoreLevel.TacPhamNgheThuatCapTinhThanhPho);
                    }
                    // Cấp quốc gia
                    else if (workLevelId == Guid.Parse("b2302b5e-1614-484d-88ad-003c411ad248"))
                    {
                        scoreLevels.Add(ScoreLevel.GiaiPhapHuuIchCapQuocGia);
                        scoreLevels.Add(ScoreLevel.TacPhamNgheThuatCapQuocGia);
                        scoreLevels.Add(ScoreLevel.ThanhTichHuanLuyenCapQuocGia);
                    }
                    // Cấp quốc tế
                    else if (workLevelId == Guid.Parse("13e5b0a5-727b-427b-b103-0d58db679dcd"))
                    {
                        scoreLevels.Add(ScoreLevel.GiaiPhapHuuIchCapQuocTe);
                        scoreLevels.Add(ScoreLevel.TacPhamNgheThuatCapQuocTe);
                        scoreLevels.Add(ScoreLevel.ThanhTichHuanLuyenCapQuocTe);
                    }
                    // Cấp trường
                    else if (workLevelId == Guid.Parse("ee81fe90-15e7-48a2-8d94-a46db55f5b8f"))
                    {
                        scoreLevels.Add(ScoreLevel.TacPhamNgheThuatCapTruong);
                    }
                }
            }
            // Sách và các loại tương tự
            else if (workTypeId == Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256") || 
                    workTypeId == Guid.Parse("d3707663-2b44-4d95-93b7-37756d3e302c") || 
                    workTypeId == Guid.Parse("14b7a7e8-7327-450e-a5ca-f7d836b14499") || 
                    workTypeId == Guid.Parse("b1131264-329f-4908-8e71-8b36088d3dde") ||
                    workTypeId == Guid.Parse("b74daf03-dc04-4738-ae87-97ec0faa07c1"))
            {
                scoreLevels.Add(ScoreLevel.Sach);
            }

            _logger.LogInformation("Tìm thấy {Count} mức điểm cho workTypeId={WorkTypeId}, workLevelId={WorkLevelId}", 
                scoreLevels.Count, workTypeId, workLevelId);

            return scoreLevels.Select(sl => (int)sl);
        }
    }
} 