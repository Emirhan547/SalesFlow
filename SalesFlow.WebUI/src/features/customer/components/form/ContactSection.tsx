import type {
  FieldErrors,
  UseFormRegister,
} from "react-hook-form";

import FormField from "@/components/common/FormField";
import FormSection from "@/components/common/FormSection";

import { Input } from "@/components/ui/input";

import type { CustomerFormData } from "../../schemas/customerSchema";

type Props = {
  register: UseFormRegister<CustomerFormData>;
  errors: FieldErrors<CustomerFormData>;
};

function ContactSection({
  register,
  errors,
}: Props) {
  return (
    <FormSection title="Contact Information">

      <div className="grid gap-6 md:grid-cols-2">

        <FormField
          label="First Name"
          error={errors.contactFirstName?.message}
        >
          <Input
            {...register("contactFirstName")}
          />
        </FormField>

        <FormField
          label="Last Name"
          error={errors.contactLastName?.message}
        >
          <Input
            {...register("contactLastName")}
          />
        </FormField>

        <FormField
          label="Email"
          error={errors.email?.message}
        >
          <Input
            type="email"
            {...register("email")}
          />
        </FormField>

        <FormField
          label="Phone Number"
          error={errors.phoneNumber?.message}
        >
          <Input
            {...register("phoneNumber")}
          />
        </FormField>

      </div>

    </FormSection>
  );
}

export default ContactSection;