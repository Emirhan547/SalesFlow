import {
  useMemo,
} from "react";

import type {
  FieldErrors,
  UseFormRegister,
} from "react-hook-form";

import FormField from "@/components/common/FormField";
import FormSection from "@/components/common/FormSection";

import { Input } from "@/components/ui/input";
import { Select } from "@/components/ui/select";

import {
  useUsers,
} from "@/features/user/hooks/useUsers";

import {
  useCustomers,
} from "@/features/customer/hooks/useCustomers";

import type {
  MeetingFormData,
} from "../../schemas/meetingSchema";

type Props = {
  register: UseFormRegister<MeetingFormData>;
  errors: FieldErrors<MeetingFormData>;
  showStatus?: boolean;
};

function GeneralSection({
  register,
  errors,
  showStatus = false,
}: Props) {

  const {
    users,
    loading: usersLoading,
    error: usersError,
  } = useUsers();

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

  return (
    <FormSection title="General Information">

      <div className="grid gap-6 md:grid-cols-2">

        <FormField
          label="Title"
          error={errors.title?.message}
        >
          <Input
            {...register("title")}
          />
        </FormField>

        <FormField
          label="Meeting Type"
          error={errors.type?.message}
        >
          <Select
            {...register("type", {
              valueAsNumber: true,
            })}
          >
            <option value={1}>
              Online
            </option>

            <option value={2}>
              Office
            </option>

            <option value={3}>
              Phone
            </option>

            <option value={4}>
              Customer Visit
            </option>

            <option value={5}>
              Other
            </option>
          </Select>
        </FormField>

        <FormField
          label="Customer"
          error={
            errors.customerId?.message ??
            customersError
          }
        >
          <Select
            {...register(
              "customerId",
              {
                valueAsNumber: true,
              }
            )}
            disabled={customersLoading}
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

          </Select>
        </FormField>

        <FormField
          label="Assigned User"
          error={
            errors.assignedUserId?.message ??
            usersError
          }
        >
          <Select
            {...register(
              "assignedUserId",
              {
                setValueAs: (
                  value
                ) =>
                  value === ""
                    ? null
                    : Number(value),
              }
            )}
            disabled={usersLoading}
          >

            <option value="">
              {usersLoading
                ? "Loading users..."
                : "Select assigned user"}
            </option>

            {users.map(
              (user) => (

                <option
                  key={user.id}
                  value={user.id}
                >
                  {user.firstName}{" "}
                  {user.lastName}
                </option>

              )
            )}

          </Select>
        </FormField>

        {showStatus && (

          <FormField
            label="Status"
            error={errors.status?.message}
          >
            <Select
              {...register(
                "status",
                {
                  valueAsNumber: true,
                }
              )}
            >
              <option value={1}>
                Scheduled
              </option>

              <option value={2}>
                Completed
              </option>

              <option value={3}>
                Cancelled
              </option>
            </Select>
          </FormField>

        )}

      </div>

    </FormSection>
  );
}

export default GeneralSection;