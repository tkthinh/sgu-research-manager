export interface SystemConfig {
  id: string;
  startDate: string;
  endDate: string;
  isClosed: boolean;
  createdDate: string;
  modifiedDate: string | null;
}

export interface CreateSystemConfigRequest {
  startDate: string;
  endDate: string;
  isClosed: boolean;
}

export interface UpdateSystemConfigRequest {
  startDate: string;
  endDate: string;
  isClosed: boolean;
} 