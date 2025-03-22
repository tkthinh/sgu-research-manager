import { ApiResponse } from "../types/common/ApiResponse";
import { User } from "../types/models/User";
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
}