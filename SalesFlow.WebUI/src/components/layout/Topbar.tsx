import {
  Bell,
  Moon,
  Search,
} from "lucide-react";

import { Input } from "@/components/ui/input";

function Topbar() {
  return (
    <header className="sticky top-0 z-50 flex h-20 items-center justify-between border-b border-slate-200 bg-white/80 px-8 backdrop-blur-xl">

      <div className="relative w-[380px]">

        <Search
          size={18}
          className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400"
        />

        <Input
          className="rounded-xl border-slate-200 bg-slate-50 pl-11"
          placeholder="Search customers, deals..."
        />

      </div>

      <div className="flex items-center gap-5">

        <button className="flex h-11 w-11 items-center justify-center rounded-xl border border-slate-200 transition hover:bg-slate-100">

          <Moon size={18} />

        </button>

        <button className="relative flex h-11 w-11 items-center justify-center rounded-xl border border-slate-200 transition hover:bg-slate-100">

          <Bell size={18} />

          <span className="absolute right-2 top-2 h-2.5 w-2.5 rounded-full bg-red-500" />

        </button>

        <div className="flex items-center gap-3">

          <div className="flex h-12 w-12 items-center justify-center rounded-full bg-blue-600 text-lg font-bold text-white">

            E

          </div>

          <div>

            <h4 className="font-semibold text-slate-800">
              Emirhan
            </h4>

            <div className="flex items-center gap-2 text-sm text-slate-500">

              <div className="h-2 w-2 rounded-full bg-green-500" />

              Administrator

            </div>

          </div>

        </div>

      </div>

    </header>
  );
}

export default Topbar;