import { ScoreLevel } from '../types/enums/ScoreLevel';

/**
 * Chuyển đổi giá trị ScoreLevel sang text hiển thị
 * @param scoreLevel Giá trị ScoreLevel cần chuyển đổi
 * @returns Chuỗi văn bản hiển thị tương ứng
 */
export const getScoreLevelText = (scoreLevel?: number | null): string => {
  if (scoreLevel === undefined || scoreLevel === null) {
    return "-";
  }

  switch (scoreLevel) {
    case ScoreLevel.BaiBaoTopMuoi:
      return "Top 10%";
    case ScoreLevel.BaiBaoTopBaMuoi:
      return "Top 30%";
    case ScoreLevel.BaiBaoTopNamMuoi:
      return "Top 50%";
    case ScoreLevel.BaiBaoTopConLai:
      return "Top còn lại";
    case ScoreLevel.BaiBaoMotDiem:
      return "Bài báo 1 điểm";
    case ScoreLevel.BaiBaoNuaDiem:
      return "Bài báo 0.5 điểm";
    case ScoreLevel.BaiBaoKhongBayNamDiem:
      return "Bài báo 0.75 điểm";
    case ScoreLevel.HDSVDatGiaiKK:
      return "HDSV đạt giải KK";
    case ScoreLevel.HDSVDatGiaiBa:
      return "HDSV đạt giải Ba";
    case ScoreLevel.HDSVDatGiaiNhi:
      return "HDSV đạt giải Nhì";
    case ScoreLevel.HDSVDatGiaiNhat:
      return "HDSV đạt giải Nhất";
    case ScoreLevel.HDSVConLai:
      return "HDSV còn lại";
    case ScoreLevel.TacPhamNgheThuatCapTruong:
      return "Tác phẩm nghệ thuật cấp trường";
    case ScoreLevel.TacPhamNgheThuatCapTinhThanhPho:
      return "Tác phẩm nghệ thuật cấp tỉnh/thành phố";
    case ScoreLevel.TacPhamNgheThuatCapQuocGia:
      return "Tác phẩm nghệ thuật cấp quốc gia";
    case ScoreLevel.TacPhamNgheThuatCapQuocTe:
      return "Tác phẩm nghệ thuật cấp quốc tế";
    case ScoreLevel.ThanhTichHuanLuyenCapQuocGia:
      return "Thành tích huấn luyện cấp quốc gia";
    case ScoreLevel.ThanhTichHuanLuyenCapQuocTe:
      return "Thành tích huấn luyện cấp quốc tế";
    case ScoreLevel.GiaiPhapHuuIchCapTinhThanhPho:
      return "Giải pháp hữu ích cấp tỉnh/thành phố";
    case ScoreLevel.GiaiPhapHuuIchCapQuocGia:
      return "Giải pháp hữu ích cấp quốc gia";
    case ScoreLevel.GiaiPhapHuuIchCapQuocTe:
      return "Giải pháp hữu ích cấp quốc tế";
    case ScoreLevel.KetQuaNghienCuu:
      return "Kết quả nghiên cứu";
    case ScoreLevel.Sach:
      return "Sách";
    default:
      return "-";
  }
};

/**
 * Tạo menu item options cho scoreLevels
 * @param scoreLevels Danh sách các score level cần chuyển đổi thành options
 * @returns Mảng các đối tượng { value, label } để sử dụng trong menu
 */
export const getScoreLevelOptions = (scoreLevels: number[]) => {
  return scoreLevels.map(level => ({
    value: level,
    label: getScoreLevelText(level)
  }));
};

/**
 * Trả về mô tả đầy đủ cho ScoreLevel
 * @param scoreLevel Giá trị ScoreLevel cần mô tả
 * @returns Mô tả đầy đủ
 */
export const getScoreLevelFullDescription = (scoreLevel?: number | null): string => {
  if (scoreLevel === undefined || scoreLevel === null) {
    return "Chưa có mức điểm";
  }

  switch (scoreLevel) {
    case ScoreLevel.BaiBaoTopMuoi:
      return "Bài báo khoa học thuộc top 10% tạp chí hàng đầu";
    case ScoreLevel.BaiBaoTopBaMuoi:
      return "Bài báo khoa học thuộc top 30% tạp chí hàng đầu";
    case ScoreLevel.BaiBaoTopNamMuoi:
      return "Bài báo khoa học thuộc top 50% tạp chí hàng đầu";
    case ScoreLevel.BaiBaoTopConLai:
      return "Bài báo khoa học thuộc top còn lại tạp chí hàng đầu";
    case ScoreLevel.BaiBaoMotDiem:
      return "Bài báo 1 điểm";
    case ScoreLevel.BaiBaoNuaDiem:
      return "Bài báo 0.5 điểm";
    case ScoreLevel.BaiBaoKhongBayNamDiem:
      return "Bài báo 0.75 điểm";
    case ScoreLevel.HDSVDatGiaiKK:
      return "Hướng dẫn đề tài NCKH đạt giải Khuyến khích";
    case ScoreLevel.HDSVDatGiaiBa:
      return "Hướng dẫn đề tài NCKH đạt giải Ba";
    case ScoreLevel.HDSVDatGiaiNhi:
      return "Hướng dẫn đề tài NCKH đạt giải Nhì";
    case ScoreLevel.HDSVDatGiaiNhat:
      return "Hướng dẫn đề tài NCKH đạt giải Nhất";
    case ScoreLevel.HDSVConLai:
      return "Hướng dẫn sinh viên NCKH còn lại";
    case ScoreLevel.TacPhamNgheThuatCapTruong:
      return "Tác phẩm nghệ thuật cấp trường";
    case ScoreLevel.TacPhamNgheThuatCapTinhThanhPho:
      return "Tác phẩm nghệ thuật cấp tỉnh/thành phố";
    case ScoreLevel.TacPhamNgheThuatCapQuocGia:
      return "Tác phẩm nghệ thuật cấp quốc gia";
    case ScoreLevel.TacPhamNgheThuatCapQuocTe:
      return "Tác phẩm nghệ thuật cấp quốc tế";
    case ScoreLevel.ThanhTichHuanLuyenCapQuocGia:
      return "Thành tích huấn luyện cấp quốc gia";
    case ScoreLevel.ThanhTichHuanLuyenCapQuocTe:
      return "Thành tích huấn luyện cấp quốc tế";
    case ScoreLevel.GiaiPhapHuuIchCapTinhThanhPho:
      return "Giải pháp hữu ích cấp tỉnh/thành phố";
    case ScoreLevel.GiaiPhapHuuIchCapQuocGia:
      return "Giải pháp hữu ích cấp quốc gia";
    case ScoreLevel.GiaiPhapHuuIchCapQuocTe:
      return "Giải pháp hữu ích cấp quốc tế";
    case ScoreLevel.KetQuaNghienCuu:
      return "Kết quả nghiên cứu";
    case ScoreLevel.Sach:
      return "Sách";
    default:
      return "Mức điểm không xác định";
  }
}; 