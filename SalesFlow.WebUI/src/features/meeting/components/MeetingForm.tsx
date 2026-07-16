import { useEffect, useState } from "react";

import {
  useForm,
  type DefaultValues,
} from "react-hook-form";

import { zodResolver } from "@hookform/resolvers/zod";

import GeneralSection from "./form/GeneralSection";
import ScheduleSection from "./form/ScheduleSection";
import DescriptionSection from "./form/DescriptionSection";

import {
  meetingSchema,
  type MeetingFormData,
} from "../schemas/meetingSchema";
import FormButtons from "./form/FormButtons";

type Props = {
  submitText: string;

  defaultValues?: DefaultValues<MeetingFormData>;

  onSubmit: (
    data: MeetingFormData
  ) => Promise<void>;
};

function MeetingForm({
  submitText,
  defaultValues,
  onSubmit,
}: Props) {

  const [loading, setLoading] =
    useState(false);

  const {
    register,
    handleSubmit,
    reset,
    formState: {
      errors,
    },
  } = useForm<MeetingFormData>({
    resolver: zodResolver(
      meetingSchema
    ),

    defaultValues: defaultValues ?? {

      title: "",

      description: "",

      startDate: "",

      endDate: "",

      type: 1,

      status: 1,

      location: "",

      customerId: 0,

      assignedUserId: null,

    },
  });

  useEffect(() => {

    if (defaultValues) {

      reset(defaultValues);

    }

  }, [
    defaultValues,
    reset,
  ]);

  async function submit(
    data: MeetingFormData
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

      <GeneralSection
        register={register}
        errors={errors}
      />

      <ScheduleSection
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

export default MeetingForm;