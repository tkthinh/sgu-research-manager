import { ApiResponse } from "../types/common/ApiResponse";
import { Assignment } from "../types/models/Assignment";
import apiClient from "./api";

// Fetch all work types
export const getAssignments = async (): Promise<ApiResponse<Assignment[]>> => {
  const response = await apiClient.get<ApiResponse<Assignment[]>>("/assignments");
  return response.data;
};

// Fetch work type by ID
export const getAssignmentById = async (id: string): Promise<ApiResponse<Assignment>> => {
  const response = await apiClient.get<ApiResponse<Assignment>>(`/assignments/${id}`);
  return response.data;
};

// Create a new work type
export const createAssignment = async (AssignmentData: Partial<Assignment>): Promise<ApiResponse<Assignment>> => {
  const response = await apiClient.post<ApiResponse<Assignment>>("/assignments", AssignmentData);
  return response.data;
};

// Update a work type
export const updateAssignment = async (id: string, AssignmentData: Partial<Assignment>): Promise<ApiResponse<Assignment>> => {
  const response = await apiClient.put<ApiResponse<Assignment>>(`/assignments/${id}`, AssignmentData);
  return response.data;
};

// Delete a work type
export const deleteAssignment = async (id: string): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(`/assignments/${id}`);
  return response.data;
};
