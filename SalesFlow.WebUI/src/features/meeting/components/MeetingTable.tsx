import { useState } from "react";
import { useNavigate } from "react-router-dom";

import {
  Calendar,
  Clock,
  Eye,
  Pencil,
  Trash2,
} from "lucide-react";

import Card from "@/components/ui/Card";
import ConfirmDialog from "@/components/ui/ConfirmDialog";

import { useDeleteMeeting } from "../hooks/useDeleteMeeting";

import type { Meeting } from "../types/Meeting";

type Props = {
  meetings: Meeting[];
  onDeleted?: () => void;
};

function getStatus(
  status: number
) {
  switch (status) {

    case 1:
      return "Scheduled";

    case 2:
      return "Completed";

    case 3:
      return "Cancelled";

    default:
      return "-";

  }
}

function MeetingTable({
  meetings,
  onDeleted,
}: Props) {

  const navigate =
    useNavigate();

  const [selectedId, setSelectedId] =
    useState<number>();

  const [dialogOpen, setDialogOpen] =
    useState(false);

  const {
    loading,
    remove,
  } = useDeleteMeeting();

  if (meetings.length === 0) {

    return (
      <Card title="Meetings">

        <div className="flex h-72 items-center justify-center text-slate-400">

          No meetings found.

        </div>

      </Card>
    );

  }

  return (
    <>

      <Card
        title="Meetings"
        subtitle={`${meetings.length} meeting(s)`}
      >

        <div className="overflow-x-auto">

          <table className="w-full">

            <thead>

              <tr className="border-b border-slate-200 text-left text-sm font-semibold text-slate-500">

                <th className="pb-4">
                  Title
                </th>

                <th className="pb-4">
                  Date
                </th>

                <th className="pb-4">
                  Status
                </th>

                <th className="pb-4 text-right">
                  Actions
                </th>

              </tr>

            </thead>

            <tbody>

              {meetings.map((meeting) => (

                <tr
                  key={meeting.id}
                  className="border-b border-slate-100 hover:bg-slate-50"
                >

                  <td className="py-5">

                    <div className="flex items-center gap-3">

                      <Calendar
                        size={18}
                      />

                      {meeting.title}

                    </div>

                  </td>

                  <td>

                    <div className="flex items-center gap-2">

                      <Clock
                        size={16}
                      />

                      {new Date(
                        meeting.startDate
                      ).toLocaleString()}

                    </div>

                  </td>

                  <td>

                    {getStatus(
                      meeting.status
                    )}

                  </td>

                  <td>

                    <div className="flex justify-end gap-2">

                      <button
                        onClick={() =>
                          navigate(`/meetings/${meeting.id}`)
                        }
                      >
                        <Eye size={18} />
                      </button>

                      <button
                        onClick={() =>
                          navigate(`/meetings/edit/${meeting.id}`)
                        }
                      >
                        <Pencil size={18} />
                      </button>

                      <button
                        onClick={() => {

                          setSelectedId(meeting.id);

                          setDialogOpen(true);

                        }}
                      >
                        <Trash2 size={18} />
                      </button>

                    </div>

                  </td>

                </tr>

              ))}

            </tbody>

          </table>

        </div>

      </Card>

      <ConfirmDialog
        open={dialogOpen}
        loading={loading}
        title="Delete Meeting"
        description="Are you sure?"
        onCancel={() => {

          setDialogOpen(false);

          setSelectedId(undefined);

        }}
        onConfirm={async () => {

          if (!selectedId)
            return;

          const success =
            await remove(selectedId);

          if (!success)
            return;

          setDialogOpen(false);

          setSelectedId(undefined);

          onDeleted?.();

        }}
      />

    </>
  );
}

export default MeetingTable;