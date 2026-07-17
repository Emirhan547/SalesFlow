import {
  ShieldCheck,
  UserRound,
} from "lucide-react";

import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";
import Card from "@/components/ui/Card";

import ProfileForm from "../components/ProfileForm";
import ChangePasswordForm from "../components/ChangePasswordForm";

import {
  useProfile,
} from "../hooks/useProfile";

function ProfilePage() {

  const {
    profile,
    loading,
    error,
    reload,
  } = useProfile();

  if (loading)
    return <LoadingState />;

  if (error)
    return (
      <div className="rounded-2xl border border-red-200 bg-red-50 p-6 text-red-600">
        {error}
      </div>
    );

  if (!profile)
    return (
      <div className="rounded-2xl bg-white p-8 text-center shadow-sm">
        Profile not found.
      </div>
    );

  const initials =
    `${profile.firstName[0] ?? ""}${profile.lastName[0] ?? ""}`
      .toUpperCase();

  return (
    <div className="space-y-8">

      <PageHeader
        title="My Profile"
        description="Manage your personal information and account security."
      />

      {/* Profile Header */}

      <div className="overflow-hidden rounded-3xl border border-slate-200 bg-white shadow-sm">

        <div className="bg-gradient-to-r from-slate-900 via-blue-900 to-blue-600 px-8 py-10">

          <div className="flex flex-col gap-6 md:flex-row md:items-center">

            <div className="flex h-28 w-28 shrink-0 items-center justify-center overflow-hidden rounded-3xl border-4 border-white bg-blue-600 text-3xl font-bold text-white shadow-lg">

              {profile.profileImageUrl ? (

                <img
                  src={profile.profileImageUrl}
                  alt={profile.userName}
                  className="h-full w-full object-cover"
                />

              ) : (

                initials

              )}

            </div>

            <div>

              <h2 className="text-3xl font-bold text-white">
                {profile.firstName}{" "}
                {profile.lastName}
              </h2>

              <p className="mt-2 font-medium text-blue-100">
                @{profile.userName}
              </p>

              <p className="mt-1 text-sm text-blue-200">
                {profile.email}
              </p>

            </div>

          </div>

        </div>

      </div>

      {/* Profile Content */}

      <div className="grid gap-8 xl:grid-cols-3">

        <div className="xl:col-span-2">

          <Card title="Personal Information">

            <div className="mb-6 flex items-center gap-3 text-slate-500">

              <div className="rounded-xl bg-blue-50 p-2 text-blue-600">
                <UserRound size={20} />
              </div>

              <p className="text-sm">
                Update your personal information and contact details.
              </p>

            </div>

            <ProfileForm
              profile={profile}
              onUpdated={reload}
            />

          </Card>

        </div>

        <div>

          <Card title="Account Security">

            <div className="mb-6 flex items-center gap-3 text-slate-500">

              <div className="rounded-xl bg-emerald-50 p-2 text-emerald-600">
                <ShieldCheck size={20} />
              </div>

              <p className="text-sm">
                Keep your account secure by using a strong password.
              </p>

            </div>

            <ChangePasswordForm />

          </Card>

        </div>

      </div>

    </div>
  );
}

export default ProfilePage;