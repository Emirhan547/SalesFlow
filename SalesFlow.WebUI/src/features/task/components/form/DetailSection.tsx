import type {
  FieldErrors,
  UseFormRegister,
} from "react-hook-form";

import FormField from "@/components/common/FormField";
import FormSection from "@/components/common/FormSection";

import { Input } from "@/components/ui/input";

import type { TaskFormData } from "../../schemas/taskSchema";

type Props = {
  register: UseFormRegister<TaskFormData>;
  errors: FieldErrors<TaskFormData>;
};

function DetailSection({
  register,
  errors,
}: Props) {

  return (
    <FormSection title="Task Details">

      <div className="grid gap-6">

        <FormField
          label="Description"
          error={errors.description?.message}
        >
          <textarea
            rows={5}
            {...register("description")}
            className="w-full rounded-xl border border-slate-200 p-3"
          />
        </FormField>

        <FormField
          label="Due Date"
          error={errors.dueDate?.message}
        >
          <Input
            type="datetime-local"
            {...register("dueDate")}
          />
        </FormField>

      </div>

    </FormSection>
  );
}

export default DetailSection;