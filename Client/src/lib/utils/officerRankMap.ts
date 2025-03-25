const officerRankMap: Record<string, string> = {
  'GiaoVien': "Giáo viên",
  "ChuyenVien": "Chuyên viên",
  "ChuyenVienChinh": "Chuyên viên chính",
  "ChuyenVienCaoCap": "Chuyên viên cao cấp",
  "TroGiang": "Trợ giảng",
  "GiangVien": "Giảng viên",
  "GiangVienChinh": "Giảng viên chính",
  "GiangVienCaoCap": "Giảng viên cao cấp",
};

export const getOfficerRank = (value: string): string => {
  return officerRankMap[value] || "-";
};