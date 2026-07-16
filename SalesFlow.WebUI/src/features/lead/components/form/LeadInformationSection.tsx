import type {
  FieldErrors,
  UseFormRegister,
} from "react-hook-form";

import FormField from "@/components/common/FormField";
import FormSection from "@/components/common/FormSection";

import { Input } from "@/components/ui/input";
import { Select } from "@/components/ui/select";

import type { LeadFormData } from "../../schemas/leadSchema";

type Props = {
  register: UseFormRegister<LeadFormData>;
  errors: FieldErrors<LeadFormData>;
};

function LeadInformationSection({
  register,
  errors,
}: Props) {

  return (

    <FormSection title="Lead Information">

      <div className="grid gap-6 md:grid-cols-2">

        <FormField
          label="Status"
          error={errors.status?.message}
        >
          <Select
            {...register("status", {
              valueAsNumber: true,
            })}
          >
            <option value={1}>New</option>
            <option value={2}>Contacted</option>
            <option value={3}>Qualified</option>
            <option value={4}>Lost</option>
            <option value={5}>Converted</option>
          </Select>
        </FormField>

        <FormField
          label="Source"
          error={errors.source?.message}
        >
          <Select
            {...register("source", {
              valueAsNumber: true,
            })}
          >
            <option value={1}>Website</option>
            <option value={2}>Phone</option>
            <option value={3}>Email</option>
            <option value={4}>Referral</option>
            <option value={5}>Social Media</option>
            <option value={6}>Advertisement</option>
            <option value={7}>Other</option>
          </Select>
        </FormField>

        <FormField
          label="Company"
          error={errors.companyName?.message}
        >
          <Input
            {...register("companyName")}
          />
        </FormField>

        <FormField
          label="Website"
          error={errors.website?.message}
        >
          <Input
            {...register("website")}
          />
        </FormField>

      </div>

    </FormSection>

  );

}

export default LeadInformationSection;