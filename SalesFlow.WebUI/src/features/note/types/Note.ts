export interface Note {
  id: number;

  content: string;

  customerId: number;

  createdById: number | null;
}