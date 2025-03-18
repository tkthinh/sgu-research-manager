const officerRankMap: Record<number, string> = {
  0: "Giáo viên",
  1: "Chuyên viên",
  2: "Chuyên viên chính",
  3: "Chuyên viên cao cấp",
  4: "Trợ giảng",
  5: "Giảng viên",
  6: "Giảng viên chính",
  7: "Giảng viên cao cấp",
};

export const getOfficerRank = (value: number): string => {
  return officerRankMap[value] || "Không xác định";
};