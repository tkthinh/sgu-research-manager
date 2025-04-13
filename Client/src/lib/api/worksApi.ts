import { ApiResponse } from "../types/common/ApiResponse";
import { Work, CreateWorkRequest, UpdateWorkWithAuthorRequest } from "../types/models/Work";
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

// Get works by system config ID (đợt kê khai)
export const getWorksBySystemConfigId = async (systemConfigId: string): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>(`/works/system-config/${systemConfigId}`);
  return response.data;
};

// Get works by academic year ID
export const getWorksByAcademicYearId = async (academicYearId: string): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>(`/works/academic-year/${academicYearId}`);
  return response.data;
};

// Get current user's works by system config ID (đợt kê khai)
export const getCurrentUserWorksBySystemConfigId = async (systemConfigId: string): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>(`/works/my-works/system-config/${systemConfigId}`);
  return response.data;
};

// Get current user's works by academic year ID
export const getCurrentUserWorksByAcademicYearId = async (academicYearId: string): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>(`/works/my-works/academic-year/${academicYearId}`);
  return response.data;
};

// Set marked for scoring
export const setMarkedForScoring = async (authorId: string, marked: boolean): Promise<ApiResponse<object>> => {
  try {
    console.log(`WorksApi:setMarkedForScoring - Đánh dấu authorId ${authorId} với trạng thái ${marked}`);
    const response = await apiClient.patch<ApiResponse<object>>(`/works/authors/${authorId}/mark`, marked);
    console.log("WorksApi:setMarkedForScoring - Phản hồi:", JSON.stringify(response.data, null, 2));
    return response.data;
  } catch (error: any) {
    console.error("WorksApi:setMarkedForScoring - Chi tiết lỗi:", error.response?.data);
    console.error("WorksApi:setMarkedForScoring - Status code:", error.response?.status);
    throw error;
  }
};

// Update work by admin
export const updateWorkByAdmin = async (workId: string, userId: string, data: UpdateWorkWithAuthorRequest): Promise<ApiResponse<Work>> => {
  try {
    console.log("WorksApi:updateWorkByAdmin - Dữ liệu gửi đi:", JSON.stringify(data, null, 2));
    const response = await apiClient.patch<ApiResponse<Work>>(`/works/${workId}/admin-update/${userId}`, data);
    console.log("WorksApi:updateWorkByAdmin - Phản hồi:", JSON.stringify(response.data, null, 2));
    return response.data;
  } catch (error: any) {
    console.error("WorksApi:updateWorkByAdmin - Chi tiết lỗi:", error.response?.data);
    console.error("WorksApi:updateWorkByAdmin - Status code:", error.response?.status);
    throw error;
  }
};

// Update work by author
export const updateWorkByAuthor = async (id: string, data: UpdateWorkWithAuthorRequest): Promise<ApiResponse<Work>> => {
  try {
    console.log("WorksApi:updateWorkByAuthor - Dữ liệu gửi đi:", JSON.stringify(data, null, 2));
    const response = await apiClient.patch<ApiResponse<Work>>(`/works/${id}`, data);
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

// Get all works of current user (including imported works)
export const getAllMyWorks = async (): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>("/works/all-my-works");
  return response.data;
};

export const getMarkedWorks = async (): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>("/works/marked");
  return response.data;
};

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

