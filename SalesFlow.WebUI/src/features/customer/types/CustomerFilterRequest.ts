export interface CustomerFilterRequest {
  page: number;

  pageSize: number;

  search?: string;

  customerType?: number;

  assignedUserId?: number;
}