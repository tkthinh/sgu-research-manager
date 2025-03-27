import { ApiResponse } from "../types/common/ApiResponse";
import { Assignment } from "../types/models/Assignment";
import apiClient from "./api";

// Fetch all assignments
export const getAssignments = async (): Promise<ApiResponse<Assignment[]>> => {
  const response =
    await apiClient.get<ApiResponse<Assignment[]>>("/assignments");
  return response.data;
};

// Fetch all assignments of a manager
export const getAssignmentsOfManager = async (managerId: string): Promise<ApiResponse<Assignment[]>> => {
  const response =
    await apiClient.get<ApiResponse<Assignment[]>>(`/assignments/user/${managerId}`);
  return response.data;
}

// Create a new assignment
export const createAssignment = async (assignmentData: {
  managerId: string;
  departmentIds: string[];
}): Promise<ApiResponse<Assignment>> => {
  const response = await apiClient.post<ApiResponse<Assignment>>(
    "/assignments",
    assignmentData,
  );
  return response.data;
};

// Update assignments for a manager (updateAssignments)
export const updateAssignment = async (assignmentData: {
  managerId: string;
  departmentIds: string[];
}): Promise<ApiResponse<Assignment>> => {
  const response = await apiClient.put<ApiResponse<Assignment>>(
    "/assignments",
    assignmentData,
  );
  return response.data;
};

// Delete all assignments for a user (deleteAssignmentsOfUser)
export const deleteAssignmentsOfUser = async (
  managerId: string,
): Promise<ApiResponse<any>> => {
  const response = await apiClient.delete<ApiResponse<any>>(
    `/assignments/user/${managerId}`,
  );
  return response.data;
};
