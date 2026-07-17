import { useParams } from "react-router-dom";

import Card from "@/components/ui/Card";

import DetailItem from "@/components/common/DetailItem";
import EmptyState from "@/components/common/EmptyState";
import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import { useMeeting } from "../hooks/useMeeting";

function MeetingDetailPage() {

  const { id } =
    useParams();

  const {
    meeting,
    loading,
    error,
  } = useMeeting(Number(id));

  if (loading)
    return <LoadingState />;

  if (error)
    return (
      <div className="rounded-2xl border border-red-200 bg-red-50 p-6 text-red-600">
        {error}
      </div>
    );

  if (!meeting)
    return (
      <EmptyState
        title="Meeting Not Found"
        description="Meeting not found."
      />
    );

  return (
    <div className="space-y-8">

      <PageHeader
        title="Meeting Detail"
        description="Meeting information."
      />

      <Card title="Meeting Information">

        <div className="grid gap-6 md:grid-cols-2">

          <DetailItem
            label="Title"
            value={meeting.title}
          />

          <DetailItem
            label="Start Date"
            value={
              new Date(
                meeting.startDate
              ).toLocaleString()
            }
          />

          <DetailItem
            label="End Date"
            value={
              new Date(
                meeting.endDate
              ).toLocaleString()
            }
          />

          <DetailItem
            label="Customer"
            value={
              meeting.customerName
            }
          />

          <DetailItem
            label="Assigned User"
            value={
              meeting.assignedUserName ??
              "Unassigned"
            }
          />

          <DetailItem
            label="Location"
            value={
              meeting.location ?? "-"
            }
          />

        </div>

      </Card>

      <Card title="Description">

        <p className="whitespace-pre-wrap text-slate-700">
          {meeting.description || "-"}
        </p>

      </Card>

    </div>
  );
}

export default MeetingDetailPage;