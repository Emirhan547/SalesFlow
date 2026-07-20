import type { LucideIcon } from "lucide-react";

type Props = {
  title: string;
  value: number;
  icon: LucideIcon;
  color: string;
};

function DashboardCard({
  title,
  value,
  icon: Icon,
  color,
}: Props) {
  return (
    <div className="group relative overflow-hidden rounded-xl border border-slate-200 bg-white p-6 shadow-sm transition-all duration-200 hover:shadow-md hover:border-slate-300">

      <div
        className={`absolute inset-x-0 top-0 h-1 ${color}`}
      />

      <div className="flex items-start justify-between">

        <div>

          <p className="text-xs font-semibold uppercase tracking-wide text-slate-500">
            {title}
          </p>

          <h2 className="mt-4 text-4xl font-bold tracking-tight text-slate-900">
            {value}
          </h2>

          <div className="mt-4 inline-flex rounded-lg bg-green-100 px-3 py-1 text-xs font-semibold text-green-700">

            Active

          </div>

        </div>

        <div
          className={`flex h-14 w-14 items-center justify-center rounded-lg ${color} text-white shadow-md transition duration-200 group-hover:scale-105`}
        >

          <Icon size={24} />

        </div>

      </div>

    </div>
  );
}

export default DashboardCard;