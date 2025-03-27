const academicTitleMap: Record<string, string> = {
  "CN": "Cử nhân",
  "ThS": "Thạc sĩ",
  "TS": "Tiến sĩ",
  "PGS_TS": "Phó Giáo sư, Tiến sĩ",
  "GS_TS": "Giáo sư, Tiến sĩ",
  "Unknown": "-",
};

export const getAcademicTitle = (value: string): string => {
  return academicTitleMap[value] || "-";
};