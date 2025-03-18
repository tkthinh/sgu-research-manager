import { ApiResponse } from "../types/common/ApiResponse";
import { Work, CreateWorkRequest, UpdateWorkByAdminRequest, UpdateWorkByAuthorRequest } from "../types/models/Work";
import apiClient from "./api";

// Fetch all works
export const getWorks = async (): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>("/api/works");
  return response.data;
};

// Fetch work by ID
export const getWorkById = async (id: string): Promise<ApiResponse<Work>> => {
  const response = await apiClient.get<ApiResponse<Work>>(`/api/works/${id}`);
  return response.data;
};

// Create a new work
export const createWork = async (data: CreateWorkRequest): Promise<ApiResponse<Work>> => {
  const response = await apiClient.post<ApiResponse<Work>>("/api/works", data);
  return response.data;
};

// Delete a work
export const deleteWork = async (id: string): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(`/api/works/${id}`);
  return response.data;
};

// Get works by user ID
export const getWorksByUserId = async (userId: string): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>(`/api/works/user/${userId}`);
  return response.data;
};

// Get works by department ID
export const getWorksByDepartmentId = async (departmentId: string): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>(`/api/works/department/${departmentId}`);
  return response.data;
};

// Set marked for scoring
export const setMarkedForScoring = async (authorId: string, marked: boolean): Promise<ApiResponse<object>> => {
  const response = await apiClient.patch<ApiResponse<object>>(`/api/works/authors/${authorId}/mark`, marked);
  return response.data;
};

// Update work by admin
export const updateWorkByAdmin = async (workId: string, userId: string, data: UpdateWorkByAdminRequest): Promise<ApiResponse<Work>> => {
  const response = await apiClient.patch<ApiResponse<Work>>(`/api/works/${workId}/admin-update/${userId}`, data);
  return response.data;
};

// Update work by author
export const updateWorkByAuthor = async (id: string, data: UpdateWorkByAuthorRequest): Promise<ApiResponse<Work>> => {
  const response = await apiClient.patch<ApiResponse<Work>>(`/api/works/${id}/update-by-author`, data);
  return response.data;
};

// Get current user's works
export const getMyWorks = async (): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>("/api/works/my-works");
  return response.data;
};
