import { ApiResponse } from "../types/common/ApiResponse";
import { AuthorRole } from "../types/models/AuthorRole";
import apiClient from "./api";

// Fetch all author roles
export const getAuthorRoles = async (): Promise<ApiResponse<AuthorRole[]>> => {
  const response = await apiClient.get<ApiResponse<AuthorRole[]>>("/authorroles");
  return response.data;
};

// Fetch author roles by work type ID
export const getAuthorRolesByWorkTypeId = async (workTypeId: string): Promise<ApiResponse<AuthorRole[]>> => {
  const response = await apiClient.get<ApiResponse<AuthorRole[]>>(`/authorroles/work-type/${workTypeId}`);
  return response.data;
};

// Fetch author role by ID
export const getAuthorRoleById = async (id: string): Promise<ApiResponse<AuthorRole>> => {
  const response = await apiClient.get<ApiResponse<AuthorRole>>(`/authorroles/${id}`);
  return response.data;
};

// Create a new author role
export const createAuthorRole = async (authorRoleData: Partial<AuthorRole>): Promise<ApiResponse<AuthorRole>> => {
  const response = await apiClient.post<ApiResponse<AuthorRole>>("/authorroles", authorRoleData);
  return response.data;
};

// Update an author role
export const updateAuthorRole = async (id: string, authorRoleData: Partial<AuthorRole>): Promise<ApiResponse<AuthorRole>> => {
  const response = await apiClient.put<ApiResponse<AuthorRole>>(`/authorroles/${id}`, authorRoleData);
  return response.data;
};

// Delete an author role
export const deleteAuthorRole = async (id: string): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(`/authorroles/${id}`);
  return response.data;
};
