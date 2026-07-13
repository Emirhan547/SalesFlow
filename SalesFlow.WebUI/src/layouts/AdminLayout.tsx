import { Outlet } from "react-router-dom";

import Sidebar from "@/components/layout/Sidebar";
import Topbar from "@/components/layout/Topbar";

function AdminLayout() {
  return (
    <div className="flex h-screen overflow-hidden bg-slate-100">

      <Sidebar />

      <div className="flex flex-1 flex-col overflow-hidden">

        <Topbar />

        <main className="flex-1 overflow-y-auto bg-gradient-to-br from-slate-100 via-slate-50 to-slate-200">

          <div className="mx-auto w-full max-w-[1700px] p-8">

            <Outlet />

          </div>

        </main>

      </div>

    </div>
  );
}

export default AdminLayout;