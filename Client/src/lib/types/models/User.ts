export interface User {
  id: string;
  userName: string;
  email: string;
  fullName: string;
  academicTitle: string;
  officerRank: string;
  departmentId: string;
  fieldId: string;
  departmentName?: string;
  fieldName?: string;
  createdAt: string;
  updatedAt?: string;
  identityId?: string;
  role: string;
}