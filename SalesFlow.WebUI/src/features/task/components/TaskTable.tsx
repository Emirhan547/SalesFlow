import { useNavigate } from "react-router-dom";

import {
  Eye,
  Pencil,
  Trash2,
} from "lucide-react";

import Card from "@/components/ui/Card";

import type { Task } from "../types/Task";

import {
  TaskPriority,
} from "../types/TaskPriority";

import {
  TaskStatus,
} from "../types/TaskStatus";

type Props = {
  tasks: Task[];
  onDeleted?: () => void;
};

function TaskTable({
  tasks,
}: Props) {

  const navigate =
    useNavigate();

  function getPriorityBadge(
    priority: number
  ) {

    switch (priority) {

      case TaskPriority.Low:
        return {
          label: "Low",
          className:
            "bg-slate-100 text-slate-700",
        };

      case TaskPriority.Medium:
        return {
          label: "Medium",
          className:
            "bg-blue-50 text-blue-700",
        };

      case TaskPriority.High:
        return {
          label: "High",
          className:
            "bg-orange-50 text-orange-700",
        };

      case TaskPriority.Critical:
        return {
          label: "Critical",
          className:
            "bg-red-50 text-red-700",
        };

      default:
        return {
          label: "-",
          className:
            "bg-slate-100 text-slate-500",
        };

    }

  }

  function getStatusBadge(
    status: number
  ) {

    switch (status) {

      case TaskStatus.Pending:
        return {
          label: "Pending",
          className:
            "bg-amber-50 text-amber-700",
        };

      case TaskStatus.InProgress:
        return {
          label: "In Progress",
          className:
            "bg-blue-50 text-blue-700",
        };

      case TaskStatus.Completed:
        return {
          label: "Completed",
          className:
            "bg-emerald-50 text-emerald-700",
        };

      case TaskStatus.Cancelled:
        return {
          label: "Cancelled",
          className:
            "bg-red-50 text-red-700",
        };

      default:
        return {
          label: "-",
          className:
            "bg-slate-100 text-slate-500",
        };

    }

  }

  return (
    <Card
      title="Tasks"
      subtitle={`${tasks.length} task(s)`}
    >
      <div className="overflow-x-auto">

        <table className="w-full">

          <thead>

            <tr className="border-b">

              <th className="py-4 text-left">
                Title
              </th>

              <th>
                Due Date
              </th>

              <th>
                Priority
              </th>

              <th>
                Status
              </th>

              <th className="text-right">
                Actions
              </th>

            </tr>

          </thead>

          <tbody>

            {tasks.map((task) => {

              const priority =
                getPriorityBadge(
                  task.priority
                );

              const status =
                getStatusBadge(
                  task.status
                );

              return (

                <tr
                  key={task.id}
                  className="border-b"
                >

                  <td className="py-4">
                    {task.title}
                  </td>

                  <td>
                    {new Date(
                      task.dueDate
                    ).toLocaleString()}
                  </td>

                  <td>

                    <span
                      className={`inline-flex rounded-full px-3 py-1 text-xs font-semibold ${priority.className}`}
                    >
                      {priority.label}
                    </span>

                  </td>

                  <td>

                    <span
                      className={`inline-flex rounded-full px-3 py-1 text-xs font-semibold ${status.className}`}
                    >
                      {status.label}
                    </span>

                  </td>

                  <td>

                    <div className="flex justify-end gap-2">

                      <button
                        onClick={() =>
                          navigate(
                            `/tasks/${task.id}`
                          )
                        }
                      >
                        <Eye size={18} />
                      </button>

                     {task.status !== TaskStatus.Completed &&
  task.status !== TaskStatus.Cancelled && (

    <button
      onClick={() =>
        navigate(
          `/tasks/edit/${task.id}`
        )
      }
      title="Edit task"
    >
      <Pencil size={18} />
    </button>

  )}

                      <button>
                        <Trash2 size={18} />
                      </button>

                    </div>

                  </td>

                </tr>

              );

            })}

          </tbody>

        </table>

      </div>

    </Card>
  );
}

export default TaskTable;