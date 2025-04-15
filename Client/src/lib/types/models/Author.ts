import { ProofStatus } from "../enums/ProofStatus";
import { AuthorRegistration } from "./AuthorRegistration";

export interface Author {
  id: string;
  workId: string;
  userId: string;
  authorRoleId: string;
  authorRoleName: string;
  purposeId: string;
  purposeName: string;
  scImagoFieldId?: string;
  scImagoFieldName?: string;
  fieldId?: string;
  fieldName?: string;
  position?: number;
  scoreLevel?: number;
  authorHour?: number;
  workHour?: number;
  proofStatus: ProofStatus;
  note?: string | null;
  authorRegistration?: AuthorRegistration | null;
  createdDate: string;
  modifiedDate?: string | null;
}

export interface UpdateAuthorRequest {
  authorRoleId?: string;
  purposeId?: string;
  position?: number;
  scoreLevel?: number;
  proofStatus?: ProofStatus;
  note?: string;
  scImagoFieldId?: string;
  fieldId?: string;
}

export interface CreateAuthorRequest {
  authorRoleId?: string;
  purposeId: string;
  position?: number;
  scoreLevel?: number;
  scImagoFieldId?: string;
  fieldId?: string;
}