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
            value={meeting.startDate}
          />

          <DetailItem
            label="End Date"
            value={meeting.endDate}
          />

          <DetailItem
            label="Customer Id"
            value={meeting.customerId}
          />

          <DetailItem
            label="Assigned User"
            value={
              meeting.assignedUserId ?? "-"
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

        <p className="text-slate-700 whitespace-pre-wrap">

          {meeting.description || "-"}

        </p>

      </Card>

    </div>
  );
}

export default MeetingDetailPage;