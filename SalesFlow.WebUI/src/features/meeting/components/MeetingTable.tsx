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
      return { label: "Scheduled", color: "bg-blue-100 text-blue-700" };

    case 2:
      return { label: "Completed", color: "bg-green-100 text-green-700" };

    case 3:
      return { label: "Cancelled", color: "bg-red-100 text-red-700" };

    default:
      return { label: "-", color: "bg-slate-100 text-slate-700" };

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

        <div className="overflow-x-auto -mx-6">

          <table className="w-full">

            <thead>

              <tr className="border-b border-slate-200 text-left text-xs font-semibold uppercase tracking-wide text-slate-600 bg-slate-50">

                <th className="px-6 py-4">
                  Title
                </th>

                <th className="px-6 py-4">
                  Date
                </th>

                <th className="px-6 py-4">
                  Status
                </th>

                <th className="px-6 py-4 text-right">
                  Actions
                </th>

              </tr>

            </thead>

            <tbody>

              {meetings.map((meeting) => {
                const statusData = getStatus(meeting.status);
                return (

                <tr
                  key={meeting.id}
                  className="border-b border-slate-100 transition-all duration-200 hover:bg-blue-50/50"
                >

                  <td className="px-6 py-5">

                    <div className="flex items-center gap-3">

                      <Calendar
                        size={18}
                        className="text-slate-400"
                      />

                      <span className="text-sm font-semibold text-slate-900">{meeting.title}</span>

                    </div>

                  </td>

                  <td className="px-6 py-5">

                    <div className="flex items-center gap-2 text-sm text-slate-600">

                      <Clock
                        size={16}
                        className="text-slate-400"
                      />

                      {new Date(
                        meeting.startDate
                      ).toLocaleDateString()}

                    </div>

                  </td>

                  <td className="px-6 py-5">

                    <span className={`rounded-md px-2.5 py-1 text-xs font-semibold ${statusData.color}`}>
                      {statusData.label}
                    </span>

                  </td>

                  <td className="px-6 py-5">

                    <div className="flex justify-end gap-1">

                      <button
                        onClick={() =>
                          navigate(`/meetings/${meeting.id}`)
                        }
                        className="rounded-lg p-2 text-slate-500 transition-all hover:bg-blue-100 hover:text-blue-600"
                        title="View"
                      >
                        <Eye size={16} />
                      </button>

                      <button
                        onClick={() =>
                          navigate(`/meetings/edit/${meeting.id}`)
                        }
                        className="rounded-lg p-2 text-slate-500 transition-all hover:bg-amber-100 hover:text-amber-600"
                        title="Edit"
                      >
                        <Pencil size={16} />
                      </button>

                      <button
                        onClick={() => {

                          setSelectedId(meeting.id);

                          setDialogOpen(true);

                        }}
                        className="rounded-lg p-2 text-slate-500 transition-all hover:bg-red-100 hover:text-red-600"
                        title="Delete"
                      >
                        <Trash2 size={16} />
                      </button>

                    </div>

                  </td>

                </tr>

              );
              })}

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