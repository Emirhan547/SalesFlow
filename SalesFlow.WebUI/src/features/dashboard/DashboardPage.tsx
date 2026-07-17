import { useEffect } from "react";

import DashboardHeader from "./components/DashboardHeader";
import DashboardLeadChart from "./components/DashboardLeadChart";
import DashboardSalesChart from "./components/DashboardSalesChart";
import DashboardSkeleton from "./components/DashboardSkeleton";
import DashboardSummary from "./components/DashboardSummary";
import QuickActions from "./components/QuickActions";
import RecentCustomers from "./components/RecentCustomers";
import RecentDeals from "./components/RecentDeals";
import UpcomingMeetings from "./components/UpcomingMeetings";
import UpcomingTasks from "./components/UpcomingTasks";
// import RecentActivities from "./components/RecentActivities";

import { useDashboard } from "./hooks/useDashboard";

import signalRService from "@/services/signalRService";

function DashboardPage() {
  const {
    data,
    loading,
    error,
    reload,
  } = useDashboard();

  useEffect(() => {
    signalRService.on(
      "DashboardUpdated",
      reload
    );

    return () => {
      signalRService.off(
        "DashboardUpdated"
      );
    };
  }, [reload]);

  if (loading) {
    return <DashboardSkeleton />;
  }

  if (error) {
    return (
      <div className="rounded-3xl border border-red-200 bg-red-50 p-8 text-red-600">
        {error}
      </div>
    );
  }

  if (!data) {
    return (
      <div className="rounded-3xl bg-white p-10 text-center shadow-sm">
        No dashboard data found.
      </div>
    );
  }

  return (
    <div className="space-y-8">

      <DashboardHeader
        onRefresh={reload}
      />

      <DashboardSummary
        summary={data.summary}
      />

      <QuickActions />

      <div className="grid gap-8 xl:grid-cols-3">

        <div className="xl:col-span-2">

          <DashboardSalesChart
            sales={data.sales}
          />

        </div>

        <DashboardLeadChart
          leads={data.leads}
        />

      </div>

      <div className="grid gap-8 xl:grid-cols-2">

        <RecentCustomers
          customers={data.recent.customers}
        />

        <RecentDeals
          deals={data.recent.deals}
        />

      </div>

      <div className="grid gap-8 xl:grid-cols-2">

        <UpcomingMeetings
          meetings={data.recent.upcomingMeetings}
        />

        <UpcomingTasks
          tasks={data.recent.upcomingTasks}
        />

      </div>

      {/*
      <RecentActivities
        activities={data.recentActivities}
      />
      */}

    </div>
  );
}

export default DashboardPage;