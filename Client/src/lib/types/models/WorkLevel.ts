export interface WorkLevel {
  id: string;
  name: string;
  workTypeId: string;
  workTypeName?: string;
  createdDate: string;
  modifiedDate: string | null;
} 