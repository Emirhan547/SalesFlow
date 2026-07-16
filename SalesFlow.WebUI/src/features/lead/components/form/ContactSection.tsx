import type {
  FieldErrors,
  UseFormRegister,
} from "react-hook-form";

import FormField from "@/components/common/FormField";
import FormSection from "@/components/common/FormSection";

import { Input } from "@/components/ui/input";

import type { LeadFormData } from "../../schemas/leadSchema";

type Props = {
  register: UseFormRegister<LeadFormData>;
  errors: FieldErrors<LeadFormData>;
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
          error={errors.firstName?.message}
        >
          <Input
            {...register("firstName")}
          />
        </FormField>

        <FormField
          label="Last Name"
          error={errors.lastName?.message}
        >
          <Input
            {...register("lastName")}
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
          label="Phone"
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