import { ApiResponse } from "../types/common/ApiResponse";
import { Work, CreateWorkRequest, UpdateWorkByAdminRequest, UpdateWorkByAuthorRequest } from "../types/models/Work";
import apiClient from "./api";

// Fetch all works
export const getWorks = async (): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>("/works");
  return response.data;
};

// Fetch work by ID
export const getWorkById = async (id: string): Promise<ApiResponse<Work>> => {
  const response = await apiClient.get<ApiResponse<Work>>(`/works/${id}`);
  return response.data;
};

// Create a new work
export const createWork = async (data: CreateWorkRequest): Promise<ApiResponse<Work>> => {
  try {
    console.log("WorksApi:createWork - Dữ liệu gửi đi:", JSON.stringify(data, null, 2));
    const response = await apiClient.post<ApiResponse<Work>>("/works", data);
    console.log("WorksApi:createWork - Phản hồi:", JSON.stringify(response.data, null, 2));
    return response.data;
  } catch (error: any) {
    console.error("WorksApi:createWork - Chi tiết lỗi:", error.response?.data);
    console.error("WorksApi:createWork - Status code:", error.response?.status);
    throw error;
  }
};

// Delete a work
export const deleteWork = async (id: string): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(`/works/${id}`);
  return response.data;
};

// Get works by user ID
export const getWorksByUserId = async (userId: string): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>(`/works/user/${userId}`);
  return response.data;
};

// Get works by department ID
export const getWorksByDepartmentId = async (departmentId: string): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>(`/works/department/${departmentId}`);
  return response.data;
};

// Set marked for scoring
export const setMarkedForScoring = async (authorId: string, marked: boolean): Promise<ApiResponse<object>> => {
  const response = await apiClient.patch<ApiResponse<object>>(`/works/authors/${authorId}/mark`, marked);
  return response.data;
};

// Update work by admin
export const updateWorkByAdmin = async (workId: string, userId: string, data: UpdateWorkByAdminRequest): Promise<ApiResponse<Work>> => {
  const response = await apiClient.patch<ApiResponse<Work>>(`/works/${workId}/admin-update/${userId}`, data);
  return response.data;
};

// Update work by author
export const updateWorkByAuthor = async (id: string, data: UpdateWorkByAuthorRequest): Promise<ApiResponse<Work>> => {
  try {
    console.log("WorksApi:updateWorkByAuthor - Dữ liệu gửi đi:", JSON.stringify(data, null, 2));
    const response = await apiClient.patch<ApiResponse<Work>>(`/works/${id}/update-by-author`, data);
    console.log("WorksApi:updateWorkByAuthor - Phản hồi:", JSON.stringify(response.data, null, 2));
    return response.data;
  } catch (error: any) {
    console.error("WorksApi:updateWorkByAuthor - Chi tiết lỗi:", error.response?.data);
    console.error("WorksApi:updateWorkByAuthor - Status code:", error.response?.status);
    throw error;
  }
};

// Get current user's works
export const getMyWorks = async (): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>("/works/my-works");
  return response.data;
};
