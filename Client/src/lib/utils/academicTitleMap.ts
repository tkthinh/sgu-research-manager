const academicTitleMap: Record<number, string> = {
  1: "Cử nhân",
  2: "Thạc sĩ",
  3: "Tiến sĩ",
  4: "Phó Giáo sư, Tiến sĩ",
  5: "Giáo sư, Tiến sĩ",
};

export const getAcademicTitle = (value: number): string => {
  return academicTitleMap[value] || "Không xác định";
};