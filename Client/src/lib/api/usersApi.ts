import { ApiResponse } from "../types/common/ApiResponse";
import { User } from "../types/models/User";
import { UserConversionResult } from "../types/models/UserConversionResult";
import apiClient from "./api";

export const searchUsers = async (searchTerm: string): Promise<ApiResponse<User[]>> => {
  const response = await apiClient.get<ApiResponse<User[]>>(`/users/search?searchTerm=${encodeURIComponent(searchTerm)}`);
  return response.data;
};

export const getUserById = async (userId: string): Promise<ApiResponse<User>> => {
  const response = await apiClient.get<ApiResponse<User>>(`/users/${userId}`);
  return response.data;
};

export const getUsers = async (): Promise<ApiResponse<User[]>> => {
  const response = await apiClient.get<ApiResponse<User[]>>('/users');
  return response.data;
}

export const getMyInfo = async (): Promise<ApiResponse<User>> => {
  const response = await apiClient.get<ApiResponse<User>>('/users/me');
  return response.data;
}

export const updateUser = async (userId: string, data: Partial<User>): Promise<ApiResponse<User>> => {
  const response = await apiClient.put<ApiResponse<User>>(`/users/${userId}`, data);
  return response.data;
}

export const adminUpdateUser = async (userId: string, data: Partial<User>): Promise<ApiResponse<User>> => {
  const response = await apiClient.put<ApiResponse<User>>(`/users/admin/${userId}`, data);
  return response.data;
}

export const deleteUser = async (userId: string): Promise<ApiResponse<User>> => {
  const response = await apiClient.delete<ApiResponse<User>>(`/users/${userId}`);
  return response.data;
}; 

export const getUsersByDepartmentId = async (departmentId: string): Promise<ApiResponse<User[]>> => {
  const response = await apiClient.get<ApiResponse<User[]>>(`/users/department/${departmentId}`);
  return response.data;
}; 

export const getUserConversionResult = async (userId: string): Promise<ApiResponse<UserConversionResult>> => {
  const response = await apiClient.get<ApiResponse<UserConversionResult>>(`/users/conversionresult/${userId}`);
  return response.data;
}; 

export const importUserFromExcelFile = async (file: File): Promise<ApiResponse<object>> => {
  const formData = new FormData();
  formData.append('file', file);
  
  const response = await apiClient.post<ApiResponse<object>>('/users/import', formData, {
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  });
  return response.data;
}