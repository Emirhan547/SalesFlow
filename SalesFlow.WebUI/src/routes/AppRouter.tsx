import { Route, Routes } from "react-router-dom";

import LoginPage from "@/pages/LoginPage";

import AdminLayout from "@/layouts/AdminLayout";

import DashboardPage from "@/features/dashboard/DashboardPage";

import ProtectedRoute from "@/components/auth/ProtectedRoute";

function AppRouter() {
  return (
    <Routes>
      <Route
        path="/"
        element={<LoginPage />}
      />

      <Route element={<ProtectedRoute />}>

        <Route element={<AdminLayout />}>

          <Route
            path="/dashboard"
            element={<DashboardPage />}
          />

        </Route>

      </Route>
    </Routes>
  );
}

export default AppRouter;