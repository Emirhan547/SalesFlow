import { z } from "zod";

export const leadSchema = z.object({

  firstName: z
    .string()
    .min(2, "First name is required."),

  lastName: z
    .string()
    .min(2, "Last name is required."),

  companyName: z
    .string()
    .optional(),

  email: z
    .email("Invalid email address."),

  phoneNumber: z
    .string()
    .min(10, "Phone number is required."),

  website: z
    .string()
    .optional(),

  address: z
    .string()
    .optional(),

  description: z
    .string()
    .optional(),

  status: z.number(),

  source: z.number(),

  assignedUserId: z
    .number()
    .nullable(),

});

export type LeadFormData =
  z.infer<typeof leadSchema>;