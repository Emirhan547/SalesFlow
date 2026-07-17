import {
  useMemo,
} from "react";

import type {
  FieldErrors,
  UseFormRegister,
} from "react-hook-form";

import {
  useUsers,
} from "@/features/user/hooks/useUsers";

import {
  useCustomers,
} from "@/features/customer/hooks/useCustomers";

import type {
  DealFormData,
} from "../../schemas/dealSchema";

type Props = {
  register: UseFormRegister<DealFormData>;
  errors: FieldErrors<DealFormData>;
};

function CustomerSection({
  register,
  errors,
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
    <div className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm">

      <div className="mb-6">

        <h2 className="text-lg font-semibold text-slate-900">
          Customer & Assignment
        </h2>

        <p className="mt-1 text-sm text-slate-500">
          Select the customer and assign the deal to a user.
        </p>

      </div>

      <div className="grid gap-6 md:grid-cols-2">

        <div>

          <label className="mb-2 block text-sm font-medium text-slate-700">
            Customer
          </label>

          <select
            {...register(
              "customerId",
              {
                valueAsNumber: true,
              }
            )}
            disabled={customersLoading}
            className="h-11 w-full rounded-xl border border-slate-200 bg-white px-4 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 disabled:bg-slate-100"
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

        <div>

          <label className="mb-2 block text-sm font-medium text-slate-700">
            Assigned User
          </label>

          <select
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
            className="h-11 w-full rounded-xl border border-slate-200 bg-white px-4 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 disabled:bg-slate-100"
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

          </select>

          {usersError && (
            <p className="mt-2 text-sm text-red-500">
              {usersError}
            </p>
          )}

          {errors.assignedUserId && (
            <p className="mt-2 text-sm text-red-500">
              {errors.assignedUserId.message}
            </p>
          )}

        </div>

      </div>

    </div>
  );
}

export default CustomerSection;