import { ApiResponse } from "../types/common/ApiResponse";
import { Cache } from "../types/models/Cache";
import apiClient from "./api";

export const getCaches = async (): Promise<ApiResponse<Cache[]>> => {
  const response = await apiClient.get<ApiResponse<Cache[]>>("/cache/keys");
  return response.data;
}

export const deleteCache = async (key: string): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(`/cache/clear/${key}`);
  return response.data;
}

export const clearCaches = async (): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>("/cache/clear-all");
  return response.data;
}