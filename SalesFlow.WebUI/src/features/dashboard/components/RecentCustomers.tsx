import {
  ArrowRight,
  Building2,
  User,
} from "lucide-react";

import Card from "@/components/ui/Card";

import type { RecentCustomerDto } from "../types/DashboardDto";

type Props = {
  customers: RecentCustomerDto[];
};

function RecentCustomers({ customers }: Props) {
  return (
    <Card
      title="Recent Customers"
      subtitle="Latest registered customers"
    >
      {customers.length === 0 ? (

        <div className="flex h-60 items-center justify-center text-slate-400">
          No customers found.
        </div>

      ) : (

        <div className="space-y-2">

          {customers.map((customer) => (

            <div
              key={customer.id}
              className="flex items-center justify-between rounded-2xl p-4 transition hover:bg-slate-50"
            >

              <div className="flex items-center gap-4">

                <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-blue-100 text-blue-600">

                  <User size={20} />

                </div>

                <div>

                  <h4 className="font-semibold text-slate-900">

                    {customer.fullName}

                  </h4>

                  <div className="mt-1 flex items-center gap-2 text-sm text-slate-500">

                    <Building2 size={14} />

                    {customer.companyName}

                  </div>

                </div>

              </div>

              <button className="flex items-center gap-1 text-sm font-semibold text-blue-600 opacity-0 transition group-hover:opacity-100">

                View

                <ArrowRight size={15} />

              </button>

            </div>

          ))}

        </div>

      )}
    </Card>
  );
}

export default RecentCustomers;