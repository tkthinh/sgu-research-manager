import { ApiResponse } from "../types/common/ApiResponse";
import { SystemConfig } from "../types/models/SystemConfig";
import apiClient from "./api";

export const getSystemConfigs = async (): Promise<
  ApiResponse<SystemConfig[]>
> => {
  const response =
    await apiClient.get<ApiResponse<SystemConfig[]>>("/systemConfigs");
  return response.data;
};

export const getSystemConfigById = async (
  id: string,
): Promise<ApiResponse<SystemConfig>> => {
  const response = await apiClient.get<ApiResponse<SystemConfig>>(
    `/systemConfigs/${id}`,
  );
  return response.data;
};

export const getSystemConfigByAcademicYearId = async (
  academicYearId: string,
): Promise<ApiResponse<SystemConfig[]>> => {
  const response = await apiClient.get<ApiResponse<SystemConfig[]>>(
    `/systemConfigs/year/${academicYearId}`,
  );
  return response.data;
};

export const createSystemConfig = async (
  data: Partial<SystemConfig>,
): Promise<ApiResponse<SystemConfig>> => {
  const response = await apiClient.post<ApiResponse<SystemConfig>>(
    "/systemConfigs",
    data,
  );
  return response.data;
};

export const updateSystemConfig = async (
  id: string,
  data: Partial<SystemConfig>,
): Promise<ApiResponse<SystemConfig>> => {
  const response = await apiClient.put<ApiResponse<SystemConfig>>(
    `/systemConfigs/${id}`,
    data,
  );
  return response.data;
};

export const deleteSystemConfig = async (
  id: string,
): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(
    `/systemConfigs/${id}`,
  );
  return response.data;
};
