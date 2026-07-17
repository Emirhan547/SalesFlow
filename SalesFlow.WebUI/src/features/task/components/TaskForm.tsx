import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

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

  return (

    <form
      onSubmit={
        handleSubmit(onSubmit)
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