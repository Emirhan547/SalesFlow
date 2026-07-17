export const NotificationTypes = {
  Info: 1,
  Success: 2,
  Warning: 3,
  Reminder: 4,
} as const;

export type NotificationType =
  (typeof NotificationTypes)[keyof typeof NotificationTypes];