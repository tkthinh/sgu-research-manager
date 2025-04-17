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

export const markNotificationAsRead = async (notificationId: string): Promise<ApiResponse<Notification>> => {
  const response = await apiClient.put<ApiResponse<Notification>>(`/notifications/${notificationId}`);
  return response.data;
}
