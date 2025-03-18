import { AcademicTitle } from "../enums/AcademicTitle";
import { OfficerRank } from "../enums/OfficerRank";

export interface User {
  id: string;
  username: string;
  email: string;
  fullname: string;
  academicTitle: AcademicTitle;
  officerRank: OfficerRank;
  departmentId: string;
  fieldId: string;
  createdAt: string;
  updatedAt?: string;
  identityId: string;
  role: string;
}