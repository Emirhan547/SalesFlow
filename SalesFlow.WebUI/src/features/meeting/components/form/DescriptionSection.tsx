import type { UseFormRegister } from "react-hook-form";

import FormSection from "@/components/common/FormSection";

import type { MeetingFormData } from "../../schemas/meetingSchema";

type Props = {
  register: UseFormRegister<MeetingFormData>;
};

function DescriptionSection({
  register,
}: Props) {
  return (
    <FormSection title="Description">

      <textarea
        rows={5}
        {...register("description")}
        className="w-full rounded-xl border border-slate-200 p-3 outline-none focus:border-blue-500"
      />

    </FormSection>
  );
}

export default DescriptionSection;