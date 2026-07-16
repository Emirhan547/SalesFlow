export interface UpdateNoteRequest {
  id: number;

  content: string;

  customerId: number;

  createdById: number | null;
}