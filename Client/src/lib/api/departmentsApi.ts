import { ApiResponse } from "../types/common/ApiResponse";
import { Department } from "../types/models/Department";
import apiClient from "./api";

// Fetch all departments
export const getDepartments = async (): Promise<ApiResponse<Department[]>> => {
  const response = await apiClient.get<ApiResponse<Department[]>>("/departments");
  return response.data;
};

// Fetch department by ID
export const getDepartmentById = async (id: string): Promise<ApiResponse<Department>> => {
  const response = await apiClient.get<ApiResponse<Department>>(`/departments/${id}`);
  return response.data;
};

// Create a new department
export const createDepartment = async (departmentData: Partial<Department>) => {
  const response = await apiClient.post("/departments", departmentData);
  return response.data;
};

// Update a department
export const updateDepartment = async (id: string, departmentData: Partial<Department>) => {
  const response = await apiClient.put(`/departments/${id}`, departmentData);
  return response.data;
};

// Delete a department
export const deleteDepartment = async (id: string) => {
  const response = await apiClient.delete(`/departments/${id}`);
  return response;
};
