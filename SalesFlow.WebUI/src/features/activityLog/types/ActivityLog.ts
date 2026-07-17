import type { ActivityAction } from "./ActivityAction";

export interface ActivityLog {
  id: number;

  action: ActivityAction;

  entityName: string;

  entityId: number;

  description: string;

  userId: number | null;

  userName?: string;

  createdDate: string;
}