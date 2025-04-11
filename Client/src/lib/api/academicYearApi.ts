import { ApiResponse } from "../types/common/ApiResponse";
import { AcademicYear } from "../types/models/AcademicYear";
import apiClient from "./api";

export const getAcademicYears = async (): Promise<
  ApiResponse<AcademicYear[]>
> => {
  const response =
    await apiClient.get<ApiResponse<AcademicYear[]>>("/academicYears");
  return response.data;
};

export const getAcademicYearById = async (
  id: string,
): Promise<ApiResponse<AcademicYear>> => {
  const response = await apiClient.get<ApiResponse<AcademicYear>>(
    `/academicYears/${id}`,
  );
  return response.data;
};

export const getCurrentAcademicYear = async (): Promise<
  ApiResponse<AcademicYear>
> => {
  const response = await apiClient.get<ApiResponse<AcademicYear>>(
    "/academicYears/current",
  );
  return response.data;
};

export const createAcademicYear = async (
  data: Partial<AcademicYear>,
): Promise<ApiResponse<AcademicYear>> => {
  const response = await apiClient.post<ApiResponse<AcademicYear>>(
    "/academicYears",
    data,
  );
  return response.data;
};

export const updateAcademicYear = async (
  id: string,
  data: Partial<AcademicYear>,
): Promise<ApiResponse<AcademicYear>> => {
  const response = await apiClient.put<ApiResponse<AcademicYear>>(
    `/academicYears/${id}`,
    data,
  );
  return response.data;
};

export const deleteAcademicYear = async (
  id: string,
): Promise<ApiResponse<boolean>> => {
  const response = await apiClient.delete<ApiResponse<boolean>>(
    `/academicYears/${id}`,
  );
  return response.data;
};
