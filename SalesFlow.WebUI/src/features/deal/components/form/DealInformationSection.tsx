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
  currentStage?: number;
};

function DealInformationSection({
  register,
  errors,
  currentStage,
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
          error={errors.stage?.message}
        >
          <Select
            {...register("stage", {
              valueAsNumber: true,
            })}
          >
            {currentStage === undefined && (
              <option value={1}>
                New
              </option>
            )}

            {currentStage === 1 && (
              <>
                <option value={1}>New</option>
                <option value={2}>Qualified</option>
                <option value={6}>Lost</option>
              </>
            )}

            {currentStage === 2 && (
              <>
                <option value={2}>Qualified</option>
                <option value={3}>Proposal Sent</option>
                <option value={6}>Lost</option>
              </>
            )}

            {currentStage === 3 && (
              <>
                <option value={3}>Proposal Sent</option>
                <option value={4}>Negotiation</option>
                <option value={6}>Lost</option>
              </>
            )}

            {currentStage === 4 && (
              <>
                <option value={4}>Negotiation</option>
                <option value={5}>Won</option>
                <option value={6}>Lost</option>
              </>
            )}

            {currentStage === 5 && (
              <option value={5}>
                Won
              </option>
            )}

            {currentStage === 6 && (
              <option value={6}>
                Lost
              </option>
            )}

          </Select>
        </FormField>

      </div>

    </FormSection>
  );
}

export default DealInformationSection;