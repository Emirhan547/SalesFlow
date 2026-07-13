import { CalendarDays } from "lucide-react";

import Card from "@/components/ui/Card";

import type { UpcomingMeetingDto } from "../types/DashboardDto";

type Props = {
  meetings: UpcomingMeetingDto[];
};

function UpcomingMeetings({ meetings }: Props) {
  return (
    <Card
      title="Upcoming Meetings"
      subtitle="Scheduled meetings"
    >
      {meetings.length === 0 ? (

        <div className="flex h-56 items-center justify-center text-slate-400">
          No meetings scheduled.
        </div>

      ) : (

        <div className="space-y-2">

          {meetings.map((meeting) => (

            <div
              key={meeting.id}
              className="flex items-center justify-between rounded-2xl p-4 transition-all duration-200 hover:bg-slate-50"
            >

              <div className="flex items-center gap-4">

                <div className="flex h-11 w-11 items-center justify-center rounded-2xl bg-orange-100 text-orange-600">

                  <CalendarDays size={18} />

                </div>

                <h4 className="font-semibold text-slate-900">
                  {meeting.title}
                </h4>

              </div>

              <div className="rounded-xl bg-slate-100 px-3 py-2 text-sm font-medium text-slate-600">

                {new Date(meeting.startDate).toLocaleDateString("tr-TR")}

              </div>

            </div>

          ))}

        </div>

      )}
    </Card>
  );
}

export default UpcomingMeetings;