import { User } from "../models/User";
import { ApiResponse } from "./ApiResponse";

export interface SignUpRequest {
  username: string;
  password: string;
  email: string;
  phoneNumber: string;
  fullname: string;
  academicTitle: string;
  officerRank: string;
  specialization: string;
  departmentId: string;
  fieldId: string;
}

export interface SignUpResponse extends ApiResponse<User> {}

export interface SignInRequest {
  username: string;
  password: string;
}

export interface SignInResponse {
  success: boolean;
  message: string;
  data: {
    token: string;
    expiration: string;
    user: User;
  };
}
