import { ApiResponse } from "../../lib/types/common/ApiResponse";
import apiClient from "./api";

export const getScoreLevelsByFilters = async (
  workTypeId: string,
  workLevelId?: string,
  authorRoleId?: string,
  purposeId?: string
): Promise<number[]> => {
  try {
    // Tạo đối tượng URLSearchParams để xây dựng query string
    const params = new URLSearchParams();
    params.append('workTypeId', workTypeId);
    
    if (workLevelId) {
      params.append('workLevelId', workLevelId);
    }
    
    if (authorRoleId) {
      params.append('authorRoleId', authorRoleId);
    }
    
    if (purposeId) {
      params.append('purposeId', purposeId);
    }
    
    const response = await apiClient.get<ApiResponse<number[]>>(`/scorelevels?${params.toString()}`);
    return response.data.data;
  } catch (error) {
    console.error("Lỗi khi lấy danh sách score levels:", error);
    return [];
  }
};

export default {
  getScoreLevelsByFilters
}; 