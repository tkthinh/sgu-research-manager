export interface WorkType {
  id: string;
  name: string;
  workLevelCount?: string,
  createdAt: string;
  updatedAt: string;
}

interface BaseInfo {
  id: string;
  name: string;
}

export interface WorkTypeWithDetails {
  id: string;
  name: string;
  workLevelCount: number;
  purposeCount: number;
  authorRoleCount: number;
  factorCount: number;
  scImagoFieldCount: number;
  workLevels: BaseInfo[];
  purposes: BaseInfo[];
  authorRoles: BaseInfo[];
  scImagoFields: BaseInfo[];
  createdDate: string;
  modifiedDate: string | null;
}