import { useState } from "react";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";



import {
  leadSchema,
  type LeadFormData,
} from "../schemas/leadSchema";
import LeadInformationSection from "./form/LeadInformationSection";
import ContactSection from "./form/ContactSection";
import AddressSection from "./form/AddressSection";
import DescriptionSection from "./form/DescriptionSection";
import FormButtons from "./form/FormButtons";

type Props = {
  submitText: string;
  defaultValues?: LeadFormData;
  onSubmit: (
    data: LeadFormData
  ) => Promise<void>;
};

function LeadForm({
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
  } = useForm<LeadFormData>({
    resolver: zodResolver(leadSchema),

    defaultValues: defaultValues ?? {

      firstName: "",

      lastName: "",

      companyName: "",

      email: "",

      phoneNumber: "",

      website: "",

      address: "",

      description: "",

      status: 1,

      source: 1,

      assignedUserId: null,

    },

  });

  async function submit(
    data: LeadFormData
  ) {

    try {

      setLoading(true);

      await onSubmit(data);

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

      <LeadInformationSection
        register={register}
        errors={errors}
      />

      <ContactSection
        register={register}
        errors={errors}
      />

      <AddressSection
        register={register}
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

export default LeadForm;