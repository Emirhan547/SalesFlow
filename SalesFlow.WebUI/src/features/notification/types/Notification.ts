import type { NotificationType } from "./NotificationType";

export interface Notification {
  id: number;

  title: string;

  message: string;

  type: NotificationType;

  isRead: boolean;

  entityName: string | null;

  entityId: number | null;

  createdDate: string;
}