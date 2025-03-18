import { ProofStatus } from "../enums/ProofStatus";
import { WorkSource } from "../enums/WorkSource";

export interface Work {
  id: string;
  title: string;
  timePublished?: string;
  totalAuthors?: number;
  totalMainAuthors?: number;
  details?: Record<string, string>;
  source: WorkSource;
  workTypeId: string;
  workLevelId?: string;
  workTypeName?: string;
  workLevelName?: string;
  createdDate: string;
  modifiedDate: string | null;
  coAuthorUserIds: string[];
  author?: {
    authorRoleId: string;
    purposeId: string;
    position?: number;
    scoreLevel?: number;
    scImagoFieldId?: string;
    fieldId?: string;
  };
}

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
  markedForScoring: boolean;
  proofStatus: ProofStatus;
  note?: string | null;
  createdDate: string;
  modifiedDate?: string | null;
}

export interface CreateWorkRequest {
  title: string;
  timePublished?: string;
  totalAuthors?: number;
  totalMainAuthors?: number;
  details?: Record<string, string>;
  source: number;
  workTypeId: string;
  workLevelId?: string;
  author: {
    authorRoleId: string;
    purposeId: string;
    position?: number;
    scoreLevel?: number;
    scImagoFieldId?: string;
    fieldId?: string;
  };
  coAuthorUserIds: string[];
}

export interface UpdateWorkRequest {
  title?: string;
  timePublished?: string;
  totalAuthors?: number;
  totalMainAuthors?: number;
  details?: Record<string, string>;
  source?: number;
  workTypeId?: string;
  workLevelId?: string;
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

export interface AddCoAuthorRequest {
  workRequest?: UpdateWorkRequest;
  authorRequest: {
    authorRoleId: string;
    purposeId: string;
    position?: number;
    scoreLevel?: number;
    scImagoFieldId?: string;
    fieldId?: string;
  };
}

export interface UpdateWorkByAuthorRequest {
  workRequest?: UpdateWorkRequest;
  authorRequest?: UpdateAuthorRequest;
}

export interface UpdateWorkByAdminRequest {
  workRequest?: UpdateWorkRequest;
  authorRequest?: UpdateAuthorRequest;
} 