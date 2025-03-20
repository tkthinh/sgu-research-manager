import { ScoreLevel } from "../enums/ScoreLevel";

export interface Factor {
  id: string;
  workTypeId: string;
  workTypeName?: string;
  workLevelId?: string;
  workLevelName?: string;
  purposeId: string;
  purposeName?: string;
  authorRoleId?: string;
  authorRoleName?: string;
  name?: string;
  scoreLevel: ScoreLevel;
  convertHour: number;
  maxAllowed: number;
  createdDate: string;
  modifiedDate: string | null;
} 