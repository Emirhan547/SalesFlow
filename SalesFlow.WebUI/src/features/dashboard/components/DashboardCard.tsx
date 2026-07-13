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
    <div className="group relative overflow-hidden rounded-3xl border border-slate-200 bg-white p-7 shadow-sm transition-all duration-300 hover:-translate-y-2 hover:shadow-2xl">

      <div
        className={`absolute inset-x-0 top-0 h-1 ${color}`}
      />

      <div className="flex items-start justify-between">

        <div>

          <p className="text-xs font-semibold uppercase tracking-[0.25em] text-slate-500">
            {title}
          </p>

          <h2 className="mt-5 text-5xl font-bold tracking-tight text-slate-900">
            {value}
          </h2>

          <div className="mt-5 inline-flex rounded-full bg-green-100 px-3 py-1 text-xs font-semibold text-green-700">

            Active

          </div>

        </div>

        <div
          className={`flex h-16 w-16 items-center justify-center rounded-2xl ${color} text-white shadow-lg transition duration-300 group-hover:rotate-6 group-hover:scale-110`}
        >

          <Icon size={30} />

        </div>

      </div>

    </div>
  );
}

export default DashboardCard;