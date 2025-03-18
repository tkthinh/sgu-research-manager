import { ApiResponse } from "../types/common/ApiResponse";
import { User } from "../types/models/User";
import apiClient from "./api";

export const searchUsers = async (searchTerm: string): Promise<ApiResponse<User[]>> => {
  const response = await apiClient.get<ApiResponse<User[]>>(`/api/users/search?searchTerm=${encodeURIComponent(searchTerm)}`);
  return response.data;
}; 