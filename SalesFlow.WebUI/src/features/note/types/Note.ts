export interface Note {
  id: number;

  content: string;

  customerId: number;
  customerName: string;

  createdById: number | null;
  createdByName: string | null;
}