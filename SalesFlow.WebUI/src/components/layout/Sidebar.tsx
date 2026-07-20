import { NavLink } from "react-router-dom";

import {
  Activity,
  Briefcase,
  CalendarDays,
  CheckSquare,
  FileText,
  LayoutDashboard,
  Settings,
  Tag,
  UserPlus,
  Users,
} from "lucide-react";

import { useProfile } from "@/features/profile/hooks/useProfile";

const menus = [
  {
    title: "Dashboard",
    icon: LayoutDashboard,
    path: "/dashboard",
  },
  {
    title: "Customers",
    icon: Users,
    path: "/customers",
  },
  {
    title: "Leads",
    icon: UserPlus,
    path: "/leads",
  },
  {
    title: "Deals",
    icon: Briefcase,
    path: "/deals",
  },
  {
    title: "Meetings",
    icon: CalendarDays,
    path: "/meetings",
  },
  {
    title: "Tasks",
    icon: CheckSquare,
    path: "/tasks",
  },
  {
    title: "Notes",
    icon: FileText,
    path: "/notes",
  },
  {
    title: "Tags",
    icon: Tag,
    path: "/tags",
  },
  {
    title: "Activity Logs",
    icon: Activity,
    path: "/activity-logs",
  },
];

function Sidebar() {

  const {
    profile,
    loading,
  } = useProfile();

  const initials = profile
    ? `${profile.firstName?.[0] ?? ""}${profile.lastName?.[0] ?? ""}`
        .toUpperCase()
    : "";

  return (
    <aside className="flex w-64 flex-col border-r border-slate-200 bg-white">

      <div className="border-b border-slate-200 px-6 py-6">

        <div className="flex items-center gap-3">

          <div className="flex h-12 w-12 items-center justify-center rounded-lg bg-gradient-to-br from-blue-600 to-blue-700 shadow-md">

            <span className="text-xl font-bold text-white">
              S
            </span>

          </div>

          <div>

            <h1 className="text-lg font-bold text-slate-900">
              SalesFlow
            </h1>

            <p className="text-xs text-slate-500">
              CRM Platform
            </p>

          </div>

        </div>

      </div>

      <nav className="flex-1 space-y-1 overflow-y-auto px-3 py-4">

        {menus.map((menu) => {

          const Icon = menu.icon;

          return (
            <NavLink
              key={menu.path}
              to={menu.path}
              className={({ isActive }) =>
                `group flex items-center gap-3 rounded-lg px-4 py-2.5 text-sm font-medium transition-all duration-200 ${
                  isActive
                    ? "bg-blue-50 text-blue-600"
                    : "text-slate-600 hover:bg-slate-50 hover:text-slate-900"
                }`
              }
            >

              {({ isActive }) => (
                <>

                  <Icon
                    size={18}
                    className={
                      isActive
                        ? "text-blue-600"
                        : "text-slate-400 transition group-hover:text-slate-600"
                    }
                  />

                  <span>
                    {menu.title}
                  </span>

                </>
              )}

            </NavLink>
          );

        })}

      </nav>

      <div className="border-t border-slate-200 p-3">

        <button className="flex w-full items-center gap-3 rounded-lg px-4 py-2.5 text-sm font-medium text-slate-600 transition-all hover:bg-slate-50 hover:text-slate-900">

          <Settings size={18} />

          Settings

        </button>

        <NavLink
          to="/profile"
          className={({ isActive }) =>
            `mt-3 flex items-center gap-3 rounded-lg p-3 transition-all duration-200 ${
              isActive
                ? "bg-blue-50 ring-1 ring-blue-200"
                : "hover:bg-slate-50"
            }`
          }
        >

          {loading ? (

            <div className="h-10 w-10 animate-pulse rounded-full bg-slate-200" />

          ) : profile?.profileImageUrl ? (

            <img
              src={profile.profileImageUrl}
              alt={profile.userName}
              className="h-10 w-10 rounded-full object-cover"
            />

          ) : (

            <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-blue-600 text-sm font-semibold text-white">
              {initials || "U"}
            </div>

          )}

          <div className="min-w-0 flex-1">

            {loading ? (

              <>
                <div className="h-3.5 w-20 animate-pulse rounded bg-slate-200" />

                <div className="mt-1.5 h-3 w-16 animate-pulse rounded bg-slate-200" />
              </>

            ) : (

              <>
                <h3 className="truncate text-sm font-semibold text-slate-900">
                  {profile
                    ? `${profile.firstName} ${profile.lastName}`
                    : "User"}
                </h3>

                <div className="mt-0.5 flex items-center gap-1.5 text-xs text-slate-500">

                  <div className="h-1.5 w-1.5 shrink-0 rounded-full bg-green-500" />

                  <span>
                    Online
                  </span>

                </div>
              </>

            )}

          </div>

        </NavLink>

      </div>

    </aside>
  );
}

export default Sidebar;