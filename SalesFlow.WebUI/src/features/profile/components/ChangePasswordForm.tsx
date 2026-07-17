import {
  useState,
} from "react";

import {
  Eye,
  EyeOff,
  Lock,
} from "lucide-react";

import { toast } from "sonner";

import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";

import {
  changePassword,
} from "../services/profileService";

import type {
  ChangePasswordRequest,
} from "../types/ChangePasswordRequest";

function ChangePasswordForm() {

  const [loading, setLoading] =
    useState(false);

  const [
    showPassword,
    setShowPassword,
  ] = useState(false);

  const [form, setForm] =
    useState<ChangePasswordRequest>({
      currentPassword: "",
      newPassword: "",
      confirmNewPassword: "",
    });

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
        await changePassword(form);

      if (!response.isSuccess) {

        toast.error(response.message);

        return;

      }

      toast.success(
        response.message
      );

      setForm({
        currentPassword: "",
        newPassword: "",
        confirmNewPassword: "",
      });

    }
    catch {

      toast.error(
        "Password could not be changed."
      );

    }
    finally {

      setLoading(false);

    }

  }

  return (
    <form
      onSubmit={handleSubmit}
      className="space-y-5"
    >

      <div>

        <Label>
          Current Password
        </Label>

        <div className="relative mt-2">

          <Lock
            size={18}
            className="absolute left-4 top-3.5 text-slate-400"
          />

          <Input
            name="currentPassword"
            type={
              showPassword
                ? "text"
                : "password"
            }
            value={form.currentPassword}
            onChange={handleChange}
            className="pl-11 pr-11"
          />

          <button
            type="button"
            onClick={() =>
              setShowPassword(
                !showPassword
              )
            }
            className="absolute right-4 top-3.5 text-slate-400"
          >
            {showPassword
              ? <EyeOff size={18} />
              : <Eye size={18} />}
          </button>

        </div>

      </div>

      <div>

        <Label>
          New Password
        </Label>

        <Input
          name="newPassword"
          type="password"
          value={form.newPassword}
          onChange={handleChange}
          className="mt-2"
        />

      </div>

      <div>

        <Label>
          Confirm New Password
        </Label>

        <Input
          name="confirmNewPassword"
          type="password"
          value={form.confirmNewPassword}
          onChange={handleChange}
          className="mt-2"
        />

      </div>

      <Button
        type="submit"
        disabled={loading}
        className="w-full bg-slate-900 hover:bg-slate-800"
      >
        {loading
          ? "Changing Password..."
          : "Change Password"}
      </Button>

    </form>
  );
}

export default ChangePasswordForm;