import {
  useEffect,
  useMemo,
} from "react";

import {
  useForm,
} from "react-hook-form";

import {
  zodResolver,
} from "@hookform/resolvers/zod";

import { toast } from "sonner";

import {
  Button,
} from "@/components/ui/button";

import {
  Label,
} from "@/components/ui/label";

import {
  useCustomers,
} from "@/features/customer/hooks/useCustomers";

import {
  noteSchema,
  type NoteFormData,
} from "../schemas/noteSchema";

type Props = {
  submitText: string;

  defaultValues?: NoteFormData;

  onSubmit: (
    data: NoteFormData
  ) => Promise<void>;
};

function NoteForm({
  submitText,
  defaultValues,
  onSubmit,
}: Props) {

  const customerRequest =
    useMemo(() => ({
      page: 1,
      pageSize: 100,
    }), []);

  const {
    data: customerData,
    loading: customersLoading,
    error: customersError,
  } = useCustomers(
    customerRequest
  );

  const customers =
    customerData?.items ?? [];

  const {
    register,
    handleSubmit,
    reset,

    formState: {
      errors,
      isSubmitting,
    },

  } = useForm<NoteFormData>({
    resolver:
      zodResolver(noteSchema),

    defaultValues,
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
    data: NoteFormData
  ) {

    try {

      await onSubmit(data);

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

  }

  return (

    <form
      onSubmit={
        handleSubmit(submit)
      }
      className="space-y-6"
    >

      <div>

        <Label>
          Content
        </Label>

        <textarea
          rows={8}
          className="mt-2 w-full rounded-xl border border-slate-200 p-3 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20"
          {...register("content")}
        />

        {errors.content && (
          <p className="mt-1 text-sm text-red-500">
            {errors.content.message}
          </p>
        )}

      </div>

      <div className="grid gap-6 md:grid-cols-2">

        <div>

          <Label>
            Customer
          </Label>

          <select
            {...register(
              "customerId",
              {
                valueAsNumber: true,
              }
            )}
            disabled={
              customersLoading
            }
            className="mt-2 h-11 w-full rounded-xl border border-slate-200 bg-white px-4 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 disabled:bg-slate-100"
          >

            <option value="">
              {customersLoading
                ? "Loading customers..."
                : "Select customer"}
            </option>

            {customers.map(
              (customer) => (

                <option
                  key={customer.id}
                  value={customer.id}
                >
                  {customer.companyName
                    ? `${customer.companyName} - ${customer.contactFirstName} ${customer.contactLastName}`
                    : `${customer.contactFirstName} ${customer.contactLastName}`}
                </option>

              )
            )}

          </select>

          {customersError && (
            <p className="mt-2 text-sm text-red-500">
              {customersError}
            </p>
          )}

          {errors.customerId && (
            <p className="mt-2 text-sm text-red-500">
              {errors.customerId.message}
            </p>
          )}

        </div>

      </div>

      <Button
        disabled={
          isSubmitting ||
          customersLoading
        }
        type="submit"
      >
        {submitText}
      </Button>

    </form>

  );
}

export default NoteForm;