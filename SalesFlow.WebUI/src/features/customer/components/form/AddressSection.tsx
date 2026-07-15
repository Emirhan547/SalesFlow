import type { UseFormRegister } from "react-hook-form";

import FormField from "@/components/common/FormField";
import FormSection from "@/components/common/FormSection";

import { TextArea } from "@/components/ui/textarea";

import type { CustomerFormData } from "../../schemas/customerSchema";

type Props = {
  register: UseFormRegister<CustomerFormData>;
};

function AddressSection({
  register,
}: Props) {
  return (
    <FormSection title="Address">

      <FormField label="Address">

        <TextArea
          rows={4}
          {...register("address")}
        />

      </FormField>

    </FormSection>
  );
}

export default AddressSection;