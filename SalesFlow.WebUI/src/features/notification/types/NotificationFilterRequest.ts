import type { NotificationType } from "./NotificationType";

export interface NotificationFilterRequest {
  page?: number;

  pageSize?: number;

  type?: NotificationType;

  isRead?: boolean;
}