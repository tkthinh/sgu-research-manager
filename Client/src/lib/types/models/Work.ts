import { WorkSource } from "../enums/WorkSource";
import { Author, UpdateAuthorRequest } from "./Author";

export interface Work {
  id: string;
  title: string;
  timePublished?: string;
  totalAuthors?: number;
  totalMainAuthors?: number;
  details?: Record<string, string>;
  source: WorkSource;
  isLocked: boolean;
  workTypeId: string;
  workLevelId?: string;
  workTypeName?: string;
  workLevelName?: string;

  academicYearId?: string;
  academicYearName?: string;
  exchangeDeadline?: string;


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
  authors?: Author[];
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
  academicYearId?: string;
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
  source: number;
  workTypeId?: string;
  workLevelId?: string;
  academicYearId?: string;
  coAuthorUserIds: string[];
}

export interface UpdateWorkWithAuthorRequest {
  workRequest?: UpdateWorkRequest;
  authorRequest?: UpdateAuthorRequest;
}
