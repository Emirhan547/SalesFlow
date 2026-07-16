import { useParams } from "react-router-dom";

import Card from "@/components/ui/Card";

import DetailItem from "@/components/common/DetailItem";
import EmptyState from "@/components/common/EmptyState";
import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import { useTask } from "../hooks/useTask";

function TaskDetailPage() {

  const { id } =
    useParams();

  const {
    task,
    loading,
    error,
  } = useTask(Number(id));

  if (loading)
    return <LoadingState />;

  if (error)
    return (
      <div className="rounded-2xl border border-red-200 bg-red-50 p-6 text-red-600">
        {error}
      </div>
    );

  if (!task)
    return (
      <EmptyState
        title="Task Not Found"
        description="Task not found."
      />
    );

  return (
    <div className="space-y-8">

      <PageHeader
        title="Task Detail"
        description="Task information."
      />

      <Card title="Task Information">

        <div className="grid gap-6 md:grid-cols-2">

          <DetailItem
            label="Title"
            value={task.title}
          />

          <DetailItem
            label="Due Date"
            value={task.dueDate}
          />

          <DetailItem
            label="Priority"
            value={task.priority}
          />

          <DetailItem
            label="Status"
            value={task.status}
          />

          <DetailItem
            label="Customer Id"
            value={task.customerId}
          />

          <DetailItem
            label="Assigned User"
            value={task.assignedUserId ?? "-"}
          />

        </div>

      </Card>

      <Card title="Description">

        <p className="whitespace-pre-wrap text-slate-700">

          {task.description || "-"}

        </p>

      </Card>

    </div>
  );
}

export default TaskDetailPage;