import { useState } from "react";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import CompanySection from "./form/CompanySection";
import ContactSection from "./form/ContactSection";
import AddressSection from "./form/AddressSection";
import DescriptionSection from "./form/DescriptionSection";
import FormButtons from "./form/FormButtons";

import {
  customerSchema,
  type CustomerFormData,
} from "../schemas/customerSchema";
import { CustomerTypes } from "../types/CustomerType";
import { toast } from "sonner";

type Props = {
  submitText?: string;
  defaultValues?: CustomerFormData;
  onSubmit: (
    data: CustomerFormData
  ) => Promise<void>;
};

function CustomerForm({
  submitText = "Save Customer",
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
    reset,
  } = useForm<CustomerFormData>({
    resolver: zodResolver(customerSchema),

    defaultValues:
      defaultValues ?? {
       customerType: CustomerTypes.Individual,
        companyName: "",
        contactFirstName: "",
        contactLastName: "",
        email: "",
        phoneNumber: "",
        website: "",
        taxNumber: "",
        address: "",
        description: "",
        assignedUserId: null,
      },
  });

  async function handleFormSubmit(
    data: CustomerFormData
  ) {
    try {
      setLoading(true);

      await onSubmit(data);

      reset();
    }
    catch (error) {

  if (error instanceof Error) {

    toast.error(error.message);

  }
  else {

    toast.error(
      "An unexpected error occurred."
    );

  }

}
finally {

  setLoading(false);

}
    
  }

  return (
    <form
      onSubmit={handleSubmit(handleFormSubmit)}
      className="space-y-8"
    >

      <CompanySection
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

export default CustomerForm;