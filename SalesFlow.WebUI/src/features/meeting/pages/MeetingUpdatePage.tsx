import {
  useNavigate,
  useParams,
} from "react-router-dom";

import { toast } from "sonner";

import EmptyState from "@/components/common/EmptyState";
import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import MeetingForm from "../components/MeetingForm";

import { useMeeting } from "../hooks/useMeeting";

import { updateMeeting } from "../services/meetingService";

import type { MeetingFormData } from "../schemas/meetingSchema";

function MeetingUpdatePage() {

  const { id } =
    useParams();

  const navigate =
    useNavigate();

  const {
    meeting,
    loading,
    error,
  } = useMeeting(Number(id));

  async function handleUpdate(
    data: MeetingFormData
  ) {

    if (!meeting)
      return;

    const response =
      await updateMeeting({

        ...data,

        id: meeting.id,

        status: meeting.status,

      });

    if (!response.isSuccess) {

      toast.error(response.message);

      return;

    }

    toast.success(response.message);

    navigate("/meetings");

  }

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
        title="Update Meeting"
        description="Update meeting information."
      />

      <MeetingForm
        submitText="Update Meeting"
        defaultValues={{

          title: meeting.title,

          description:
            meeting.description ?? "",

          startDate:
            meeting.startDate,

          endDate:
            meeting.endDate,

          type:
            meeting.type,

          status:
            meeting.status,

          location:
            meeting.location ?? "",

          customerId:
            meeting.customerId,

          assignedUserId:
            meeting.assignedUserId,

        }}
        onSubmit={handleUpdate}
      />

    </div>
  );
}

export default MeetingUpdatePage;