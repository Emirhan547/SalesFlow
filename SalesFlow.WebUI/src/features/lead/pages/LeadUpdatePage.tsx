import { useNavigate, useParams } from "react-router-dom";

import { toast } from "sonner";

import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import LeadForm from "../components/LeadForm";

import { useLead } from "../hooks/useLead";

import { updateLead } from "../services/leadService";

import type { LeadFormData } from "../schemas/leadSchema";

function LeadUpdatePage() {

  const { id } = useParams();

  const navigate =
    useNavigate();

  const {

    lead,

    loading,

    error,

  } = useLead(Number(id));

  async function handleUpdate(
    data: LeadFormData
  ) {

    if (!lead)
      return;

    const response =
  await updateLead({
    id: lead.id,
    ...data,
  });

toast.success(response.message);

navigate("/leads");

  }

  if (loading)
    return <LoadingState />;

  if (error)
    return (

      <div className="rounded-2xl border border-red-200 bg-red-50 p-6 text-red-600">

        {error}

      </div>

    );

  if (!lead)
    return (

      <div className="rounded-2xl bg-white p-8 text-center shadow">

        Lead not found.

      </div>

    );

  return (

    <div className="space-y-8">

      <PageHeader
        title="Update Lead"
        description="Update lead information."
      />

      <div className="rounded-3xl border border-slate-200 bg-white p-8 shadow-sm">
        <LeadForm
          submitText="Update Lead"
          defaultValues={{
            firstName: lead.firstName,
            lastName: lead.lastName,
            companyName: lead.companyName ?? "",
            email: lead.email,
            phoneNumber: lead.phoneNumber,
            website: lead.website ?? "",
            address: lead.address ?? "",
            description: lead.description ?? "",
            status: lead.status,
            source: lead.source,
            assignedUserId:
              lead.assignedUserId ?? null,
          }}
          onSubmit={handleUpdate}
        />
      </div>

    </div>

  );

}

export default LeadUpdatePage;