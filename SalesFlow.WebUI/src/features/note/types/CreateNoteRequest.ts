export interface CreateNoteRequest {
  content: string;

  customerId: number;

  createdById: number | null;
}