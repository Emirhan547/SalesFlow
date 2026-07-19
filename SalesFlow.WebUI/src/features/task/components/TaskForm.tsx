import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import { toast } from "sonner";

import GeneralSection from "./form/GeneralSection";
import DetailSection from "./form/DetailSection";
import AssignmentSection from "./form/AssignmentSection";
import FormButtons from "./form/FormButtons";

import {
  taskSchema,
  type TaskFormData,
} from "../schemas/taskSchema";

type Props = {
  submitText: string;

  loading?: boolean;

  defaultValues?: Partial<TaskFormData>;

  currentStatus?: number;

  onSubmit: (
    data: TaskFormData
  ) => Promise<void>;
};

function TaskForm({
  submitText,
  loading = false,
  defaultValues,
  currentStatus,
  onSubmit,
}: Props) {

  const {
    register,
    handleSubmit,
    formState: {
      errors,
    },
  } = useForm<TaskFormData>({
    resolver:
      zodResolver(taskSchema),

    defaultValues: {
      priority: 2,
      assignedUserId: null,
      ...defaultValues,
    },
  });

  async function submit(
    data: TaskFormData
  ) {

    try {

      await onSubmit(data);

    }
    catch (error) {

      if (error instanceof Error) {

        toast.error(error.message);

      }
      else {

        toast.error(
          "An unexpected error occurred."
        );

      }

    }

  }

  return (

    <form
      onSubmit={
        handleSubmit(submit)
      }
      className="space-y-8"
    >

      <GeneralSection
        register={register}
        errors={errors}
        currentStatus={currentStatus}
      />

      <DetailSection
        register={register}
        errors={errors}
      />

      <AssignmentSection
        register={register}
        errors={errors}
      />

      <FormButtons
        loading={loading}
        submitText={submitText}
      />

    </form>
  );
}

export default TaskForm;