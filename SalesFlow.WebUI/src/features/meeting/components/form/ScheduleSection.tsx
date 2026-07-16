import type {
  FieldErrors,
  UseFormRegister,
} from "react-hook-form";

import FormField from "@/components/common/FormField";
import FormSection from "@/components/common/FormSection";

import { Input } from "@/components/ui/input";

import type { MeetingFormData } from "../../schemas/meetingSchema";

type Props = {
  register: UseFormRegister<MeetingFormData>;
  errors: FieldErrors<MeetingFormData>;
};

function ScheduleSection({
  register,
  errors,
}: Props) {
  return (
    <FormSection title="Schedule">

      <div className="grid gap-6 md:grid-cols-2">

        <FormField
          label="Start Date"
          error={errors.startDate?.message}
        >
          <Input
            type="datetime-local"
            {...register("startDate")}
          />
        </FormField>

        <FormField
          label="End Date"
          error={errors.endDate?.message}
        >
          <Input
            type="datetime-local"
            {...register("endDate")}
          />
        </FormField>

        <FormField
          label="Location"
          error={errors.location?.message}
        >
          <Input
            {...register("location")}
          />
        </FormField>

      </div>

    </FormSection>
  );
}

export default ScheduleSection;