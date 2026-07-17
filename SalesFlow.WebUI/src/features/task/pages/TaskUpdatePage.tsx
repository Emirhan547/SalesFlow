import {
  useNavigate,
  useParams,
} from "react-router-dom";

import { toast } from "sonner";

import EmptyState from "@/components/common/EmptyState";
import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import TaskForm from "../components/TaskForm";

import { useTask } from "../hooks/useTask";

import { updateTask } from "../services/taskService";

import type { TaskFormData } from "../schemas/taskSchema";

function TaskUpdatePage() {

  const { id } =
    useParams();

  const navigate =
    useNavigate();

  const {
    task,
    loading,
    error,
  } = useTask(Number(id));

 async function handleUpdate(
  data: TaskFormData
) {

  if (!task)
    return;

  const response =
    await updateTask({

      ...data,

      id: task.id,

      status:
        data.status ??
        task.status,

    });

  if (!response.isSuccess) {

    toast.error(
      response.message
    );

    return;

  }

  toast.success(
    response.message
  );

  navigate("/tasks");
}

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
        title="Update Task"
        description="Update task information."
      />

     <TaskForm
  submitText="Update Task"
  currentStatus={task.status}
  defaultValues={{

    title:
      task.title,

    description:
      task.description ?? "",

    dueDate:
      task.dueDate,

    priority:
      task.priority,

    status:
      task.status,

    customerId:
      task.customerId,

    assignedUserId:
      task.assignedUserId,

  }}
  onSubmit={handleUpdate}
/>

    </div>
  );
}

export default TaskUpdatePage;