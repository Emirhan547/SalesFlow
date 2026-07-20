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
            "bg-green-100 text-green-700",
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
            "bg-purple-100 text-purple-700",
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

      <div className="rounded-xl border border-slate-200 bg-white py-20 text-center text-slate-400 shadow-sm">

        No activity logs found.

      </div>

    );

  }

  return (

    <div className="overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">

      <table className="w-full">

        <thead className="bg-slate-50">

          <tr className="text-left text-xs font-semibold uppercase tracking-wide text-slate-600 border-b border-slate-200">

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
                className="border-b border-slate-100 transition-all duration-200 hover:bg-blue-50/50"
              >

                <td className="px-6 py-5">

                  <span
                    className={`inline-flex items-center gap-2 rounded-md px-2.5 py-1 text-xs font-semibold ${action.className}`}
                  >

                    <Icon size={12} />

                    {action.text}

                  </span>

                </td>

                <td className="px-6 py-5">

                  <span className="rounded-md bg-slate-100 px-2.5 py-1 text-xs font-semibold text-slate-700">

                    {activity.entityName}

                  </span>

                </td>

                <td className="max-w-md px-6 py-5">

                  <p className="line-clamp-2 text-sm text-slate-700">

                    {activity.description}

                  </p>

                </td>

                <td className="px-6 py-5">

                  <div className="flex items-center gap-2">

                    <div className="flex h-8 w-8 items-center justify-center rounded-full bg-blue-100 text-xs font-semibold text-blue-700">

                      {activity.userName
                        ? activity.userName[0]
                        : "-"}

                    </div>

                    <span className="text-sm font-medium text-slate-700">

                      {activity.userName ?? "-"}

                    </span>

                  </div>

                </td>

                <td className="px-6 py-5 text-sm">

                  <div className="font-medium text-slate-900">

                    {new Date(
                      activity.createdDate
                    ).toLocaleDateString()}

                  </div>

                  <div className="text-xs text-slate-500">

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
                      className="rounded-lg p-2 text-slate-500 transition-all hover:bg-blue-100 hover:text-blue-600"
                      title="View"
                    >

                      <Eye size={16} />

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