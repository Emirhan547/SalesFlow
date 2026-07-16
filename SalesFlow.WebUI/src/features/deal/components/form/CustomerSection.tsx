import type {
  FieldErrors,
  UseFormRegister,
} from "react-hook-form";

import FormField from "@/components/common/FormField";
import FormSection from "@/components/common/FormSection";

import { Input } from "@/components/ui/input";

import type { DealFormData } from "../../schemas/dealSchema";

type Props = {
  register: UseFormRegister<DealFormData>;
  errors: FieldErrors<DealFormData>;
};

function CustomerSection({
  register,
  errors,
}: Props) {
  return (
    <FormSection title="Customer">

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
          error={errors.assignedUserId?.message}
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

export default CustomerSection;