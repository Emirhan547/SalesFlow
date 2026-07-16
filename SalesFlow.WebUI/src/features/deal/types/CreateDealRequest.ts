export interface CreateDealRequest {
  title: string;

  description?: string;

  amount: number;

  expectedCloseDate?: string;

  customerId: number;

  assignedUserId: number | null;
}