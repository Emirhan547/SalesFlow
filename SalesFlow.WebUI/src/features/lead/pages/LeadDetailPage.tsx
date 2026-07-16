import { useParams } from "react-router-dom";

import Card from "@/components/ui/Card";

import DetailItem from "@/components/common/DetailItem";
import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import { useLead } from "../hooks/useLead";

function getStatusText(status: number) {
  switch (status) {
    case 1:
      return "New";
    case 2:
      return "Contacted";
    case 3:
      return "Qualified";
    case 4:
      return "Lost";
    case 5:
      return "Converted";
    default:
      return "-";
  }
}

function getSourceText(source: number) {
  switch (source) {
    case 1:
      return "Website";
    case 2:
      return "Phone";
    case 3:
      return "Email";
    case 4:
      return "Referral";
    case 5:
      return "Social";
    case 6:
      return "Advertisement";
    case 7:
      return "Other";
    default:
      return "-";
  }
}

function LeadDetailPage() {

  const { id } =
    useParams();

  const {

    lead,

    loading,

    error,

  } = useLead(Number(id));

  if (loading)
    return <LoadingState />;

  if (error)
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 p-6 text-red-600">
        {error}
      </div>
    );

  if (!lead)
    return (
      <div className="rounded-xl bg-white p-8 text-center shadow">
        Lead not found.
      </div>
    );

  return (

    <div className="space-y-8">

      <PageHeader
        title="Lead Detail"
        description="Lead information"
      />

      <Card title="General">

        <div className="grid gap-6 md:grid-cols-2">

          <DetailItem
            label="First Name"
            value={lead.firstName}
          />

          <DetailItem
            label="Last Name"
            value={lead.lastName}
          />

          <DetailItem
            label="Company"
            value={lead.companyName}
          />

          <DetailItem
            label="Email"
            value={lead.email}
          />

          <DetailItem
            label="Phone"
            value={lead.phoneNumber}
          />

          <DetailItem
            label="Website"
            value={lead.website}
          />

          <DetailItem
            label="Status"
            value={getStatusText(lead.status)}
          />

          <DetailItem
            label="Source"
            value={getSourceText(lead.source)}
          />

        </div>

      </Card>

      <Card title="Address">

        <p>

          {lead.address || "-"}

        </p>

      </Card>

      <Card title="Description">

        <p>

          {lead.description || "-"}

        </p>

      </Card>

    </div>

  );

}

export default LeadDetailPage;