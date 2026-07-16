import { useParams } from "react-router-dom";

import Card from "@/components/ui/Card";

import DetailItem from "@/components/common/DetailItem";
import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import { useDeal } from "../hooks/useDeal";

function getStageText(
  stage: number
) {
  switch (stage) {

    case 1:
      return "New";

    case 2:
      return "Qualified";

    case 3:
      return "Proposal Sent";

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

function DealDetailPage() {

  const { id } =
    useParams();

  const {
    deal,
    loading,
    error,
  } = useDeal(Number(id));

  if (loading)
    return <LoadingState />;

  if (error)
    return (
      <div className="rounded-2xl border border-red-200 bg-red-50 p-6 text-red-600">
        {error}
      </div>
    );

  if (!deal)
    return (
      <div className="rounded-2xl bg-white p-8 text-center shadow">
        Deal not found.
      </div>
    );

  return (
    <div className="space-y-8">

      <PageHeader
        title="Deal Detail"
        description="Deal information overview."
      />

      <Card title="Deal Information">

        <div className="grid gap-6 md:grid-cols-2">

          <DetailItem
            label="Title"
            value={deal.title}
          />

          <DetailItem
            label="Amount"
            value={`$${deal.amount.toLocaleString()}`}
          />

          <DetailItem
            label="Stage"
            value={getStageText(deal.stage)}
          />

          <DetailItem
            label="Customer Id"
            value={deal.customerId}
          />

          <DetailItem
            label="Assigned User"
            value={deal.assignedUserId ?? "-"}
          />

          <DetailItem
            label="Expected Close Date"
            value={
              deal.expectedCloseDate
                ? new Date(
                    deal.expectedCloseDate
                  ).toLocaleDateString()
                : "-"
            }
          />

        </div>

      </Card>

      <Card title="Description">

        <p className="text-slate-700 whitespace-pre-wrap">

          {deal.description || "-"}

        </p>

      </Card>

    </div>
  );
}

export default DealDetailPage;