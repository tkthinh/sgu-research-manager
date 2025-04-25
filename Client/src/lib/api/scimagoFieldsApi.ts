import { ApiResponse } from "../types/common/ApiResponse";
import { ScimagoField } from "../types/models/ScimagoField";
import apiClient from "./api";

export const getScimagoFields = async (): Promise<ApiResponse<ScimagoField[]>> => {
  const response = await apiClient.get<ApiResponse<ScimagoField[]>>("/scimagofields");
  return response.data;
};

export const getScimagoFieldById = async (id: string): Promise<ApiResponse<ScimagoField>> => {
  const response = await apiClient.get<ApiResponse<ScimagoField>>(`/scimagofields/${id}`);
  return response.data;
};

export const createScimagoField = async (data: Partial<ScimagoField>): Promise<ApiResponse<ScimagoField>> => {
  const response = await apiClient.post<ApiResponse<ScimagoField>>("/scimagofields", data);
  return response.data;
};

export const updateScimagoField = async (id: string, data: Partial<ScimagoField>): Promise<ApiResponse<ScimagoField>> => {
  const response = await apiClient.put<ApiResponse<ScimagoField>>(`/scimagofields/${id}`, data);
  return response.data;
};

export const deleteScimagoField = async (id: string): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(`/scimagofields/${id}`);
  return response.data;
};
