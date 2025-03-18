import { ProofStatus } from "../enums/ProofStatus";
import { ScoreLevel } from "../enums/ScoreLevel";

export interface Author {
  id: string;
  workId: string;
  userId: string;
  authorRoleId: string;
  purposeId: string;
  scimagoFieldId?: string;
  fieldId?: string;
  position?: number;
  scoreLevel?: ScoreLevel;
  authorHour: number;
  workHour: number;
  markedForScoring: boolean;
  proofStatus: ProofStatus;
  note?: string;
  // Navigation properties
  fieldName?: string;
  scimagoFieldName?: string;
  userName?: string;
  authorRoleName?: string;
  purposeName?: string;
  createdDate: string;
  modifiedDate: string | null;
} 