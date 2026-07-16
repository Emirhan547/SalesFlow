import type {
  FieldErrors,
  UseFormRegister,
} from "react-hook-form";

import FormField from "@/components/common/FormField";
import FormSection from "@/components/common/FormSection";

import { Input } from "@/components/ui/input";
import { Select } from "@/components/ui/select";

import type { DealFormData } from "../../schemas/dealSchema";

type Props = {
  register: UseFormRegister<DealFormData>;
  errors: FieldErrors<DealFormData>;
};

function DealInformationSection({
  register,
  errors,
}: Props) {
  return (
    <FormSection title="Deal Information">

      <div className="grid gap-6 md:grid-cols-2">

        <FormField
          label="Title"
          error={errors.title?.message}
        >
          <Input
            placeholder="Website Redesign"
            {...register("title")}
          />
        </FormField>

        <FormField
          label="Amount"
          error={errors.amount?.message}
        >
          <Input
            type="number"
            step="0.01"
            {...register("amount", {
              valueAsNumber: true,
            })}
          />
        </FormField>

        <FormField
          label="Expected Close Date"
          error={errors.expectedCloseDate?.message}
        >
          <Input
            type="date"
            {...register("expectedCloseDate")}
          />
        </FormField>

        <FormField
          label="Stage"
        >
          <Select disabled>

            <option>
              New
            </option>

          </Select>

        </FormField>

      </div>

    </FormSection>
  );
}

export default DealInformationSection;