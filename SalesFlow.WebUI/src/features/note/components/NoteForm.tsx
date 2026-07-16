import { useEffect } from "react";

import {
  useForm,
} from "react-hook-form";

import {
  zodResolver,
} from "@hookform/resolvers/zod";

import {
  Button,
} from "@/components/ui/button";

import {
  Input,
} from "@/components/ui/input";

import {
  Label,
} from "@/components/ui/label";

import {
  noteSchema,
  type NoteFormData,
} from "../schemas/noteSchema";

type Props = {
  submitText: string;

  defaultValues?: NoteFormData;

  onSubmit: (
    data: NoteFormData
  ) => Promise<void>;
};

function NoteForm({
  submitText,
  defaultValues,
  onSubmit,
}: Props) {

  const {

    register,

    handleSubmit,

    reset,

    formState: {
      errors,
      isSubmitting,
    },

  } = useForm<NoteFormData>({
    resolver:
      zodResolver(noteSchema),

    defaultValues,
  });

  useEffect(() => {

    if (defaultValues)
      reset(defaultValues);

  }, [
    defaultValues,
    reset,
  ]);

  return (

    <form
      onSubmit={
        handleSubmit(onSubmit)
      }
      className="space-y-6"
    >

      <div>

        <Label>
          Content
        </Label>

        <textarea

          rows={8}

          className="mt-2 w-full rounded-xl border border-slate-200 p-3"

          {...register("content")}

        />

        <p className="mt-1 text-sm text-red-500">
          {errors.content?.message}
        </p>

      </div>

      <div className="grid grid-cols-2 gap-6">

        <div>

          <Label>
            Customer Id
          </Label>

          <Input
            type="number"
            {...register(
              "customerId",
              {
                valueAsNumber: true,
              }
            )}
          />

        </div>

        <div>

          <Label>
            Created By
          </Label>

          <Input
            type="number"
            {...register(
              "createdById",
              {
                valueAsNumber: true,
              }
            )}
          />

        </div>

      </div>

      <Button
        disabled={
          isSubmitting
        }
        type="submit"
      >
        {submitText}
      </Button>

    </form>

  );
}

export default NoteForm;