import { ApiResponse } from "../types/common/ApiResponse";
import { Work, CreateWorkRequest, UpdateWorkWithAuthorRequest, WorkFilter } from "../types/models/Work";
import apiClient from "./api";

// Fetch all works with filter
export const getWorksWithFilter = async (filter: WorkFilter): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>("/works/filter", { params: filter });
  return response.data;
};

// Fetch work by ID
export const getWorkById = async (id: string): Promise<ApiResponse<Work>> => {
  const response = await apiClient.get<ApiResponse<Work>>(`/works/${id}`);
  return response.data;
};

// Create a new work
export const createWork = async (requestData: CreateWorkRequest): Promise<ApiResponse<Work>> => {
  const response = await apiClient.post<ApiResponse<Work>>("/works", requestData);
  return response.data;
};

// Delete a work
export const deleteWork = async (id: string): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(`/works/${id}`);
  return response.data;
};

// Update work by admin
export const updateWorkByAdmin = async (
  workId: string,
  userId: string,
  requestData: UpdateWorkWithAuthorRequest
): Promise<ApiResponse<Work>> => {
  const response = await apiClient.patch<ApiResponse<Work>>(
    `/works/${workId}/admin-update/${userId}`,
    requestData
  );
  return response.data;
};

// Update work by author
export const updateWorkByAuthor = async (
  workId: string,
  requestData: UpdateWorkWithAuthorRequest
): Promise<ApiResponse<Work>> => {
  const response = await apiClient.patch<ApiResponse<Work>>(`/works/${workId}`, requestData);
  return response.data;
};

// Register work by author
export const registerWorkByAuthor = async (
  authorId: string,
  registered: boolean
): Promise<ApiResponse<object>> => {
  const response = await apiClient.patch<ApiResponse<object>>(
    `/works/authors/${authorId}/register`,
    registered
  );
  return response.data;
};

// Update work status
export const updateWorkStatus = async (
  workId: string,
  authorId: string,
  status: number
): Promise<ApiResponse<Work>> => {
  const data = {
    proofStatus: status
  };
  const response = await apiClient.patch<ApiResponse<Work>>(`/works/${workId}/admin-update/${authorId}`, data);
  return response.data;
};

// Update work note
export const updateWorkNote = async (
  workId: string,
  authorId: string,
  note: string
): Promise<ApiResponse<Work>> => {
  const data = {
    note: note
  };
  const response = await apiClient.patch<ApiResponse<Work>>(`/works/${workId}/admin-update/${authorId}`, data);
  return response.data;
};

