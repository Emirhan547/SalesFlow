import {
  ArrowRight,
  CheckSquare,
} from "lucide-react";

import Card from "@/components/ui/Card";

import type { UpcomingTaskDto } from "../types/DashboardDto";

type Props = {
  tasks: UpcomingTaskDto[];
};

function UpcomingTasks({ tasks }: Props) {
  return (
    <Card
      title="Upcoming Tasks"
      subtitle="Tasks waiting to be completed"
    >
      {tasks.length === 0 ? (

        <div className="flex h-60 items-center justify-center text-slate-400">
          No tasks available.
        </div>

      ) : (

        <div className="space-y-2">

          {tasks.map((task) => (

            <div
              key={task.id}
              className="flex items-center justify-between rounded-2xl p-4 transition hover:bg-slate-50"
            >

              <div className="flex items-center gap-4">

                <div className="flex h-11 w-11 items-center justify-center rounded-2xl bg-emerald-100 text-emerald-600">

                  <CheckSquare size={18} />

                </div>

                <div>

                  <h4 className="font-semibold text-slate-900">
                    {task.title}
                  </h4>

                  <p className="text-sm text-slate-500">
                    {new Date(task.dueDate).toLocaleDateString("tr-TR")}
                  </p>

                </div>

              </div>

              <ArrowRight
                size={18}
                className="text-slate-400"
              />

            </div>

          ))}

        </div>

      )}
    </Card>
  );
}

export default UpcomingTasks;