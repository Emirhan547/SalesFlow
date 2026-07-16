import { useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";

import { Plus } from "lucide-react";

import { Button } from "@/components/ui/button";

import EmptyState from "@/components/common/EmptyState";
import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import TaskSearch from "../components/TaskSearch";
import TaskTable from "../components/TaskTable";
import TaskPagination from "../components/TaskPagination";

import { useTasks } from "../hooks/useTasks";
import { useDebounce } from "../hooks/useDebounce";

function TaskListPage() {

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
  } = useTasks(filter);

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
        title="No Tasks"
        description="There are no tasks."
      />
    );

  return (
    <div className="space-y-8">

      <PageHeader
        title="Tasks"
        description="Manage all tasks."
        action={
          <Button
            onClick={() =>
              navigate("/tasks/create")
            }
          >
            <Plus
              size={18}
              className="mr-2"
            />

            New Task

          </Button>
        }
      />

      <TaskSearch
        value={search}
        onChange={(value) => {

          setSearch(value);

          setPage(1);

        }}
      />

      <TaskTable
        tasks={data.items}
        onDeleted={reload}
      />

      <TaskPagination
        page={data.page}
        totalPages={data.totalPages}
        hasPrevious={data.hasPrevious}
        hasNext={data.hasNext}
        onPageChange={setPage}
      />

    </div>
  );
}

export default TaskListPage;