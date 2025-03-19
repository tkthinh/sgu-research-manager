import { ApiResponse } from "../types/common/ApiResponse";
import { Author } from "../types/models/Author";
import apiClient from "./api";

export const getAuthors = async (): Promise<ApiResponse<Author[]>> => {
  const response = await apiClient.get<ApiResponse<Author[]>>("/authors");
  return response.data;
};

export const getAuthorById = async (id: string): Promise<ApiResponse<Author>> => {
  const response = await apiClient.get<ApiResponse<Author>>(`/authors/${id}`);
  return response.data;
};

export const createAuthor = async (authorData: Partial<Author>): Promise<ApiResponse<Author>> => {
  const response = await apiClient.post<ApiResponse<Author>>("/authors", authorData);
  return response.data;
};

export const deleteAuthor = async (id: string): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(`/authors/${id}`);
  return response.data;
}; 