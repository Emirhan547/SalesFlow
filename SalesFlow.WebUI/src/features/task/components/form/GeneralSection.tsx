import type {
  FieldErrors,
  UseFormRegister,
} from "react-hook-form";

import FormField from "@/components/common/FormField";
import FormSection from "@/components/common/FormSection";

import { Input } from "@/components/ui/input";
import { Select } from "@/components/ui/select";

import type { TaskFormData } from "../../schemas/taskSchema";

import {
  TaskStatus,
} from "../../types/TaskStatus";

type Props = {
  register: UseFormRegister<TaskFormData>;
  errors: FieldErrors<TaskFormData>;
  currentStatus?: number;
};

function GeneralSection({
  register,
  errors,
  currentStatus,
}: Props) {

  return (
    <FormSection title="Task Information">

      <div className="grid gap-6 md:grid-cols-2">

        <FormField
          label="Title"
          error={errors.title?.message}
        >
          <Input
            {...register("title")}
          />
        </FormField>

        <FormField
          label="Priority"
          error={errors.priority?.message}
        >
          <Select
            {...register("priority", {
              valueAsNumber: true,
            })}
          >
            <option value={1}>
              Low
            </option>

            <option value={2}>
              Medium
            </option>

            <option value={3}>
              High
            </option>

            <option value={4}>
              Critical
            </option>
          </Select>
        </FormField>

        {currentStatus !== undefined && (

          <FormField
            label="Status"
            error={errors.status?.message}
          >
            <Select
              {...register("status", {
                valueAsNumber: true,
              })}
            >

              {currentStatus ===
                TaskStatus.Pending && (
                <>
                  <option
                    value={TaskStatus.Pending}
                  >
                    Pending
                  </option>

                  <option
                    value={TaskStatus.InProgress}
                  >
                    In Progress
                  </option>

                  <option
                    value={TaskStatus.Cancelled}
                  >
                    Cancelled
                  </option>
                </>
              )}

              {currentStatus ===
                TaskStatus.InProgress && (
                <>
                  <option
                    value={TaskStatus.InProgress}
                  >
                    In Progress
                  </option>

                  <option
                    value={TaskStatus.Completed}
                  >
                    Completed
                  </option>

                  <option
                    value={TaskStatus.Cancelled}
                  >
                    Cancelled
                  </option>
                </>
              )}

            </Select>
          </FormField>

        )}

      </div>

    </FormSection>
  );
}

export default GeneralSection;