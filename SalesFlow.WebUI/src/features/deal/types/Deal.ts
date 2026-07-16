import type { DealStage } from "./DealStage";

export interface Deal {
  id: number;

  title: string;

  description?: string;

  amount: number;

  expectedCloseDate?: string;

  stage: DealStage;

  customerId: number;

  assignedUserId?: number | null;
}