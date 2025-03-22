export interface User {
  id: string;
  userName: string;
  email: string;
  phoneNumber: string;
  fullName: string;
  academicTitle: string;
  officerRank: string;
  departmentId?: string;
  fieldId?: string;
  specialization: string;
  departmentName: string;
  fieldName: string;
  identityId?: string;
  role?: string;
  isApproved?: boolean;
}
