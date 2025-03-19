export interface WorkAuthor {
  workId: string;
  userId: string;
  // Navigation properties
  work?: any;
  user?: any;
  createdDate: string;
  modifiedDate: string | null;
} 