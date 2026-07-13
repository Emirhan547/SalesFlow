import { NavLink } from "react-router-dom";

import {
  Briefcase,
  CalendarDays,
  CheckSquare,
  LayoutDashboard,
  Settings,
  UserPlus,
  Users,
} from "lucide-react";

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
];

function Sidebar() {
  return (
    <aside className="flex w-72 flex-col border-r border-slate-800 bg-slate-900">

      <div className="border-b border-slate-800 px-8 py-8">

        <div className="flex items-center gap-4">

          <div className="flex h-14 w-14 items-center justify-center rounded-2xl bg-blue-600 shadow-lg">

            <span className="text-2xl font-bold text-white">
              S
            </span>

          </div>

          <div>

            <h1 className="text-2xl font-bold text-white">
              SalesFlow
            </h1>

            <p className="text-sm text-slate-400">
              CRM Platform
            </p>

          </div>

        </div>

      </div>

     <nav className="flex-1 space-y-2 px-5 py-6">
  {menus.map((menu) => {
    const Icon = menu.icon;

    return (
      <NavLink
        key={menu.path}
        to={menu.path}
        className={({ isActive }) =>
          `group flex items-center gap-4 rounded-xl px-5 py-3 transition-all duration-300 ${
            isActive
              ? "border-l-4 border-blue-500 bg-blue-500/15"
              : "hover:bg-slate-800"
          }`
        }
      >
        {({ isActive }) => (
          <>
            <Icon
              size={20}
              className={
                isActive
                  ? "text-blue-400 transition"
                  : "text-slate-400 transition group-hover:text-white"
              }
            />

            <span
              className={
                isActive
                  ? "font-medium text-white"
                  : "font-medium text-slate-300 group-hover:text-white"
              }
            >
              {menu.title}
            </span>
          </>
        )}
      </NavLink>
    );
  })}
</nav>

      <div className="border-t border-slate-800 p-5">

        <button className="flex w-full items-center gap-3 rounded-xl px-4 py-3 text-slate-400 transition hover:bg-slate-800 hover:text-white">

          <Settings size={20} />

          Settings

        </button>

        <div className="mt-6 flex items-center gap-3 rounded-xl bg-slate-800 p-4">

          <div className="flex h-12 w-12 items-center justify-center rounded-full bg-blue-600 text-lg font-bold text-white">

            E

          </div>

          <div>

            <h3 className="font-semibold text-white">
              Emirhan
            </h3>

            <div className="flex items-center gap-2 text-sm text-slate-400">

              <div className="h-2 w-2 rounded-full bg-green-500" />

              Online

            </div>

          </div>

        </div>

      </div>

    </aside>
  );
}

export default Sidebar;