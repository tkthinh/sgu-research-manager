import { ApiResponse } from "../types/common/ApiResponse";
import { Notification } from "../types/models/Notification";
import apiClient from "./api";

export const getGlobalNotifications = async (): Promise<ApiResponse<Notification[]>> => {
  const response = await apiClient.get<ApiResponse<Notification[]>>("/notifications/global");
  return response.data;
};

export const getMyNotifications = async (): Promise<ApiResponse<Notification[]>> => {
  const response = await apiClient.get<ApiResponse<Notification[]>>("/notifications/me");
  return response.data;
};
