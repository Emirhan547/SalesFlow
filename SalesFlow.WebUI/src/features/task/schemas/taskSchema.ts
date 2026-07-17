import { z } from "zod";

import { TaskPriority } from "../types/TaskPriority";
import { TaskStatus } from "../types/TaskStatus";

export const taskSchema = z.object({

  title: z
    .string()
    .min(2, "Title is required."),

  description: z
    .string()
    .optional(),

  dueDate: z
    .string()
    .min(1, "Due date is required."),

  priority: z.union([
    z.literal(TaskPriority.Low),
    z.literal(TaskPriority.Medium),
    z.literal(TaskPriority.High),
    z.literal(TaskPriority.Critical),
  ]),

  customerId: z
    .number(),

  assignedUserId: z
    .number()
    .nullable(),

  status: z
    .union([
      z.literal(TaskStatus.Pending),
      z.literal(TaskStatus.InProgress),
      z.literal(TaskStatus.Completed),
      z.literal(TaskStatus.Cancelled),
    ])
    .optional(),

});

export type TaskFormData =
  z.infer<typeof taskSchema>;