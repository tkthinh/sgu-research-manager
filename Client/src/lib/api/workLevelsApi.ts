import { ApiResponse } from "../types/common/ApiResponse";
import { WorkLevel } from "../types/models/WorkLevel";
import apiClient from "./api";

// Fetch all work levels
export const getWorkLevels = async (): Promise<ApiResponse<WorkLevel[]>> => {
  const response = await apiClient.get<ApiResponse<WorkLevel[]>>("/worklevels");
  return response.data;
};

// Fetch work levels by work type ID
export const getWorkLevelsByWorkTypeId = async (workTypeId: string): Promise<ApiResponse<WorkLevel[]>> => {
  const response = await apiClient.get<ApiResponse<WorkLevel[]>>(`/worklevels/work-type/${workTypeId}`);
  return response.data;
};

// Fetch work level by ID
export const getWorkLevelById = async (id: string): Promise<ApiResponse<WorkLevel>> => {
  const response = await apiClient.get<ApiResponse<WorkLevel>>(`/worklevels/${id}`);
  return response.data;
};

// Create a new work level
export const createWorkLevel = async (workLevelData: Partial<WorkLevel>): Promise<ApiResponse<WorkLevel>> => {
  const response = await apiClient.post<ApiResponse<WorkLevel>>("/worklevels", workLevelData);
  return response.data;
};

// Update a work level
export const updateWorkLevel = async (id: string, workLevelData: Partial<WorkLevel>): Promise<ApiResponse<WorkLevel>> => {
  const response = await apiClient.put<ApiResponse<WorkLevel>>(`/worklevels/${id}`, workLevelData);
  return response.data;
};

// Delete a work level
export const deleteWorkLevel = async (id: string): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(`/worklevels/${id}`);
  return response.data;
};
