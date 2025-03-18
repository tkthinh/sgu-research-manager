import { ApiResponse } from "../types/common/ApiResponse";
import { WorkType } from "../types/models/WorkType";
import apiClient from "./api";

// Fetch all work types
export const getWorkTypes = async (): Promise<ApiResponse<WorkType[]>> => {
  const response = await apiClient.get<ApiResponse<WorkType[]>>("/worktypes");
  return response.data;
};

// Fetch work type by ID
export const getWorkTypeById = async (id: string): Promise<ApiResponse<WorkType>> => {
  const response = await apiClient.get<ApiResponse<WorkType>>(`/worktypes/${id}`);
  return response.data;
};

// Create a new work type
export const createWorkType = async (workTypeData: Partial<WorkType>): Promise<ApiResponse<WorkType>> => {
  const response = await apiClient.post<ApiResponse<WorkType>>("/worktypes", workTypeData);
  return response.data;
};

// Update a work type
export const updateWorkType = async (id: string, workTypeData: Partial<WorkType>): Promise<ApiResponse<WorkType>> => {
  const response = await apiClient.put<ApiResponse<WorkType>>(`/worktypes/${id}`, workTypeData);
  return response.data;
};

// Delete a work type
export const deleteWorkType = async (id: string): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(`/worktypes/${id}`);
  return response.data;
};
