import type { UseFormRegister } from "react-hook-form";

import FormField from "@/components/common/FormField";
import FormSection from "@/components/common/FormSection";

import type { DealFormData } from "../../schemas/dealSchema";

type Props = {
  register: UseFormRegister<DealFormData>;
};

function DescriptionSection({
  register,
}: Props) {
  return (
    <FormSection title="Description">

      <FormField label="Description">

        <textarea
          rows={5}
          {...register("description")}
          className="w-full rounded-xl border border-slate-200 p-3 outline-none focus:border-blue-500"
        />

      </FormField>

    </FormSection>
  );
}

export default DescriptionSection;