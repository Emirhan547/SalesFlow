import { z } from "zod";

export const dealSchema = z.object({
  title: z
    .string()
    .trim()
    .min(2, "Title must be at least 2 characters."),

  description: z
    .string()
    .optional(),

  amount: z
    .number({
      error: "Amount is required.",
    })
    .min(0, "Amount cannot be negative."),

  expectedCloseDate: z
    .string()
    .optional(),

  stage: z
    .number()
    .min(1)
    .max(6),

  customerId: z
    .number({
      error: "Customer is required.",
    })
    .min(1, "Customer is required."),

  assignedUserId: z
    .number()
    .nullable(),
});

export type DealFormData =
  z.infer<typeof dealSchema>;