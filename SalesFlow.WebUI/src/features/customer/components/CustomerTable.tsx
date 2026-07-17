import { useState } from "react";
import { useNavigate } from "react-router-dom";

import {
  Building2,
  Eye,
  Mail,
  Pencil,
  Phone,
  Trash2,
  User,
} from "lucide-react";

import Card from "@/components/ui/Card";
import ConfirmDialog from "@/components/ui/ConfirmDialog";

import { useDeleteCustomer } from "../hooks/useDeleteCustomer";

import type { Customer } from "../types/Customer";
import { CustomerTypes } from "../types/CustomerType";

type Props = {
  customers: Customer[];
  onDeleted?: () => void;
};

function CustomerTable({
  customers,
  onDeleted,
}: Props) {

  const navigate =
    useNavigate();

  const [selectedId, setSelectedId] =
    useState<number>();

  const [dialogOpen, setDialogOpen] =
    useState(false);

  const {
    loading,
    remove,
  } = useDeleteCustomer();

  if (customers.length === 0) {

    return (
      <Card
        title="Customers"
      >

        <div className="flex h-72 items-center justify-center text-slate-400">

          No customers found.

        </div>

      </Card>
    );

  }

  return (
    <>

      <Card
        title="Customers"
        subtitle={`${customers.length} customer(s)`}
      >

        <div className="overflow-x-auto">

          <table className="w-full">

            <thead>

              <tr className="border-b border-slate-200 text-left text-sm font-semibold uppercase tracking-wide text-slate-500">

                <th className="pb-5">
                  Customer
                </th>

                <th className="pb-5">
                  Contact
                </th>

                <th className="pb-5">
                  Email
                </th>

                <th className="pb-5">
                  Phone
                </th>

                <th className="pb-5 text-right">
                  Actions
                </th>

              </tr>

            </thead>

            <tbody>

              {customers.map((customer) => (

                <tr
                  key={customer.id}
                  className="border-b border-slate-100 transition duration-200 hover:bg-slate-50"
                >

                  <td className="py-5">

                    <div className="flex items-center gap-4">

                      <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-blue-100 text-blue-600">

                        <Building2 size={20} />

                      </div>

                      <div>

                        <h3 className="font-semibold text-slate-900">

                          {customer.companyName || "Individual Customer"}

                        </h3>

                        <div className="mt-2 flex items-center gap-2">

                          <span className="rounded-full bg-slate-100 px-2 py-1 text-xs font-medium text-slate-600">

                            #{customer.id}

                          </span>

                          <span
                            className={`rounded-full px-2 py-1 text-xs font-medium ${
                            customer.customerType === CustomerTypes.Individual
                                ? "bg-blue-100 text-blue-700"
                                : "bg-violet-100 text-violet-700"
                            }`}
                          >

                            {customer.customerType === CustomerTypes.Individual
  ? "Individual"
  : "Company"}

                          </span>

                        </div>

                      </div>

                    </div>

                  </td>

                  <td>

                    <div className="flex items-center gap-2 text-slate-700">

                      <User size={16} />

                      {customer.contactFirstName}{" "}
                      {customer.contactLastName}

                    </div>

                  </td>

                  <td>

                    <div className="flex items-center gap-2 text-slate-700">

                      <Mail size={16} />

                      {customer.email}

                    </div>

                  </td>

                  <td>

                    <div className="flex items-center gap-2 text-slate-700">

                      <Phone size={16} />

                      {customer.phoneNumber}

                    </div>

                  </td>

                  <td>

                    <div className="flex justify-end gap-2">

                      <button
                        onClick={() =>
                          navigate(`/customers/${customer.id}`)
                        }
                        className="rounded-xl border border-transparent p-2.5 text-slate-500 transition hover:border-blue-200 hover:bg-blue-50 hover:text-blue-600"
                      >
                        <Eye size={18} />
                      </button>

                      <button
                        onClick={() =>
                          navigate(`/customers/edit/${customer.id}`)
                        }
                        className="rounded-xl border border-transparent p-2.5 text-slate-500 transition hover:border-amber-200 hover:bg-amber-50 hover:text-amber-600"
                      >
                        <Pencil size={18} />
                      </button>

                      <button
                        onClick={() => {

                          setSelectedId(customer.id);

                          setDialogOpen(true);

                        }}
                        className="rounded-xl border border-transparent p-2.5 text-slate-500 transition hover:border-red-200 hover:bg-red-50 hover:text-red-600"
                      >
                        <Trash2 size={18} />
                      </button>

                    </div>

                  </td>

                </tr>

              ))}

            </tbody>

          </table>

        </div>

      </Card>

      <ConfirmDialog
        open={dialogOpen}
        title="Delete Customer"
        description="Are you sure you want to delete this customer?"
        loading={loading}
        onCancel={() => {

          setDialogOpen(false);

          setSelectedId(undefined);

        }}
        onConfirm={async () => {

          if (!selectedId)
            return;

          const success =
            await remove(selectedId);

          if (!success)
            return;

          setDialogOpen(false);

          setSelectedId(undefined);

          onDeleted?.();

        }}
      />

    </>
  );
}

export default CustomerTable;