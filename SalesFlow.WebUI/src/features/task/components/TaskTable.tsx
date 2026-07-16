import { useNavigate } from "react-router-dom";

import {
  Eye,
  Pencil,
  Trash2,
} from "lucide-react";

import Card from "@/components/ui/Card";

import type { Task } from "../types/Task";

type Props = {
  tasks: Task[];
  onDeleted?: () => void;
};

function TaskTable({
  tasks,
}: Props) {

  const navigate =
    useNavigate();

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

            {tasks.map(task => (

              <tr
                key={task.id}
                className="border-b"
              >

                <td className="py-4">
                  {task.title}
                </td>

                <td>
                  {task.dueDate}
                </td>

                <td>
                  {task.priority}
                </td>

                <td>
                  {task.status}
                </td>

                <td>

                  <div className="flex justify-end gap-2">

                    <button
                      onClick={() =>
                        navigate(`/tasks/${task.id}`)
                      }
                    >
                      <Eye size={18} />
                    </button>

                    <button
                      onClick={() =>
                        navigate(`/tasks/edit/${task.id}`)
                      }
                    >
                      <Pencil size={18} />
                    </button>

                    <button>
                      <Trash2 size={18} />
                    </button>

                  </div>

                </td>

              </tr>

            ))}

          </tbody>

        </table>

      </div>

    </Card>
  );
}

export default TaskTable;