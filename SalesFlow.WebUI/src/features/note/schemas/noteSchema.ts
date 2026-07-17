import { z } from "zod";

export const noteSchema =
  z.object({

    content:
      z.string()
        .min(3)
        .max(2000),

    customerId:
      z.number().min(1),

   

  });

export type NoteFormData =
  z.infer<typeof noteSchema>;