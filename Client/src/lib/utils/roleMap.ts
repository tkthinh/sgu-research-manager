const roleMap: Record<string, string> = {
  "User": "Người dùng",
  "Manager": "Quản lý",
  "Admin": "Admin",
};

export const getRole = (value: string): string => {
  return roleMap[value] || "-";
};