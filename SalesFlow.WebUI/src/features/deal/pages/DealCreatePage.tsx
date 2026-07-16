import { useNavigate } from "react-router-dom";

import { toast } from "sonner";

import PageHeader from "@/components/common/PageHeader";


import { createDeal } from "../services/dealService";

import type { DealFormData } from "../schemas/dealSchema";
import DealForm from "../components/DealForm";

function DealCreatePage() {

  const navigate =
    useNavigate();

  async function handleCreate(
    data: DealFormData
  ) {

    const response =
      await createDeal(data);

    if (!response.isSuccess) {

      toast.error(response.message);

      return;

    }

    toast.success(response.message);

    navigate("/deals");

  }

  return (
    <div className="space-y-8">

      <PageHeader
        title="Create Deal"
        description="Create a new deal."
      />

      <DealForm
        submitText="Create Deal"
        onSubmit={handleCreate}
      />

    </div>
  );
}

export default DealCreatePage;