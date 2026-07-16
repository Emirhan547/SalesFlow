import type {
  FieldErrors,
  UseFormRegister,
} from "react-hook-form";

import FormField from "@/components/common/FormField";
import FormSection from "@/components/common/FormSection";

import { Input } from "@/components/ui/input";
import { Select } from "@/components/ui/select";

import type { MeetingFormData } from "../../schemas/meetingSchema";

type Props = {
  register: UseFormRegister<MeetingFormData>;
  errors: FieldErrors<MeetingFormData>;
};

function GeneralSection({
  register,
  errors,
}: Props) {
  return (
    <FormSection title="General Information">

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
          label="Meeting Type"
          error={errors.type?.message}
        >
          <Select
            {...register("type", {
              valueAsNumber: true,
            })}
          >
            <option value={1}>Online</option>
            <option value={2}>Office</option>
            <option value={3}>Phone</option>
            <option value={4}>Customer Visit</option>
            <option value={5}>Other</option>
          </Select>
        </FormField>

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
              setValueAs: (v) =>
                v === "" ? null : Number(v),
            })}
          />
        </FormField>

      </div>

    </FormSection>
  );
}

export default GeneralSection;