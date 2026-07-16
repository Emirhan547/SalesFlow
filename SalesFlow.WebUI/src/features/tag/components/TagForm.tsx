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

import FormField from "@/components/common/FormField";
import FormSection from "@/components/common/FormSection";

import {
  tagSchema,
  type TagFormData,
} from "../schemas/tagSchema";

type Props = {
  submitText: string;

  defaultValues?: TagFormData;

  onSubmit: (
    data: TagFormData
  ) => Promise<void>;
};

function TagForm({
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

  } = useForm<TagFormData>({
    resolver:
      zodResolver(tagSchema),

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
      onSubmit={handleSubmit(onSubmit)}
      className="space-y-6"
    >

      <FormSection title="Tag Information">

        <div className="grid gap-6 md:grid-cols-2">

          <FormField
            label="Name"
            error={errors.name?.message}
          >
            <Input
              {...register("name")}
            />
          </FormField>

          <FormField
            label="Color"
            error={errors.color?.message}
          >
            <Input
              type="color"
              {...register("color")}
            />
          </FormField>

        </div>

      </FormSection>

      <Button
        type="submit"
        disabled={isSubmitting}
      >
        {submitText}
      </Button>

    </form>

  );
}

export default TagForm;