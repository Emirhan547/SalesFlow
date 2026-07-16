import { z } from "zod";

export const meetingSchema = z
  .object({

    title: z
      .string()
      .min(2, "Title is required."),

    description: z
      .string()
      .optional(),

    startDate: z
      .string()
      .min(1, "Start date is required."),

    endDate: z
      .string()
      .min(1, "End date is required."),

    type: z.union([
      z.literal(1),
      z.literal(2),
      z.literal(3),
      z.literal(4),
      z.literal(5),
    ]),

    status: z
      .union([
        z.literal(1),
        z.literal(2),
        z.literal(3),
      ])
      .optional(),

    location: z
      .string()
      .optional(),

    customerId: z
      .number()
      .min(1, "Customer is required."),

    assignedUserId: z
      .number()
      .nullable(),

  })
  .refine(
    x => new Date(x.endDate) > new Date(x.startDate),
    {
      path: ["endDate"],
      message:
        "End date must be after start date.",
    }
  );

export type MeetingFormData =
  z.infer<typeof meetingSchema>;