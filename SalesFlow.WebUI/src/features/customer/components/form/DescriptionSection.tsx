import type { UseFormRegister } from "react-hook-form";

import FormField from "@/components/common/FormField";
import FormSection from "@/components/common/FormSection";

import { TextArea } from "@/components/ui/textarea";

import type { CustomerFormData } from "../../schemas/customerSchema";

type Props = {
  register: UseFormRegister<CustomerFormData>;
};

function DescriptionSection({
  register,
}: Props) {
  return (
    <FormSection title="Description">

      <FormField label="Description">

        <TextArea
          rows={5}
          {...register("description")}
        />

      </FormField>

    </FormSection>
  );
}

export default DescriptionSection;