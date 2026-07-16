export const TaskPriority = {
  Low: 1,
  Medium: 2,
  High: 3,
  Critical: 4,
} as const;

export type TaskPriority =
  (typeof TaskPriority)[keyof typeof TaskPriority];