import {
  ArrowRight,
  CalendarPlus,
  CircleDollarSign,
  UserPlus,
  Users,
} from "lucide-react";

import { useNavigate } from "react-router-dom";

import Card from "@/components/ui/Card";

function QuickActions() {

  const navigate = useNavigate();

  const actions = [
    {
      title: "New Customer",
      description: "Create customer",
      icon: Users,
      color: "bg-blue-600",
      path: "/customers/create",
    },
    {
      title: "New Lead",
      description: "Add lead",
      icon: UserPlus,
      color: "bg-green-600",
      path: "/leads/create",
    },
    {
      title: "New Deal",
      description: "Create deal",
      icon: CircleDollarSign,
      color: "bg-violet-600",
      path: "/deals/create",
    },
    {
      title: "New Meeting",
      description: "Schedule meeting",
      icon: CalendarPlus,
      color: "bg-orange-500",
      path: "/meetings/create",
    },
  ];

  return (
    <Card
      title="Quick Actions"
      subtitle="Frequently used shortcuts"
    >

      <div className="grid gap-5 md:grid-cols-2 xl:grid-cols-4">

        {actions.map((action) => {

          const Icon = action.icon;

          return (

            <button
              key={action.title}
              onClick={() => navigate(action.path)}
              className="group rounded-2xl border border-slate-200 bg-slate-50 p-5 text-left transition-all duration-300 hover:-translate-y-1 hover:border-blue-300 hover:bg-white hover:shadow-lg"
            >

              <div
                className={`mb-5 flex h-14 w-14 items-center justify-center rounded-2xl ${action.color} text-white shadow`}
              >

                <Icon size={28} />

              </div>

              <h3 className="font-semibold text-slate-900">

                {action.title}

              </h3>

              <p className="mt-2 text-sm text-slate-500">

                {action.description}

              </p>

              <div className="mt-6 flex items-center gap-2 text-sm font-semibold text-blue-600 opacity-0 transition group-hover:opacity-100">

                Open

                <ArrowRight size={16} />

              </div>

            </button>

          );

        })}

      </div>

    </Card>
  );
}

export default QuickActions;