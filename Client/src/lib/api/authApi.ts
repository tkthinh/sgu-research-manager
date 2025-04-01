import {
  SignInRequest,
  SignInResponse,
  SignUpRequest,
  SignUpResponse,
} from "../types/common/Auth";
import apiClient from "./api";

export const signUp = async (data: SignUpRequest): Promise<SignUpResponse> => {
  const response = await apiClient.post<SignUpResponse>("/auth/register", data);
  return response.data;
};

export const signIn = async (data: SignInRequest): Promise<SignInResponse> => {
  const response = await apiClient.post<SignInResponse>("/auth/login", data);
  return response.data;
};

export const changePassword = async (currentPassword: string, newPassword: string) => {
  const response = await apiClient.post("/auth/change-password", {
    currentPassword,
    newPassword,
  });
  return response.data;
}