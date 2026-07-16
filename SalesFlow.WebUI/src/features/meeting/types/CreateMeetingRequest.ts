import type { MeetingType } from "./MeetingType";

export interface CreateMeetingRequest {
  title: string;

  description?: string;

  startDate: string;

  endDate: string;

  type: MeetingType;

  location?: string;

  customerId: number;

  assignedUserId: number | null;
}