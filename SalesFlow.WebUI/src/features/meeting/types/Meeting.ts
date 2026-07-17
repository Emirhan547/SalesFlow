import type { MeetingStatus } from "./MeetingStatus";
import type { MeetingType } from "./MeetingType";

export interface Meeting {
  id: number;

  title: string;

  description?: string;

  startDate: string;

  endDate: string;

  type: MeetingType;

  status: MeetingStatus;

  location?: string;

  customerId: number;

  assignedUserId: number | null;
  customerName: string;

assignedUserName: string | null;
  
}