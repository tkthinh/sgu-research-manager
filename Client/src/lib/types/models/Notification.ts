export interface Notification {
  id: string;
  content: string;
  isGlobal: boolean;
  userId?: string;
  isRead?: boolean;
  createdDate: Date;
}