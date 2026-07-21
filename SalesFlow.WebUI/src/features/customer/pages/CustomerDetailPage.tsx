import { useMemo, useState } from "react";
import { useParams } from "react-router-dom";

import DetailItem from "@/components/common/DetailItem";
import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";
import Card from "@/components/ui/Card";

import AttachmentTable from "@/features/attachment/components/AttachmentTable";
import AttachmentUpload from "@/features/attachment/components/AttachmentUpload";
import { useAttachments } from "@/features/attachment/hooks/useAttachments";

import AIInsightsPanel from "../components/AIInsightsPanel";
import CustomerFollowUpEmailModal from "../components/CustomerFollowUpEmailModal";
import { useCustomer } from "../hooks/useCustomer";
import { useCustomerInsights } from "../hooks/useCustomerInsights";

function CustomerDetailPage() {

  const { id } = useParams();

  const customerId = Number(id);

  const [emailModalOpen, setEmailModalOpen] =
    useState(false);

  const attachmentFilter = useMemo(
    () => ({
      page: 1,
      pageSize: 100,
      customerId,
    }),
    [customerId]
  );

  const {
    customer,
    loading,
    error,
  } = useCustomer(customerId);

  const {
  insights,
  loading: insightsLoading,
  error: insightsError,
  reload: reloadInsights,
} = useCustomerInsights(customerId, false);

const [aiLoaded, setAiLoaded] =
  useState(false);

  const {
    data: attachmentData,
    loading: attachmentLoading,
    reload: reloadAttachments,
  } = useAttachments(attachmentFilter);

  if (loading)
    return <LoadingState />;

  if (error)
    return (
      <div className="rounded-2xl border border-red-200 bg-red-50 p-6 text-red-600">
        {error}
      </div>
    );

  if (!customer)
    return (
      <div className="rounded-2xl bg-white p-8 text-center shadow">
        Customer not found.
      </div>
    );

  return (

    <>

      <div className="space-y-8">

        <PageHeader
          title="Customer Detail"
          description="Customer information overview"
        />

        <Card title="Company Information">

          <div className="grid gap-6 md:grid-cols-2">

            <DetailItem
              label="Company Name"
              value={customer.companyName}
            />

            <DetailItem
              label="Website"
              value={customer.website}
            />

            <DetailItem
              label="Tax Number"
              value={customer.taxNumber}
            />

            <DetailItem
              label="Customer Type"
              value={
                customer.customerType === 1
                  ? "Individual"
                  : "Company"
              }
            />

          </div>

        </Card>

        <Card title="Contact Information">

          <div className="grid gap-6 md:grid-cols-2">

            <DetailItem
              label="First Name"
              value={customer.contactFirstName}
            />

            <DetailItem
              label="Last Name"
              value={customer.contactLastName}
            />

            <DetailItem
              label="Email"
              value={customer.email}
            />

            <DetailItem
              label="Phone"
              value={customer.phoneNumber}
            />

          </div>

        </Card>

        <Card>

          <div className="mb-8 flex flex-col gap-4 md:flex-row md:items-center md:justify-between">

            <div>

              <h2 className="text-xl font-semibold text-slate-900">

                ✨ AI Customer Insights

              </h2>

              <p className="mt-1 text-sm text-slate-500">

                AI-generated analysis and recommendations based on available CRM data.

              </p>

            </div>

            <button
              onClick={() => setEmailModalOpen(true)}
              className="rounded-xl bg-blue-600 px-5 py-2 font-medium text-white transition hover:bg-blue-700"
            >
              Generate Follow-up Email
            </button>

          </div>

         {!aiLoaded ? (

  <div className="rounded-xl border border-dashed border-slate-300 bg-slate-50 p-8 text-center">

    <p className="mb-4 text-slate-500">
      AI insights have not been generated yet.
    </p>

    <button
      onClick={async () => {
        await reloadInsights();
        setAiLoaded(true);
      }}
      className="rounded-lg bg-blue-600 px-5 py-2 text-white transition hover:bg-blue-700"
    >
      ✨ Generate AI Insights
    </button>

  </div>

) : insightsLoading ? (

  <LoadingState />

) : insightsError ? (

  <div className="rounded-xl border border-red-200 bg-red-50 p-4 text-red-600">
    {insightsError}
  </div>

) : insights ? (

  <AIInsightsPanel
    insights={insights}
  />

) : (

  <div className="rounded-xl border border-dashed border-slate-300 bg-slate-50 p-8 text-center text-slate-500">
    No AI insights available.
  </div>

)}

        </Card>

        <Card title="Address">

          <p>

            {customer.address || "-"}

          </p>

        </Card>

        <Card title="Description">

          <p>

            {customer.description || "-"}

          </p>

        </Card>

        <Card title="Attachments">

          <AttachmentUpload
            customerId={customerId}
            onUploaded={reloadAttachments}
          />

          <div className="mt-6">

            {attachmentLoading ? (

              <LoadingState />

            ) : (

              <AttachmentTable
                attachments={attachmentData?.items ?? []}
                onDeleted={reloadAttachments}
              />

            )}

          </div>

        </Card>

      </div>

      <CustomerFollowUpEmailModal
        open={emailModalOpen}
        customerId={customerId}
        onClose={() => setEmailModalOpen(false)}
      />

    </>

  );

}

export default CustomerDetailPage;