export interface AuthorRole {
  id: string;
  name: string;
  workTypeId: string;
  workTypeName?: string;
  isMainAuthor: boolean;
  createdDate: string;
  modifiedDate: string | null;
} 