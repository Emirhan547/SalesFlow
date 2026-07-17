import type { ActivityAction } from "./ActivityAction";

export interface ActivityLogFilterRequest {
  page: number;

  pageSize: number;

  action?: ActivityAction;

  entityName?: string;

  userId?: number;

  startDate?: string;

  endDate?: string;
}