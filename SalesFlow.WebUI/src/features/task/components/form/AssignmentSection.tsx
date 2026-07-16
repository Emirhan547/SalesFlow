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

function AssignmentSection({
  register,
  errors,
}: Props) {

  return (
    <FormSection title="Assignment">

      <div className="grid gap-6 md:grid-cols-2">

        <FormField
          label="Customer Id"
          error={errors.customerId?.message}
        >
          <Input
            type="number"
            {...register("customerId", {
              valueAsNumber: true,
            })}
          />
        </FormField>

        <FormField
          label="Assigned User Id"
        >
          <Input
            type="number"
            {...register("assignedUserId", {
              valueAsNumber: true,
            })}
          />
        </FormField>

      </div>

    </FormSection>
  );
}

export default AssignmentSection;