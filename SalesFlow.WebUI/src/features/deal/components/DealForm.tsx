import { useState } from "react";

import { toast } from "sonner";

import {
  useForm,
  type DefaultValues,
} from "react-hook-form";

import { zodResolver } from "@hookform/resolvers/zod";

import DealInformationSection from "./form/DealInformationSection";
import CustomerSection from "./form/CustomerSection";
import DescriptionSection from "./form/DescriptionSection";
import FormButtons from "./form/FormButtons";

import {
  dealSchema,
  type DealFormData,
} from "../schemas/dealSchema";

type Props = {
  submitText: string;

  defaultValues?: DefaultValues<DealFormData>;

  onSubmit: (
    data: DealFormData
  ) => Promise<void>;
};

function DealForm({
  submitText,
  defaultValues,
  onSubmit,
}: Props) {

  const [loading, setLoading] =
    useState(false);

  const {
    register,
    handleSubmit,
    formState: {
      errors,
    },
  } = useForm<DealFormData>({
    resolver: zodResolver(dealSchema),

    defaultValues:
      defaultValues ?? {

        title: "",

        description: "",

        amount: 0,

        expectedCloseDate: "",

        customerId: 0,

        assignedUserId: null,

      },
  });

  async function submit(
    data: DealFormData
  ) {

    try {

      setLoading(true);

      await onSubmit(data);

    }
    catch {

      toast.error(
        "Unexpected error."
      );

    }
    finally {

      setLoading(false);

    }

  }

  return (
    <form
      onSubmit={handleSubmit(submit)}
      className="space-y-8"
    >

      <DealInformationSection
        register={register}
        errors={errors}
      />

      <CustomerSection
        register={register}
        errors={errors}
      />

      <DescriptionSection
        register={register}
      />

      <FormButtons
        loading={loading}
        submitText={submitText}
      />

    </form>
  );
}

export default DealForm;