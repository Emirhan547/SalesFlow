import { useNavigate } from "react-router-dom";

import { toast } from "sonner";

import PageHeader from "@/components/common/PageHeader";

import LeadForm from "../components/LeadForm";

import { createLead } from "../services/leadService";

import type { LeadFormData } from "../schemas/leadSchema";

function LeadCreatePage() {

  const navigate =
    useNavigate();

  async function handleCreate(
    data: LeadFormData
  ) {

    const response =
  await createLead(data);

toast.success(response.message);

navigate("/leads");

  }

  return (

    <div className="space-y-8">

      <PageHeader
        title="Create Lead"
        description="Add a new lead into CRM."
      />

      <div className="rounded-3xl border border-slate-200 bg-white p-8 shadow-sm">
        <LeadForm
          submitText="Create Lead"
          onSubmit={handleCreate}
        />
      </div>

    </div>

  );

}

export default LeadCreatePage;