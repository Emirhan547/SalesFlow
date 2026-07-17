import {
  useNavigate,
  useParams,
} from "react-router-dom";

import { toast } from "sonner";

import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import DealForm from "../components/DealForm";

import { useDeal } from "../hooks/useDeal";

import { updateDeal } from "../services/dealService";

import type { DealFormData } from "../schemas/dealSchema";
import type { DealStage } from "../types/DealStage";

function DealUpdatePage() {

  const { id } =
    useParams();

  const navigate =
    useNavigate();

  const {
    deal,
    loading,
    error,
  } = useDeal(Number(id));

  async function handleUpdate(
  data: DealFormData
) {

  if (!deal)
    return;

  const response =
    await updateDeal({

      id: deal.id,

      ...data,

      stage:
        data.stage as DealStage,

    });

  if (!response.isSuccess) {

    toast.error(response.message);

    return;

  }

  toast.success(response.message);

  navigate("/deals");

}

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
        title="Update Deal"
        description="Update deal information."
      />

     <DealForm
  submitText="Update Deal"
  currentStage={deal.stage}
  defaultValues={{
    title: deal.title,
    description: deal.description ?? "",
    amount: deal.amount,
    expectedCloseDate: deal.expectedCloseDate ?? "",
    stage: deal.stage,
    customerId: deal.customerId,
    assignedUserId: deal.assignedUserId ?? null,
  }}
  onSubmit={handleUpdate}
/>

    </div>
  );
}

export default DealUpdatePage;