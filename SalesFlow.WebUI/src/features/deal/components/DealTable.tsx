import { useState } from "react";
import { useNavigate } from "react-router-dom";

import {
  DollarSign,
  Eye,
  Pencil,
  Trash2,
  Calendar,
  Briefcase,
} from "lucide-react";

import Card from "@/components/ui/Card";
import ConfirmDialog from "@/components/ui/ConfirmDialog";

import { useDeleteDeal } from "../hooks/useDeleteDeal";

import type { Deal } from "../types/Deal";

type Props = {
  deals: Deal[];
  onDeleted?: () => void;
};

function DealTable({
  deals,
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
  } = useDeleteDeal();

  if (deals.length === 0) {
    return (
      <Card title="Deals">

        <div className="flex h-72 items-center justify-center text-slate-400">

          No deals found.

        </div>

      </Card>
    );
  }

  function getStageColor(
    stage: number
  ) {

    switch (stage) {

      case 1:
        return "bg-blue-100 text-blue-700";

      case 2:
        return "bg-yellow-100 text-yellow-700";

      case 3:
        return "bg-indigo-100 text-indigo-700";

      case 4:
        return "bg-orange-100 text-orange-700";

      case 5:
        return "bg-green-100 text-green-700";

      case 6:
        return "bg-red-100 text-red-700";

      default:
        return "bg-slate-100 text-slate-700";

    }

  }

  function getStageText(
    stage: number
  ) {

    switch (stage) {

      case 1:
        return "New";

      case 2:
        return "Qualified";

      case 3:
        return "Proposal";

      case 4:
        return "Negotiation";

      case 5:
        return "Won";

      case 6:
        return "Lost";

      default:
        return "-";

    }

  }

  return (
    <>

      <Card
        title="Deals"
        subtitle={`${deals.length} deal(s)`}
      >

        <div className="overflow-x-auto">

          <table className="w-full">

            <thead>

              <tr className="border-b border-slate-200 text-left text-sm font-semibold text-slate-500">

                <th className="pb-4">
                  Deal
                </th>

                <th className="pb-4">
                  Amount
                </th>

                <th className="pb-4">
                  Stage
                </th>

                <th className="pb-4">
                  Close Date
                </th>

                <th className="pb-4 text-right">
                  Actions
                </th>

              </tr>

            </thead>

            <tbody>

              {deals.map((deal) => (

                <tr
                  key={deal.id}
                  className="border-b border-slate-100 transition hover:bg-slate-50"
                >

                  <td className="py-5">

                    <div className="flex items-center gap-4">

                      <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-indigo-100 text-indigo-600">

                        <Briefcase size={20} />

                      </div>

                      <div>

                        <h3 className="font-semibold text-slate-900">

                          {deal.title}

                        </h3>

                        <p className="text-sm text-slate-500">

                          Deal #{deal.id}

                        </p>

                      </div>

                    </div>

                  </td>

                  <td>

                    <div className="flex items-center gap-2">

                      <DollarSign size={16} />

                      {deal.amount.toLocaleString()} $

                    </div>

                  </td>

                  <td>

                    <span
                      className={`rounded-full px-3 py-1 text-xs font-semibold ${getStageColor(deal.stage)}`}
                    >

                      {getStageText(deal.stage)}

                    </span>

                  </td>

                  <td>

                    <div className="flex items-center gap-2">

                      <Calendar size={16} />

                      {deal.expectedCloseDate
                        ? new Date(
                            deal.expectedCloseDate
                          ).toLocaleDateString()
                        : "-"}

                    </div>

                  </td>

                  <td>

                    <div className="flex justify-end gap-2">

                      <button
                        onClick={() =>
                          navigate(`/deals/${deal.id}`)
                        }
                        className="rounded-xl p-2 text-slate-500 transition hover:bg-blue-100 hover:text-blue-600"
                      >
                        <Eye size={18} />
                      </button>

                      <button
                        onClick={() =>
                          navigate(`/deals/edit/${deal.id}`)
                        }
                        className="rounded-xl p-2 text-slate-500 transition hover:bg-amber-100 hover:text-amber-600"
                      >
                        <Pencil size={18} />
                      </button>

                      <button
                        onClick={() => {

                          setSelectedId(
                            deal.id
                          );

                          setDialogOpen(
                            true
                          );

                        }}
                        className="rounded-xl p-2 text-slate-500 transition hover:bg-red-100 hover:text-red-600"
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
        title="Delete Deal"
        description="Are you sure you want to delete this deal?"
        loading={loading}
        onCancel={() => {

          setDialogOpen(false);

          setSelectedId(undefined);

        }}
        onConfirm={async () => {

          if (!selectedId)
            return;

          const success =
            await remove(
              selectedId
            );

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

export default DealTable;