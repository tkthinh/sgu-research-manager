import { ApiResponse } from "../types/common/ApiResponse";
import { Work, CreateWorkRequest, UpdateWorkWithAuthorRequest } from "../types/models/Work";
import apiClient from "./api";

// Fetch all works
export const getWorks = async (): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter();
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

// Get works by user ID
export const getWorksByUserId = async (userId: string): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({ userId });
};

// Get works by department ID
export const getWorksByDepartmentId = async (departmentId: string): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({ departmentId });
};

// Get works by system config ID (đợt kê khai)
export const getWorksBySystemConfigId = async (systemConfigId: string): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>(`/works/system-config/${systemConfigId}`);
  return response.data;
};

// Get works by academic year ID
export const getWorksByAcademicYearId = async (academicYearId: string): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({ academicYearId });
};

// Get current user's works by system config ID (đợt kê khai)
export const getCurrentUserWorksBySystemConfigId = async (systemConfigId: string): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>(`/works/my-works/system-config/${systemConfigId}`);
  return response.data;
};

// Get current user's works by academic year ID
export const getCurrentUserWorksByAcademicYearId = async (academicYearId: string): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({ academicYearId, isCurrentUser: true });
};

// Get current user's works by academic year ID and proof status
export const getCurrentUserWorksByAcademicYearAndProofStatus = async (academicYearId: string, proofStatus: number): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({ academicYearId, proofStatus, isCurrentUser: true });
};

// Get current user's works by academic year ID and source
export const getCurrentUserWorksByAcademicYearAndSource = async (academicYearId: string, source: number): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({ academicYearId, source, isCurrentUser: true });
};

// Get current user's works as coauthor by academic year ID
export const getCurrentUserCoAuthorWorksByAcademicYearId = async (academicYearId: string): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({ academicYearId, onlyCoAuthorWorks: true, isCurrentUser: true });
};

// Get current user's registered works
export const getRegisteredWorks = async (): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({ onlyRegisteredWorks: true, isCurrentUser: true });
};

// Get current user's works
export const getMyWorks = async (): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({ isCurrentUser: true });
};

// Get all works of current user (including imported works)
export const getAllMyWorks = async (): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({ isCurrentUser: true });
};

// Get all works of current user by academic year ID (including imported works)
export const getAllMyWorksByAcademicYearId = async (academicYearId: string): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({ academicYearId, isCurrentUser: true });
};

// Get marked works
export const getMarkedWorks = async (): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({ onlyRegisteredWorks: true, isCurrentUser: true });
};

// Get user works by academic year ID
export const getUserWorksByAcademicYearId = async (userId: string, academicYearId: string): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({ userId, academicYearId });
};

// Get works by department and academic year ID
export const getWorksByDepartmentAndAcademicYearId = async (departmentId: string, academicYearId: string): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({ departmentId, academicYearId });
};

// Get current user's registered works by academic year ID
export const getRegisteredWorksByAcademicYear = async (academicYearId: string): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({ academicYearId, onlyRegisteredWorks: true, isCurrentUser: true });
};

// Get works with filter
export const getWorksWithFilter = async (
  params?: {
    userId?: string;
    departmentId?: string;
    academicYearId?: string;
    proofStatus?: number;
    source?: number;
    onlyRegisteredWorks?: boolean;
    onlyCoAuthorWorks?: boolean;
    onlyRegisterableWorks?: boolean;
    isCurrentUser?: boolean;
  }
): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>("/works/filter", { params });
  return response.data;
};

// Get works that can be registered for conversion in current academic year
export const getRegisterableWorksByAcademicYearId = async (
  academicYearId: string
): Promise<ApiResponse<Work[]>> => {
  return await getWorksWithFilter({
    academicYearId,
    onlyRegisterableWorks: true,
    isCurrentUser: true
  });
};

// Set marked for scoring
export const setMarkedForScoring = async (authorId: string, marked: boolean): Promise<ApiResponse<object>> => {
  try {
    console.log(`WorksApi:setMarkedForScoring - Đánh dấu authorId ${authorId} với trạng thái ${marked}`);
    
    // In ra kiểu dữ liệu để kiểm tra
    console.log("WorksApi:setMarkedForScoring - Kiểu dữ liệu gửi đi:", typeof marked, marked);
    
    // Gửi giá trị boolean trực tiếp làm body request thay vì stringify nó
    const response = await apiClient.patch<ApiResponse<object>>(
      `/works/authors/${authorId}/mark`,
      marked
    );
    console.log("WorksApi:setMarkedForScoring - Phản hồi:", JSON.stringify(response.data, null, 2));
    return response.data;
  } catch (error: any) {
    console.error("WorksApi:setMarkedForScoring - Chi tiết lỗi:", error.response?.data);
    console.error("WorksApi:setMarkedForScoring - Status code:", error.response?.status);
    throw error;
  }
};

// Update work by admin
export const updateWorkByAdmin = async (workId: string, userId: string, requestData: UpdateWorkWithAuthorRequest): Promise<ApiResponse<Work>> => {
  const response = await apiClient.patch<ApiResponse<Work>>(`/works/${workId}/admin-update/${userId}`, requestData);
  return response.data;
};

// Update work by author
export const updateWorkByAuthor = async (workId: string, requestData: UpdateWorkWithAuthorRequest): Promise<ApiResponse<Work>> => {
  const response = await apiClient.patch<ApiResponse<Work>>(`/works/${workId}`, requestData);
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

