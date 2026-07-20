import {
  Bell,
  Moon,
  Search,
} from "lucide-react";

import { NavLink } from "react-router-dom";

import { Input } from "@/components/ui/input";

import { useProfile } from "@/features/profile/hooks/useProfile";

function Topbar() {

  const {
    profile,
    loading,
  } = useProfile();

  const initials = profile
    ? `${profile.firstName?.[0] ?? ""}${profile.lastName?.[0] ?? ""}`
        .toUpperCase()
    : "";

  return (
    <header className="sticky top-0 z-50 flex h-20 items-center justify-between border-b border-slate-200 bg-white/95 px-8 backdrop-blur-sm">

      <div className="relative w-96">

        <Search
          size={18}
          className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400"
        />

        <Input
          className="rounded-lg border-slate-200 bg-slate-50 pl-11 text-sm placeholder:text-slate-400 focus:bg-white focus:ring-1 focus:ring-blue-500/20"
          placeholder="Search customers, deals..."
        />

      </div>

      <div className="flex items-center gap-3">

        <button className="flex h-10 w-10 items-center justify-center rounded-lg border border-slate-200 text-slate-600 transition-all hover:bg-slate-50 hover:text-slate-900">

          <Moon size={18} />

        </button>

        <button className="relative flex h-10 w-10 items-center justify-center rounded-lg border border-slate-200 text-slate-600 transition-all hover:bg-slate-50 hover:text-slate-900">

          <Bell size={18} />

          <span className="absolute right-2 top-2 h-2 w-2 rounded-full bg-red-500" />

        </button>

        <div className="w-px h-6 bg-slate-200" />

        <NavLink
          to="/profile"
          className="flex items-center gap-3 rounded-lg p-1.5 transition-all hover:bg-slate-50"
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

            <div className="flex h-10 w-10 items-center justify-center rounded-full bg-blue-600 text-sm font-semibold text-white">

              {initials || "U"}

            </div>

          )}

          <div className="hidden min-w-0 sm:block">

            {loading ? (

              <>
                <div className="h-3.5 w-20 animate-pulse rounded bg-slate-200" />

                <div className="mt-1.5 h-3 w-16 animate-pulse rounded bg-slate-200" />
              </>

            ) : (

              <>
                <h4 className="text-sm font-semibold text-slate-900">

                  {profile
                    ? `${profile.firstName} ${profile.lastName}`
                    : "User"}

                </h4>

                <div className="flex items-center gap-1.5 text-xs text-slate-500">

                  <div className="h-1.5 w-1.5 rounded-full bg-green-500" />

                  Administrator

                </div>
              </>

            )}

          </div>

        </NavLink>

      </div>

    </header>
  );
}

export default Topbar;