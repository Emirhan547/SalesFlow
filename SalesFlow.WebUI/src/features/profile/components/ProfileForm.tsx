import {
  useEffect,
  useState,
} from "react";

import {
  Mail,
  Phone,
  User,
} from "lucide-react";

import { toast } from "sonner";

import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";

import {
  updateProfile,
} from "../services/profileService";

import type { Profile } from "../types/Profile";
import type { UpdateProfileRequest } from "../types/UpdateProfileRequest";

type Props = {
  profile: Profile;
  onUpdated: () => void;
};

function ProfileForm({
  profile,
  onUpdated,
}: Props) {

  const [loading, setLoading] =
    useState(false);

  const [form, setForm] =
    useState<UpdateProfileRequest>({
      firstName: "",
      lastName: "",
      userName: "",
      phoneNumber: "",
    });

  useEffect(() => {

    setForm({
      firstName: profile.firstName,
      lastName: profile.lastName,
      userName: profile.userName,
      phoneNumber:
        profile.phoneNumber ?? "",
    });

  }, [profile]);

  function handleChange(
    event: React.ChangeEvent<HTMLInputElement>
  ) {

    const {
      name,
      value,
    } = event.target;

    setForm((previous) => ({
      ...previous,
      [name]: value,
    }));

  }

  async function handleSubmit(
    event: React.FormEvent
  ) {

    event.preventDefault();

    try {

      setLoading(true);

      const response =
        await updateProfile(form);

      if (!response.isSuccess) {

        toast.error(response.message);

        return;

      }

      toast.success(
        response.message
      );

      onUpdated();

    }
    catch {

      toast.error(
        "Profile could not be updated."
      );

    }
    finally {

      setLoading(false);

    }

  }

  return (
    <form
      onSubmit={handleSubmit}
      className="space-y-6"
    >

      <div className="grid gap-6 md:grid-cols-2">

        <div>

          <Label>
            First Name
          </Label>

          <div className="relative mt-2">

            <User
              size={18}
              className="absolute left-4 top-3.5 text-slate-400"
            />

            <Input
              name="firstName"
              value={form.firstName}
              onChange={handleChange}
              className="pl-11"
            />

          </div>

        </div>

        <div>

          <Label>
            Last Name
          </Label>

          <Input
            name="lastName"
            value={form.lastName}
            onChange={handleChange}
            className="mt-2"
          />

        </div>

        <div>

          <Label>
            Username
          </Label>

          <Input
            name="userName"
            value={form.userName}
            onChange={handleChange}
            className="mt-2"
          />

        </div>

        <div>

          <Label>
            Phone Number
          </Label>

          <div className="relative mt-2">

            <Phone
              size={18}
              className="absolute left-4 top-3.5 text-slate-400"
            />

            <Input
              name="phoneNumber"
              value={
                form.phoneNumber ?? ""
              }
              onChange={handleChange}
              className="pl-11"
            />

          </div>

        </div>

        <div className="md:col-span-2">

          <Label>
            Email
          </Label>

          <div className="relative mt-2">

            <Mail
              size={18}
              className="absolute left-4 top-3.5 text-slate-400"
            />

            <Input
              value={profile.email}
              disabled
              className="bg-slate-50 pl-11"
            />

          </div>

          <p className="mt-2 text-xs text-slate-400">
            Email address cannot be changed here.
          </p>

        </div>

      </div>

      <div className="flex justify-end">

        <Button
          disabled={loading}
          type="submit"
          className="bg-blue-600 hover:bg-blue-700"
        >
          {loading
            ? "Saving..."
            : "Save Changes"}
        </Button>

      </div>

    </form>
  );
}

export default ProfileForm;