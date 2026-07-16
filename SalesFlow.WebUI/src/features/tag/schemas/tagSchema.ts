import { z } from "zod";

export const tagSchema = z.object({

  name:
    z.string()
      .min(2, "Name is required.")
      .max(100),

  color:
    z.string()
      .optional(),

});

export type TagFormData =
  z.infer<typeof tagSchema>;