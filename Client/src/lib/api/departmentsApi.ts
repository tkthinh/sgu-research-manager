import { ApiResponse } from "../types/common/ApiResponse";
import { Department } from "../types/models/Department";
import apiClient from "./api";

export const getDepartments = async (): Promise<ApiResponse<Department[]>> => {
  const response = await apiClient.get<ApiResponse<Department[]>>("/api/departments");
  return response.data;
};

export const getDepartmentById = async (id: string): Promise<ApiResponse<Department>> => {
  const response = await apiClient.get<ApiResponse<Department>>(`/api/departments/${id}`);
  return response.data;
};

export const createDepartment = async (data: Partial<Department>): Promise<ApiResponse<Department>> => {
  const response = await apiClient.post<ApiResponse<Department>>("/api/departments", data);
  return response.data;
};

export const updateDepartment = async (id: string, data: Partial<Department>): Promise<ApiResponse<Department>> => {
  const response = await apiClient.put<ApiResponse<Department>>(`/api/departments/${id}`, data);
  return response.data;
};

export const deleteDepartment = async (id: string): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(`/api/departments/${id}`);
  return response.data;
};
