import { Navigate, Route, Routes } from "react-router-dom";

import LoginPage from "@/pages/LoginPage";

import AdminLayout from "@/layouts/AdminLayout";

import DashboardPage from "@/features/dashboard/DashboardPage";

import ProtectedRoute from "@/components/auth/ProtectedRoute";

import CustomerListPage from "@/features/customer/pages/CustomerListPage";
import CustomerCreatePage from "@/features/customer/pages/CustomerCreatePage";
import CustomerUpdatePage from "@/features/customer/pages/CustomerUpdatePage";
import CustomerDetailPage from "@/features/customer/pages/CustomerDetailPage";

import LeadListPage from "@/features/lead/pages/LeadListPage";
import LeadCreatePage from "@/features/lead/pages/LeadCreatePage";
import LeadUpdatePage from "@/features/lead/pages/LeadUpdatePage";
import LeadDetailPage from "@/features/lead/pages/LeadDetailPage";

import DealListPage from "@/features/deal/pages/DealListPage";
import DealCreatePage from "@/features/deal/pages/DealCreatePage";
import DealUpdatePage from "@/features/deal/pages/DealUpdatePage";
import DealDetailPage from "@/features/deal/pages/DealDetailPage";

import MeetingListPage from "@/features/meeting/pages/MeetingListPage";
import MeetingCreatePage from "@/features/meeting/pages/MeetingCreatePage";
import MeetingUpdatePage from "@/features/meeting/pages/MeetingUpdatePage";
import MeetingDetailPage from "@/features/meeting/pages/MeetingDetailPage";

import TaskListPage from "@/features/task/pages/TaskListPage";
import TaskCreatePage from "@/features/task/pages/TaskCreatePage";
import TaskUpdatePage from "@/features/task/pages/TaskUpdatePage";
import TaskDetailPage from "@/features/task/pages/TaskDetailPage";

import NoteListPage from "@/features/note/pages/NoteListPage";
import NoteCreatePage from "@/features/note/pages/NoteCreatePage";
import NoteUpdatePage from "@/features/note/pages/NoteUpdatePage";
import NoteDetailPage from "@/features/note/pages/NoteDetailPage";

import TagListPage from "@/features/tag/pages/TagListPage";
import TagCreatePage from "@/features/tag/pages/TagCreatePage";
import TagUpdatePage from "@/features/tag/pages/TagUpdatePage";
import TagDetailPage from "@/features/tag/pages/TagDetailPage";

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

          {/* Customers */}

          <Route
            path="/customers"
            element={<CustomerListPage />}
          />

          <Route
            path="/customers/create"
            element={<CustomerCreatePage />}
          />

          <Route
            path="/customers/edit/:id"
            element={<CustomerUpdatePage />}
          />

          <Route
            path="/customers/:id"
            element={<CustomerDetailPage />}
          />

          {/* Leads */}

          <Route
            path="/leads"
            element={<LeadListPage />}
          />

          <Route
            path="/leads/create"
            element={<LeadCreatePage />}
          />

          <Route
            path="/leads/edit/:id"
            element={<LeadUpdatePage />}
          />

          <Route
            path="/leads/:id"
            element={<LeadDetailPage />}
          />

          {/* Deals */}

          <Route
            path="/deals"
            element={<DealListPage />}
          />

          <Route
            path="/deals/create"
            element={<DealCreatePage />}
          />

          <Route
            path="/deals/edit/:id"
            element={<DealUpdatePage />}
          />

          <Route
            path="/deals/:id"
            element={<DealDetailPage />}
          />

          {/* Meetings */}

          <Route
            path="/meetings"
            element={<MeetingListPage />}
          />

          <Route
            path="/meetings/create"
            element={<MeetingCreatePage />}
          />

          <Route
            path="/meetings/edit/:id"
            element={<MeetingUpdatePage />}
          />

          <Route
            path="/meetings/:id"
            element={<MeetingDetailPage />}
          />

          {/* Tasks */}

          <Route
            path="/tasks"
            element={<TaskListPage />}
          />

          <Route
            path="/tasks/create"
            element={<TaskCreatePage />}
          />

          <Route
            path="/tasks/edit/:id"
            element={<TaskUpdatePage />}
          />

          <Route
            path="/tasks/:id"
            element={<TaskDetailPage />}
          />

          {/* Notes */}

          <Route
            path="/notes"
            element={<NoteListPage />}
          />

          <Route
            path="/notes/create"
            element={<NoteCreatePage />}
          />

          <Route
            path="/notes/edit/:id"
            element={<NoteUpdatePage />}
          />

          <Route
            path="/notes/:id"
            element={<NoteDetailPage />}
          />

          {/* Tags */}

          <Route
            path="/tags"
            element={<TagListPage />}
          />

          <Route
            path="/tags/create"
            element={<TagCreatePage />}
          />

          <Route
            path="/tags/edit/:id"
            element={<TagUpdatePage />}
          />

          <Route
            path="/tags/:id"
            element={<TagDetailPage />}
          />

          <Route
            path="*"
            element={
              <Navigate
                to="/dashboard"
                replace
              />
            }
          />

        </Route>

      </Route>

    </Routes>
  );
}

export default AppRouter;