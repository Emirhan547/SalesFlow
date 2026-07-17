import {
  useEffect,
  useMemo,
  useState,
} from "react";

import LoadingState from "@/components/common/LoadingState";

import ActivityLogHeader from "../components/ActivityLogHeader";
import ActivityLogPagination from "../components/ActivityLogPagination";
import ActivityLogSearch from "../components/ActivityLogSearch";
import ActivityLogTable from "../components/ActivityLogTable";

import { useActivityLogs } from "../hooks/useActivityLogs";

import signalRService from "@/services/signalRService";

function ActivityLogListPage() {

  const [page, setPage] =
    useState(1);

  const [search, setSearch] =
    useState("");

  const filter = useMemo(() => ({
    page,
    pageSize: 10,
    entityName: search,
  }), [page, search]);

  const {
    data,
    loading,
    error,
    reload,
  } = useActivityLogs(filter);

  useEffect(() => {

    signalRService.on(
      "ActivityLogCreated",
      reload
    );

    return () => {

      signalRService.off(
        "ActivityLogCreated"
      );

    };

  }, [reload]);

  if (loading)
    return <LoadingState />;

  if (error)
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 p-6 text-red-600">
        {error}
      </div>
    );

  return (

    <div className="space-y-6">

      <ActivityLogHeader />

      <ActivityLogSearch
        value={search}
        onChange={(value) => {

          setPage(1);

          setSearch(value);

        }}
      />

      <ActivityLogTable
        activityLogs={data?.items ?? []}
      />

      {data && (

        <ActivityLogPagination
          page={data.page}
          totalPages={data.totalPages}
          hasPrevious={data.hasPrevious}
          hasNext={data.hasNext}
          onPageChange={setPage}
        />

      )}

    </div>

  );
}

export default ActivityLogListPage;