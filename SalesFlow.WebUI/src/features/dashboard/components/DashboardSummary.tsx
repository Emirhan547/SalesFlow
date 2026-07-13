import {
  Briefcase,
  CalendarDays,
  CheckSquare,
  UserPlus,
  Users,
} from "lucide-react";

import DashboardCard from "./DashboardCard";

import type { DashboardSummaryDto } from "../types/DashboardDto";

type Props = {
  summary: DashboardSummaryDto;
};

function DashboardSummary({ summary }: Props) {
  return (
    <div className="grid gap-6 md:grid-cols-2 xl:grid-cols-5">

      <DashboardCard
        title="Customers"
        value={summary.totalCustomers}
        icon={Users}
        color="bg-blue-600"
      />

      <DashboardCard
        title="Leads"
        value={summary.totalLeads}
        icon={UserPlus}
        color="bg-green-600"
      />

      <DashboardCard
        title="Deals"
        value={summary.totalDeals}
        icon={Briefcase}
        color="bg-violet-600"
      />

      <DashboardCard
        title="Meetings"
        value={summary.totalMeetings}
        icon={CalendarDays}
        color="bg-orange-500"
      />

      <DashboardCard
        title="Tasks"
        value={summary.totalTasks}
        icon={CheckSquare}
        color="bg-rose-600"
      />

    </div>
  );
}

export default DashboardSummary;