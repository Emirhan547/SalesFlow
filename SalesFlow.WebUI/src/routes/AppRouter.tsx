import { Navigate, Route, Routes } from "react-router-dom";

import LoginPage from "@/pages/LoginPage";

import AdminLayout from "@/layouts/AdminLayout";

import DashboardPage from "@/features/dashboard/DashboardPage";

import ProtectedRoute from "@/components/auth/ProtectedRoute";
import CustomerCreatePage from "@/features/customer/pages/CustomerCreatePage";
import CustomerDetailPage from "@/features/customer/pages/CustomerDetailPage";
import CustomerListPage from "@/features/customer/pages/CustomerListPage";
import CustomerUpdatePage from "@/features/customer/pages/CustomerUpdatePage";

function PlaceholderPage({
  title,
  description,
}: {
  title: string;
  description: string;
}) {
  return (
    <div className="rounded-3xl border border-slate-200 bg-white p-8 shadow-sm">
      <h1 className="text-3xl font-semibold text-slate-900">
        {title}
      </h1>
      <p className="mt-2 text-slate-600">{description}</p>
    </div>
  );
}

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

          <Route path="/customers" element={<CustomerListPage />} />
          <Route path="/customers/create" element={<CustomerCreatePage />} />
          <Route path="/customers/edit/:id" element={<CustomerUpdatePage />} />
          <Route path="/customers/:id" element={<CustomerDetailPage />} />

          <Route
            path="/leads"
            element={
              <PlaceholderPage
                title="Leads"
                description="Leads management is coming soon."
              />
            }
          />

          <Route
            path="/deals"
            element={
              <PlaceholderPage
                title="Deals"
                description="Deals management is coming soon."
              />
            }
          />

          <Route
            path="/meetings"
            element={
              <PlaceholderPage
                title="Meetings"
                description="Meetings scheduling is coming soon."
              />
            }
          />

          <Route
            path="/tasks"
            element={
              <PlaceholderPage
                title="Tasks"
                description="Task tracking is coming soon."
              />
            }
          />

          <Route path="*" element={<Navigate to="/dashboard" replace />} />
        </Route>
      </Route>
    </Routes>
  );
}

export default AppRouter;