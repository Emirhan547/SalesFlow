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
  page: number;
  pageSize: number;
  onDeleted?: () => void;
};

function DealTable({
  deals,
  page,
  pageSize,
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

        <div className="overflow-x-auto -mx-6">

          <table className="w-full">

            <thead>

              <tr className="border-b border-slate-200 text-left text-xs font-semibold uppercase tracking-wide text-slate-600 bg-slate-50">

                <th className="px-6 py-4">
                  Deal
                </th>

                <th className="px-6 py-4">
                  Amount
                </th>

                <th className="px-6 py-4">
                  Stage
                </th>

                <th className="px-6 py-4">
                  Close Date
                </th>

                <th className="px-6 py-4 text-right">
                  Actions
                </th>

              </tr>

            </thead>

            <tbody>

              {deals.map((deal, index) => (

                <tr
                  key={deal.id}
                  className="border-b border-slate-100 transition-all duration-200 hover:bg-blue-50/50"
                >

                  <td className="px-6 py-5">

                    <div className="flex items-center gap-3">

                      <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-indigo-100 text-indigo-600">

                        <Briefcase size={18} />

                      </div>

                      <div>

                        <h3 className="text-sm font-semibold text-slate-900">

                          {deal.title}

                        </h3>

                        <p className="text-xs text-slate-500 mt-0.5">

                          Deal #{(page - 1) * pageSize + index + 1}

                        </p>

                      </div>

                    </div>

                  </td>

                  <td className="px-6 py-5">

                    <div className="flex items-center gap-2 text-sm text-slate-900 font-semibold">

                      <DollarSign size={16} className="text-slate-400" />

                      {deal.amount.toLocaleString()}

                    </div>

                  </td>

                  <td className="px-6 py-5">

                    <span
                      className={`rounded-md px-2.5 py-1 text-xs font-semibold ${getStageColor(deal.stage)}`}
                    >

                      {getStageText(deal.stage)}

                    </span>

                  </td>

                  <td className="px-6 py-5">

                    <div className="flex items-center gap-2 text-sm text-slate-600">

                      <Calendar size={16} className="text-slate-400" />

                      {deal.expectedCloseDate
                        ? new Date(
                            deal.expectedCloseDate
                          ).toLocaleDateString()
                        : "-"}

                    </div>

                  </td>

                  <td className="px-6 py-5">

                    <div className="flex justify-end gap-1">

                      <button
                        onClick={() =>
                          navigate(`/deals/${deal.id}`)
                        }
                        className="rounded-lg p-2 text-slate-500 transition-all hover:bg-blue-100 hover:text-blue-600"
                        title="View"
                      >
                        <Eye size={16} />
                      </button>

                      <button
                        onClick={() =>
                          navigate(`/deals/edit/${deal.id}`)
                        }
                        className="rounded-lg p-2 text-slate-500 transition-all hover:bg-amber-100 hover:text-amber-600"
                        title="Edit"
                      >
                        <Pencil size={16} />
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