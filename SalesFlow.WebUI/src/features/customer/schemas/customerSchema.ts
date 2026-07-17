import { z } from "zod";

import { CustomerTypes } from "../types/CustomerType";

export const customerSchema = z.object({
  customerType: z.union([
    z.literal(CustomerTypes.Individual),
    z.literal(CustomerTypes.Corporate),
  ]),

  companyName: z.string().optional(),

  contactFirstName: z
    .string()
    .min(2, "First name is required"),

  contactLastName: z
    .string()
    .min(2, "Last name is required"),

  email: z
    .email("Invalid email"),

  phoneNumber: z
    .string()
    .min(10, "Phone number is required"),

  website: z.string().optional(),

  taxNumber: z.string().optional(),

  address: z.string().optional(),

  description: z.string().optional(),

  assignedUserId: z
    .number()
    .nullable(),
}).superRefine((data, ctx) => {

  if (
    data.customerType === CustomerTypes.Corporate &&
    !data.companyName?.trim()
  ) {
    ctx.addIssue({
      code: z.ZodIssueCode.custom,
      path: ["companyName"],
      message: "Company name is required for corporate customers.",
    });
  }

});

export type CustomerFormData =
  z.infer<typeof customerSchema>;