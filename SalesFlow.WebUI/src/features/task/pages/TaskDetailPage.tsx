import { useParams } from "react-router-dom";

import Card from "@/components/ui/Card";

import DetailItem from "@/components/common/DetailItem";
import EmptyState from "@/components/common/EmptyState";
import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import { useTask } from "../hooks/useTask";

import {
  TaskPriority,
} from "../types/TaskPriority";

import {
  TaskStatus,
} from "../types/TaskStatus";

function getPriorityText(
  priority: number
) {
  switch (priority) {

    case TaskPriority.Low:
      return "Low";

    case TaskPriority.Medium:
      return "Medium";

    case TaskPriority.High:
      return "High";

    case TaskPriority.Critical:
      return "Critical";

    default:
      return "-";
  }
}

function getStatusText(
  status: number
) {
  switch (status) {

    case TaskStatus.Pending:
      return "Pending";

    case TaskStatus.InProgress:
      return "In Progress";

    case TaskStatus.Completed:
      return "Completed";

    case TaskStatus.Cancelled:
      return "Cancelled";

    default:
      return "-";
  }
}

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
            value={
              new Date(
                task.dueDate
              ).toLocaleString()
            }
          />

          <DetailItem
            label="Priority"
            value={
              getPriorityText(
                task.priority
              )
            }
          />

          <DetailItem
            label="Status"
            value={
              getStatusText(
                task.status
              )
            }
          />

          <DetailItem
            label="Customer"
            value={
              task.customerName
            }
          />

          <DetailItem
            label="Assigned User"
            value={
              task.assignedUserName ??
              "Unassigned"
            }
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