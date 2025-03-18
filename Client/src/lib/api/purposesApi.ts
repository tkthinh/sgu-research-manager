import { ApiResponse } from "../types/common/ApiResponse";
import { Purpose } from "../types/models/Purpose";
import apiClient from "./api";

// Fetch all purposes
export const getPurposes = async (): Promise<ApiResponse<Purpose[]>> => {
  const response = await apiClient.get<ApiResponse<Purpose[]>>("/purposes");
  return response.data;
};

// Fetch purposes by work type ID
export const getPurposesByWorkTypeId = async (workTypeId: string): Promise<ApiResponse<Purpose[]>> => {
  const response = await apiClient.get<ApiResponse<Purpose[]>>(`/purposes/work-type/${workTypeId}`);
  return response.data;
};

// Fetch purpose by ID
export const getPurposeById = async (id: string): Promise<ApiResponse<Purpose>> => {
  const response = await apiClient.get<ApiResponse<Purpose>>(`/purposes/${id}`);
  return response.data;
};

// Create a new purpose
export const createPurpose = async (purposeData: Partial<Purpose>): Promise<ApiResponse<Purpose>> => {
  const response = await apiClient.post<ApiResponse<Purpose>>("/purposes", purposeData);
  return response.data;
};

// Update a purpose
export const updatePurpose = async (id: string, purposeData: Partial<Purpose>): Promise<ApiResponse<Purpose>> => {
  const response = await apiClient.put<ApiResponse<Purpose>>(`/purposes/${id}`, purposeData);
  return response.data;
};

// Delete a purpose
export const deletePurpose = async (id: string): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(`/purposes/${id}`);
  return response.data;
};
