import { ApiResponse } from "../types/common/ApiResponse";
import { SystemConfig, CreateSystemConfigRequest, UpdateSystemConfigRequest } from "../types/models/SystemConfig";
import apiClient from "./api";

// Lấy cấu hình hệ thống
export const getSystemConfig = async (): Promise<ApiResponse<SystemConfig>> => {
  try {
    const response = await apiClient.get<ApiResponse<SystemConfig>>("/systemconfigs");
    return response.data;
  } catch (error: any) {
    console.error("Lỗi khi lấy cấu hình hệ thống:", error.response?.data);
    throw error;
  }
};

// Tạo cấu hình hệ thống mới
export const createSystemConfig = async (data: CreateSystemConfigRequest): Promise<ApiResponse<boolean>> => {
  try {
    const response = await apiClient.post<ApiResponse<boolean>>("/systemconfigs", data);
    return response.data;
  } catch (error: any) {
    console.error("Lỗi khi tạo cấu hình hệ thống:", error.response?.data);
    throw error;
  }
};

// Cập nhật cấu hình hệ thống
export const updateSystemConfig = async (data: UpdateSystemConfigRequest): Promise<ApiResponse<boolean>> => {
  try {
    const response = await apiClient.put<ApiResponse<boolean>>("/systemconfigs", data);
    return response.data;
  } catch (error: any) {
    console.error("Lỗi khi cập nhật cấu hình hệ thống:", error.response?.data);
    throw error;
  }
}; 