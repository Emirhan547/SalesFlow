import {
  CalendarDays,
  Download,
  RotateCw,
} from "lucide-react";

type Props = {
  onRefresh: () => void;
};

function DashboardHeader({ onRefresh }: Props) {
  const today = new Date().toLocaleDateString("tr-TR", {
    day: "numeric",
    month: "long",
    year: "numeric",
  });

  return (
    <div className="flex flex-col gap-6 xl:flex-row xl:items-center xl:justify-between">

      <div>

        <div className="inline-flex rounded-full bg-blue-100 px-3 py-1 text-sm font-semibold text-blue-700">
          CRM Dashboard
        </div>

        <h1 className="mt-4 text-5xl font-bold tracking-tight text-slate-900">
          Welcome back 👋
        </h1>

        <p className="mt-3 max-w-2xl text-lg text-slate-500">
          Monitor customers, deals, meetings and your team's daily performance from one place.
        </p>

      </div>

      <div className="flex flex-wrap items-center gap-4">

        <div className="flex items-center gap-3 rounded-2xl border border-slate-200 bg-white px-5 py-3 shadow-sm">

          <CalendarDays
            size={20}
            className="text-blue-600"
          />

          <div>

            <p className="text-xs text-slate-500">
              Today
            </p>

            <p className="font-semibold">
              {today}
            </p>

          </div>

        </div>

        <button
          className="flex items-center gap-2 rounded-2xl border border-slate-200 bg-white px-5 py-3 font-semibold transition hover:bg-slate-100"
        >

          <Download size={18} />

          Export

        </button>

        <button
          onClick={onRefresh}
          className="flex items-center gap-2 rounded-2xl bg-blue-600 px-6 py-3 font-semibold text-white shadow-lg transition hover:bg-blue-700 hover:shadow-xl"
        >

          <RotateCw size={18} />

          Refresh

        </button>

      </div>

    </div>
  );
}

export default DashboardHeader;