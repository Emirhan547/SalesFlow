import { BadgeDollarSign } from "lucide-react";

import Card from "@/components/ui/Card";

import type { RecentDealDto } from "../types/DashboardDto";

type Props = {
  deals: RecentDealDto[];
};

function RecentDeals({ deals }: Props) {
  return (
    <Card
      title="Recent Deals"
      subtitle="Latest created deals"
    >
      {deals.length === 0 ? (

        <div className="flex h-56 items-center justify-center text-slate-400">
          No deals found.
        </div>

      ) : (

        <div className="space-y-2">

          {deals.map((deal) => (

            <div
              key={deal.id}
              className="flex items-center justify-between rounded-2xl p-4 transition-all duration-200 hover:bg-slate-50"
            >

              <div>

                <h4 className="font-semibold text-slate-900">
                  {deal.title}
                </h4>

              </div>

              <div className="flex items-center gap-2 rounded-xl bg-green-50 px-3 py-2 font-semibold text-green-600">

                <BadgeDollarSign size={18} />

                ₺{deal.amount.toLocaleString()}

              </div>

            </div>

          ))}

        </div>

      )}
    </Card>
  );
}

export default RecentDeals;