import {
  ArrowRightLeft,
  Eye,
  LogIn,
  LogOut,
  Pencil,
  Plus,
  Trash2,
  User,
} from "lucide-react";

import { useNavigate } from "react-router-dom";

import type { ActivityLog } from "../types/ActivityLog";
import { ActivityActions } from "../types/ActivityAction";

type Props = {
  activityLogs: ActivityLog[];
};

function ActivityLogTable({
  activityLogs,
}: Props) {

  const navigate =
    useNavigate();

  function getAction(action: number) {

    switch (action) {

      case ActivityActions.Create:
        return {
          text: "Create",
          icon: Plus,
          className:
            "bg-emerald-100 text-emerald-700",
        };

      case ActivityActions.Update:
        return {
          text: "Update",
          icon: Pencil,
          className:
            "bg-amber-100 text-amber-700",
        };

      case ActivityActions.Delete:
        return {
          text: "Delete",
          icon: Trash2,
          className:
            "bg-red-100 text-red-700",
        };

      case ActivityActions.Convert:
        return {
          text: "Convert",
          icon: ArrowRightLeft,
          className:
            "bg-violet-100 text-violet-700",
        };

      case ActivityActions.Login:
        return {
          text: "Login",
          icon: LogIn,
          className:
            "bg-blue-100 text-blue-700",
        };

      case ActivityActions.Logout:
        return {
          text: "Logout",
          icon: LogOut,
          className:
            "bg-slate-200 text-slate-700",
        };

      default:
        return {
          text: "-",
          icon: User,
          className:
            "bg-slate-100 text-slate-600",
        };

    }

  }

  if (!activityLogs.length) {

    return (

      <div className="rounded-2xl border bg-white py-20 text-center text-slate-400">

        No activity logs found.

      </div>

    );

  }

  return (

    <div className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">

      <table className="w-full">

        <thead className="bg-slate-50">

          <tr className="text-left text-xs font-semibold uppercase tracking-wider text-slate-500">

            <th className="px-6 py-4">
              Action
            </th>

            <th className="px-6 py-4">
              Entity
            </th>

            <th className="px-6 py-4">
              Description
            </th>

            <th className="px-6 py-4">
              User
            </th>

            <th className="px-6 py-4">
              Date
            </th>

            <th className="px-6 py-4 text-right">
              Details
            </th>

          </tr>

        </thead>

        <tbody>

          {activityLogs.map((activity) => {

            const action =
              getAction(activity.action);

            const Icon =
              action.icon;

            return (

              <tr
                key={activity.id}
                className="border-t transition hover:bg-slate-50"
              >

                <td className="px-6 py-5">

                  <span
                    className={`inline-flex items-center gap-2 rounded-full px-3 py-1 text-xs font-semibold ${action.className}`}
                  >

                    <Icon size={14} />

                    {action.text}

                  </span>

                </td>

                <td className="px-6 py-5">

                  <span className="rounded-full bg-slate-100 px-3 py-1 text-sm font-medium text-slate-700">

                    {activity.entityName}

                  </span>

                </td>

                <td className="max-w-md px-6 py-5">

                  <p className="line-clamp-2 text-sm text-slate-700">

                    {activity.description}

                  </p>

                </td>

                <td className="px-6 py-5">

                  <div className="flex items-center gap-3">

                    <div className="flex h-9 w-9 items-center justify-center rounded-full bg-blue-100 font-semibold text-blue-700">

                      {activity.userName
                        ? activity.userName[0]
                        : "-"}

                    </div>

                    <span className="font-medium text-slate-700">

                      {activity.userName ?? "-"}

                    </span>

                  </div>

                </td>

                <td className="px-6 py-5 text-sm">

                  <div className="font-medium text-slate-800">

                    {new Date(
                      activity.createdDate
                    ).toLocaleDateString()}

                  </div>

                  <div className="text-slate-500">

                    {new Date(
                      activity.createdDate
                    ).toLocaleTimeString([], {
                      hour: "2-digit",
                      minute: "2-digit",
                    })}

                  </div>

                </td>

                <td className="px-6 py-5">

                  <div className="flex justify-end">

                    <button
                      onClick={() =>
                        navigate(
                          `/activitylogs/${activity.id}`
                        )
                      }
                      className="rounded-xl p-2 text-slate-500 transition hover:bg-blue-100 hover:text-blue-600"
                    >

                      <Eye size={18} />

                    </button>

                  </div>

                </td>

              </tr>

            );

          })}

        </tbody>

      </table>

    </div>

  );

}

export default ActivityLogTable;