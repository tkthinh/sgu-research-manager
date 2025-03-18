import { ApiResponse } from "../types/common/ApiResponse";
import { Factor } from "../types/models/Factor";
import { ScoreLevel } from "../types/enums/ScoreLevel";
import apiClient from "./api";

export const getFactors = async (): Promise<ApiResponse<Factor[]>> => {
  const response = await apiClient.get<ApiResponse<Factor[]>>("/factors");
  return response.data;
};

export const getFactorsByWorkTypeId = async (workTypeId: string): Promise<ApiResponse<Factor[]>> => {
  const response = await apiClient.get<ApiResponse<Factor[]>>(`/factors/work-type/${workTypeId}`);
  return response.data;
};

export const getFactorById = async (id: string): Promise<ApiResponse<Factor>> => {
  const response = await apiClient.get<ApiResponse<Factor>>(`/factors/${id}`);
  return response.data;
};

export const createFactor = async (factorData: Partial<Factor>): Promise<ApiResponse<Factor>> => {
  const response = await apiClient.post<ApiResponse<Factor>>("/factors", factorData);
  return response.data;
};

export const updateFactor = async (id: string, factorData: Partial<Factor>): Promise<ApiResponse<Factor>> => {
  const response = await apiClient.put<ApiResponse<Factor>>(`/factors/${id}`, factorData);
  return response.data;
};

export const deleteFactor = async (id: string): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(`/factors/${id}`);
  return response.data;
};

export interface FindFactorParams {
  workTypeId: string;
  workLevelId?: string | null;
  purposeId: string;
  authorRoleId?: string | null;
  scoreLevel?: ScoreLevel | null;
}

export const findFactor = async (params: FindFactorParams): Promise<ApiResponse<Factor>> => {
  const queryParams = new URLSearchParams();
  queryParams.append("workTypeId", params.workTypeId);
  if (params.workLevelId) queryParams.append("workLevelId", params.workLevelId);
  queryParams.append("purposeId", params.purposeId);
  if (params.authorRoleId) queryParams.append("authorRoleId", params.authorRoleId);
  if (params.scoreLevel !== undefined && params.scoreLevel !== null) 
    queryParams.append("scoreLevel", params.scoreLevel.toString());

  const response = await apiClient.get<ApiResponse<Factor>>(`/factors/find?${queryParams.toString()}`);
  return response.data;
}; 