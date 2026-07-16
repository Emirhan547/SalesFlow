import { useMemo } from "react";
import { useParams } from "react-router-dom";

import DetailItem from "@/components/common/DetailItem";
import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";
import Card from "@/components/ui/Card";

import AttachmentTable from "@/features/attachment/components/AttachmentTable";
import { useAttachments } from "@/features/attachment/hooks/useAttachments";

import { useCustomer } from "../hooks/useCustomer";
import { CustomerType } from "../types/CustomerType";
import AttachmentUpload from "@/features/attachment/components/AttachmentUpload";
function CustomerDetailPage() {
  const { id } = useParams();

  const customerId = Number(id);

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
              customer.customerType === CustomerType.Individual
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

      <Card title="Address">

        <p>{customer.address || "-"}</p>

      </Card>

      <Card title="Description">

        <p>{customer.description || "-"}</p>

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
  );
}

export default CustomerDetailPage;