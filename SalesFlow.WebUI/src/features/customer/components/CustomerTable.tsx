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

        <div className="overflow-x-auto -mx-6">

          <table className="w-full">

            <thead>

              <tr className="border-b border-slate-200 text-left text-xs font-semibold uppercase tracking-wide text-slate-600 bg-slate-50">

                <th className="px-6 py-4">
                  Customer
                </th>

                <th className="px-6 py-4">
                  Contact
                </th>

                <th className="px-6 py-4">
                  Email
                </th>

                <th className="px-6 py-4">
                  Phone
                </th>

                <th className="px-6 py-4 text-right">
                  Actions
                </th>

              </tr>

            </thead>

            <tbody>

              {customers.map((customer) => (

                <tr
                  key={customer.id}
                  className="border-b border-slate-100 transition-all duration-200 hover:bg-blue-50/50"
                >

                  <td className="px-6 py-5">

                    <div className="flex items-center gap-3">

                      <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-blue-100 text-blue-600">

                       <Building2 size={18} />

                      </div>

                      <div>

                       <h3 className="text-sm font-semibold text-slate-900">

                          {customer.companyName || "Individual Customer"}

                        </h3>

                       <div className="mt-1.5 flex items-center gap-2">

                         <span className="rounded-md bg-slate-100 px-2 py-0.5 text-xs font-medium text-slate-600">

                            #{customer.id}

                          </span>

                          <span
                           className={`rounded-md px-2 py-0.5 text-xs font-medium ${
                            customer.customerType === CustomerTypes.Individual
                                ? "bg-blue-100 text-blue-700"
                               : "bg-purple-100 text-purple-700"
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

                  <td className="px-6 py-5">

                    <div className="flex items-center gap-2 text-sm text-slate-700">

                      <User size={16} className="text-slate-400" />

                      {customer.contactFirstName}{" "}
                      {customer.contactLastName}

                    </div>

                  </td>

                  <td className="px-6 py-5">

                    <div className="flex items-center gap-2 text-sm text-slate-700">

                      <Mail size={16} className="text-slate-400" />

                      {customer.email}

                    </div>

                  </td>

                  <td className="px-6 py-5">

                    <div className="flex items-center gap-2 text-sm text-slate-700">

                      <Phone size={16} className="text-slate-400" />

                      {customer.phoneNumber}

                    </div>

                  </td>

                  <td className="px-6 py-5">

                    <div className="flex justify-end gap-1">

                      <button
                        onClick={() =>
                          navigate(`/customers/${customer.id}`)
                        }
                        className="rounded-lg p-2 text-slate-500 transition-all hover:bg-blue-100 hover:text-blue-600"
                        title="View"
                      >
                        <Eye size={16} />
                      </button>

                      <button
                        onClick={() =>
                          navigate(`/customers/edit/${customer.id}`)
                        }
                        className="rounded-lg p-2 text-slate-500 transition-all hover:bg-amber-100 hover:text-amber-600"
                        title="Edit"
                      >
                        <Pencil size={16} />
                      </button>

                      <button
                        onClick={() => {

                          setSelectedId(customer.id);

                          setDialogOpen(true);

                        }}
                        className="rounded-lg p-2 text-slate-500 transition-all hover:bg-red-100 hover:text-red-600"
                        title="Delete"
                      >
                        <Trash2 size={16} />
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