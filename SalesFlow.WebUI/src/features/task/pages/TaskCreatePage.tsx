import { useNavigate } from "react-router-dom";

import { toast } from "sonner";

import PageHeader from "@/components/common/PageHeader";

import TaskForm from "../components/TaskForm";

import { createTask } from "../services/taskService";

import type { TaskFormData } from "../schemas/taskSchema";

function TaskCreatePage() {

  const navigate =
    useNavigate();

  async function handleCreate(
    data: TaskFormData
  ) {

    const response =
      await createTask(data);

    if (!response.isSuccess) {

      toast.error(response.message);

      return;

    }

    toast.success(response.message);

    navigate("/tasks");

  }

  return (
    <div className="space-y-8">

      <PageHeader
        title="Create Task"
        description="Create a new task."
      />

      <TaskForm
        submitText="Create Task"
        onSubmit={handleCreate}
      />

    </div>
  );
}

export default TaskCreatePage;