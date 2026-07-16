import type { TaskPriority } from "./TaskPriority";

export interface CreateTaskRequest {

  title: string;

  description?: string;

  dueDate: string;

  priority: TaskPriority;

  customerId: number;

  assignedUserId: number | null;

}