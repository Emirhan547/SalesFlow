import type {
  FieldErrors,
  UseFormRegister,
} from "react-hook-form";

import FormField from "@/components/common/FormField";
import FormSection from "@/components/common/FormSection";

import { Input } from "@/components/ui/input";
import { Select } from "@/components/ui/select";

import type { CustomerFormData } from "../../schemas/customerSchema";
import { CustomerType } from "../../types/CustomerType";

type Props = {
  register: UseFormRegister<CustomerFormData>;
  errors: FieldErrors<CustomerFormData>;
};

function CompanySection({
  register,
  errors,
}: Props) {
  return (
    <FormSection title="Company Information">

      <div className="grid gap-6 md:grid-cols-2">

        <FormField
          label="Customer Type"
          error={errors.customerType?.message}
        >
          <Select
            {...register("customerType", {
              valueAsNumber: true,
            })}
          >
            <option value={CustomerType.Individual}>
              Individual
            </option>

            <option value={CustomerType.Corporate}>
              Company
            </option>

          </Select>
        </FormField>

        <FormField
          label="Company Name"
          error={errors.companyName?.message}
        >
          <Input
            placeholder="Microsoft"
            {...register("companyName")}
          />
        </FormField>

        <FormField
          label="Website"
          error={errors.website?.message}
        >
          <Input
            placeholder="https://company.com"
            {...register("website")}
          />
        </FormField>

        <FormField
          label="Tax Number"
          error={errors.taxNumber?.message}
        >
          <Input
            {...register("taxNumber")}
          />
        </FormField>

      </div>

    </FormSection>
  );
}

export default CompanySection;