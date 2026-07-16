export const TaskStatus = {
  Pending: 1,
  InProgress: 2,
  Completed: 3,
  Cancelled: 4,
} as const;

export type TaskStatus =
  (typeof TaskStatus)[keyof typeof TaskStatus];