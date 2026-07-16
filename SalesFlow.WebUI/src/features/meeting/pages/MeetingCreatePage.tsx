import { useNavigate } from "react-router-dom";

import { toast } from "sonner";

import PageHeader from "@/components/common/PageHeader";

import MeetingForm from "../components/MeetingForm";

import { createMeeting } from "../services/meetingService";

import type { MeetingFormData } from "../schemas/meetingSchema";

function MeetingCreatePage() {

  const navigate =
    useNavigate();

  async function handleCreate(
    data: MeetingFormData
  ) {

    const response =
      await createMeeting(data);

    if (!response.isSuccess) {

      toast.error(response.message);

      return;

    }

    toast.success(response.message);

    navigate("/meetings");

  }

  return (
    <div className="space-y-8">

      <PageHeader
        title="Create Meeting"
        description="Create a new meeting."
      />

      <MeetingForm
        submitText="Create Meeting"
        onSubmit={handleCreate}
      />

    </div>
  );
}

export default MeetingCreatePage;