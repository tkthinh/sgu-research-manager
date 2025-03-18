import { ApiResponse } from "../types/common/ApiResponse";
import { Field } from "../types/models/Field";
import apiClient from "./api";

// Fetch all fields
export const getFields = async (): Promise<ApiResponse<Field[]>> => {
  const response = await apiClient.get<ApiResponse<Field[]>>("/fields");
  return response.data;
};

// Fetch field by ID
export const getFieldById = async (id: string): Promise<ApiResponse<Field>> => {
  const response = await apiClient.get<ApiResponse<Field>>(`/fields/${id}`);
  return response.data;
};

// Create a new field
export const createField = async (fieldData: Partial<Field>): Promise<ApiResponse<Field>> => {
  const response = await apiClient.post<ApiResponse<Field>>("/fields", fieldData);
  return response.data;
};

// Update a field
export const updateField = async (id: string, fieldData: Partial<Field>): Promise<ApiResponse<Field>> => {
  const response = await apiClient.put<ApiResponse<Field>>(`/fields/${id}`, fieldData);
  return response.data;
};

// Delete a field
export const deleteField = async (id: string): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(`/fields/${id}`);
  return response.data;
};
