import { z } from "zod";

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

  priority: z
    .number(),

  customerId: z
    .number({
      required_error: "Customer is required.",
    }),

  assignedUserId: z
    .number()
    .nullable(),

  status: z
    .number()
    .optional(),

});

export type TaskFormData =
  z.infer<typeof taskSchema>;