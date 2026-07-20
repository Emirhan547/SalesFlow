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
            "bg-blue-100 text-blue-700",
        };

      case TaskPriority.High:
        return {
          label: "High",
          className:
            "bg-orange-100 text-orange-700",
        };

      case TaskPriority.Critical:
        return {
          label: "Critical",
          className:
            "bg-red-100 text-red-700",
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
            "bg-amber-100 text-amber-700",
        };

      case TaskStatus.InProgress:
        return {
          label: "In Progress",
          className:
            "bg-blue-100 text-blue-700",
        };

      case TaskStatus.Completed:
        return {
          label: "Completed",
          className:
            "bg-green-100 text-green-700",
        };

      case TaskStatus.Cancelled:
        return {
          label: "Cancelled",
          className:
            "bg-red-100 text-red-700",
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
      <div className="overflow-x-auto -mx-6">

        <table className="w-full">

          <thead>

            <tr className="border-b border-slate-200 text-left text-xs font-semibold uppercase tracking-wide text-slate-600 bg-slate-50">

              <th className="px-6 py-4">
                Title
              </th>

              <th className="px-6 py-4">
                Due Date
              </th>

              <th className="px-6 py-4">
                Priority
              </th>

              <th className="px-6 py-4">
                Status
              </th>

              <th className="px-6 py-4 text-right">
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
                  className="border-b border-slate-100 transition-all duration-200 hover:bg-blue-50/50"
                >

                  <td className="px-6 py-5 text-sm text-slate-900 font-semibold">
                    {task.title}
                  </td>

                  <td className="px-6 py-5 text-sm text-slate-600">
                    {new Date(
                      task.dueDate
                    ).toLocaleDateString()}
                  </td>

                  <td className="px-6 py-5">

                    <span
                     className={`inline-flex rounded-md px-2.5 py-1 text-xs font-semibold ${priority.className}`}
                    >
                      {priority.label}
                    </span>

                  </td>

                  <td className="px-6 py-5">

                    <span
                     className={`inline-flex rounded-md px-2.5 py-1 text-xs font-semibold ${status.className}`}
                    >
                      {status.label}
                    </span>

                  </td>

                  <td className="px-6 py-5">

                    <div className="flex justify-end gap-1">

                      <button
                        onClick={() =>
                          navigate(
                            `/tasks/${task.id}`
                          )
                        }
                       className="rounded-lg p-2 text-slate-500 transition-all hover:bg-blue-100 hover:text-blue-600"
                       title="View"
                     >
                       <Eye size={16} />
                     </button>

                     {task.status !== TaskStatus.Completed &&
  task.status !== TaskStatus.Cancelled && (

    <button
      onClick={() =>
        navigate(
          `/tasks/edit/${task.id}`
        )
      }
      className="rounded-lg p-2 text-slate-500 transition-all hover:bg-amber-100 hover:text-amber-600"
      title="Edit task"
    >
      <Pencil size={16} />
    </button>

  )}

                     <button className="rounded-lg p-2 text-slate-500 transition-all hover:bg-red-100 hover:text-red-600" title="Delete">
                       <Trash2 size={16} />
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