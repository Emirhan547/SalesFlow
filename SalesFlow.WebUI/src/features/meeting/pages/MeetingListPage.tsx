import { useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";

import { Plus } from "lucide-react";

import { Button } from "@/components/ui/button";

import EmptyState from "@/components/common/EmptyState";
import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import MeetingSearch from "../components/MeetingSearch";
import MeetingTable from "../components/MeetingTable";
import MeetingPagination from "../components/MeetingPagination";

import { useMeetings } from "../hooks/useMeetings";
import { useDebounce } from "../hooks/useDebounce";

function MeetingListPage() {

  const navigate =
    useNavigate();

  const [search, setSearch] =
    useState("");

  const [page, setPage] =
    useState(1);

  const debouncedSearch =
    useDebounce(search);

  const filter = useMemo(
    () => ({
      page,
      pageSize: 10,
      search: debouncedSearch,
    }),
    [page, debouncedSearch]
  );

  const {
    data,
    loading,
    error,
    reload,
  } = useMeetings(filter);

  if (loading)
    return <LoadingState />;

  if (error)
    return (
      <div className="rounded-2xl border border-red-200 bg-red-50 p-6 text-red-600">
        {error}
      </div>
    );

  if (!data)
    return (
      <EmptyState
        title="No Meetings"
        description="There are no meetings."
      />
    );

  return (
    <div className="space-y-8">

      <PageHeader
        title="Meetings"
        description="Manage meetings."
        action={
          <Button
            onClick={() =>
              navigate("/meetings/create")
            }
          >
            <Plus
              size={18}
              className="mr-2"
            />

            New Meeting

          </Button>
        }
      />

      <MeetingSearch
        value={search}
        onChange={(value) => {

          setSearch(value);

          setPage(1);

        }}
      />

      <MeetingTable
        meetings={data.items}
        onDeleted={reload}
      />

      <MeetingPagination
        page={data.page}
        totalPages={data.totalPages}
        hasPrevious={data.hasPrevious}
        hasNext={data.hasNext}
        onPageChange={setPage}
      />

    </div>
  );
}

export default MeetingListPage;