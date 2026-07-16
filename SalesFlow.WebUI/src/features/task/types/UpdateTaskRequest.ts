import type { TaskPriority } from "./TaskPriority";
import type { TaskStatus } from "./TaskStatus";

export interface UpdateTaskRequest {

  id: number;

  title: string;

  description?: string;

  dueDate: string;

  priority: TaskPriority;

  status: TaskStatus;

  customerId: number;

  assignedUserId: number | null;

}