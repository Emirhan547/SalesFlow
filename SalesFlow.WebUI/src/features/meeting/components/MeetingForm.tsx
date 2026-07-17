import {
  useEffect,
  useState,
} from "react";

import axios from "axios";

import { toast } from "sonner";

import {
  useForm,
  type DefaultValues,
} from "react-hook-form";

import {
  zodResolver,
} from "@hookform/resolvers/zod";

import GeneralSection from "./form/GeneralSection";
import ScheduleSection from "./form/ScheduleSection";
import DescriptionSection from "./form/DescriptionSection";
import FormButtons from "./form/FormButtons";

import {
  meetingSchema,
  type MeetingFormData,
} from "../schemas/meetingSchema";

import {
  checkMeetingAvailability,
} from "../services/meetingService";

type Props = {
  submitText: string;

  defaultValues?: DefaultValues<MeetingFormData>;

  meetingId?: number;

  showStatus?: boolean;

  onSubmit: (
    data: MeetingFormData
  ) => Promise<void>;
};

function MeetingForm({
  submitText,
  defaultValues,
  meetingId,
  showStatus = false,
  onSubmit,
}: Props) {

  const [loading, setLoading] =
    useState(false);

  const [
    checkingAvailability,
    setCheckingAvailability,
  ] = useState(false);

  const [
    isAvailable,
    setIsAvailable,
  ] = useState<boolean | null>(
    null
  );

  const {
    register,
    handleSubmit,
    reset,
    watch,
    formState: {
      errors,
    },
  } = useForm<MeetingFormData>({
    resolver:
      zodResolver(
        meetingSchema
      ),

    defaultValues:
      defaultValues ?? {

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

  const assignedUserId =
    watch("assignedUserId");

  const startDate =
    watch("startDate");

  const endDate =
    watch("endDate");

  useEffect(() => {

    if (defaultValues) {

      reset(defaultValues);

    }

  }, [
    defaultValues,
    reset,
  ]);

  useEffect(() => {

    if (
      !assignedUserId ||
      !startDate ||
      !endDate
    ) {

      setIsAvailable(null);

      return;
    }

    if (
      new Date(endDate) <=
      new Date(startDate)
    ) {

      setIsAvailable(null);

      return;
    }

    const timeout =
      setTimeout(
        async () => {

          try {

            setCheckingAvailability(
              true
            );

            const response =
              await checkMeetingAvailability(
                assignedUserId,
                startDate,
                endDate,
                meetingId
              );

            setIsAvailable(
              response.data
            );

          }
          catch {

            setIsAvailable(null);

          }
          finally {

            setCheckingAvailability(
              false
            );

          }

        },
        500
      );

    return () =>
      clearTimeout(timeout);

  }, [
    assignedUserId,
    startDate,
    endDate,
    meetingId,
  ]);

  async function submit(
    data: MeetingFormData
  ) {

    if (
      isAvailable === false
    ) {

      toast.error(
        "The assigned user already has another meeting during this time."
      );

      return;
    }

    try {

      setLoading(true);

      await onSubmit(data);

    }
    catch (error) {

      if (
        axios.isAxiosError(error)
      ) {

        toast.error(
          error.response?.data
            ?.Message ??
          error.response?.data
            ?.message ??
          "Meeting could not be saved."
        );

        return;

      }

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
      onSubmit={
        handleSubmit(submit)
      }
      className="space-y-8"
    >

     <GeneralSection
  register={register}
  errors={errors}
  showStatus={showStatus}
/>

      <ScheduleSection
        register={register}
        errors={errors}
      />

      {checkingAvailability && (

        <div className="rounded-xl border border-slate-200 bg-slate-50 px-4 py-3 text-sm text-slate-600">

          Checking user availability...

        </div>

      )}

      {!checkingAvailability &&
        isAvailable === true && (

        <div className="rounded-xl border border-green-200 bg-green-50 px-4 py-3 text-sm text-green-700">

          The assigned user is available during this time.

        </div>

      )}

      {!checkingAvailability &&
        isAvailable === false && (

        <div className="rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">

          The assigned user already has another meeting during this time.

        </div>

      )}

      <DescriptionSection
        register={register}
      />

      <FormButtons
        loading={
          loading ||
          checkingAvailability
        }
        submitText={
          submitText
        }
      />

    </form>

  );
}

export default MeetingForm;