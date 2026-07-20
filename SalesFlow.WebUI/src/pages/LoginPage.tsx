import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import signalRService from "@/services/signalRService";
import {
  Eye,
  EyeOff,
  Lock,
  Mail,
  ArrowRight,
  Users,
  Briefcase,
  CalendarDays,
} from "lucide-react";

import { login } from "@/features/auth/services/authService";
import {
  setAccessToken,
  setRefreshToken,
} from "@/features/auth/services/storageService";

import { useAuth } from "@/features/auth/hooks/useAuth";

import type { LoginRequest } from "@/features/auth/types/LoginRequest";

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import Card from "@/components/ui/Card";

function LoginPage() {
  const navigate = useNavigate();
  const auth = useAuth();

  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);

  const [form, setForm] = useState<LoginRequest>({
    email: "",
    password: "",
  });

  function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    setForm((prev) => ({
      ...prev,
      [e.target.name]: e.target.value,
    }));
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    setLoading(true);

    try {
      const response = await login(form);

      setAccessToken(response.accessToken);
      setRefreshToken(response.refreshToken);

      auth.login();
  await signalRService.start();
      navigate("/dashboard", {
        replace: true,
      });
    } catch {
      alert("Email veya şifre hatalı.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="min-h-screen overflow-hidden bg-gradient-to-br from-slate-50 via-blue-50 to-indigo-100">

      <div className="grid min-h-screen lg:grid-cols-2">

        {/* LEFT */}

        <div className="relative hidden overflow-hidden bg-slate-900 lg:flex">

          <div className="absolute -left-40 top-0 h-96 w-96 rounded-full bg-blue-600/30 blur-3xl" />

          <div className="absolute bottom-0 right-0 h-96 w-96 rounded-full bg-indigo-500/30 blur-3xl" />

          <div className="relative z-10 flex w-full flex-col justify-between p-16">

            <div>

              <div className="flex h-16 w-16 items-center justify-center rounded-xl bg-gradient-to-br from-blue-600 to-blue-700 text-3xl font-bold text-white shadow-lg">
                S
              </div>

              <h1 className="mt-10 text-5xl font-black leading-tight text-white">
                SalesFlow
              </h1>

              <p className="mt-6 max-w-lg text-lg leading-relaxed text-slate-300">
                Modern CRM platform for managing customers,
                leads, deals and meetings in one place.
              </p>

            </div>

            <div className="grid grid-cols-3 gap-6">

              <div className="rounded-xl border border-white/10 bg-white/10 p-6 backdrop-blur">
                <Users className="mb-4 text-blue-300" size={28} />
                <h3 className="text-2xl font-bold text-white">
                  12K+
                </h3>
                <p className="mt-2 text-sm text-slate-300">
                  Customers
                </p>
              </div>

              <div className="rounded-xl border border-white/10 bg-white/10 p-6 backdrop-blur">
                <Briefcase className="mb-4 text-purple-300" size={28} />
                <h3 className="text-2xl font-bold text-white">
                  950+
                </h3>
                <p className="mt-2 text-sm text-slate-300">
                  Deals
                </p>
              </div>

              <div className="rounded-xl border border-white/10 bg-white/10 p-6 backdrop-blur">
                <CalendarDays className="mb-4 text-orange-300" size={28} />
                <h3 className="text-2xl font-bold text-white">
                  480+
                </h3>
                <p className="mt-2 text-sm text-slate-300">
                  Meetings
                </p>
              </div>

            </div>

          </div>

        </div>

        {/* RIGHT */}

        <div className="flex items-center justify-center p-8">

          <Card className="w-full max-w-md border-white/50 bg-white/95 p-10 shadow-lg backdrop-blur">

            <div>

              <p className="text-sm font-semibold text-blue-600">
                Welcome Back 👋
              </p>

              <h2 className="mt-3 text-3xl font-bold text-slate-900">
                Sign In
              </h2>

              <p className="mt-2 text-sm text-slate-500">
                Sign in to continue to your dashboard.
              </p>

            </div>

            <form
              onSubmit={handleSubmit}
              className="mt-8 space-y-5"
            >

              <div>

                <Label>Email</Label>

                <div className="relative mt-2">

                  <Mail className="absolute left-4 top-3 text-slate-400" size={18} />

                  <Input
                    name="email"
                    type="email"
                    value={form.email}
                    onChange={handleChange}
                    placeholder="Enter your email"
                    className="h-12 rounded-lg pl-12"
                  />

                </div>

              </div>

              <div>

                <div className="mb-2 flex items-center justify-between">

                  <Label>Password</Label>

                  <button
                    type="button"
                    className="text-xs font-semibold text-blue-600 hover:text-blue-700 transition"
                  >
                    Forgot password?
                  </button>

                </div>

                <div className="relative">

                  <Lock className="absolute left-4 top-3 text-slate-400" size={18} />

                  <Input
                    name="password"
                    type={showPassword ? "text" : "password"}
                    value={form.password}
                    onChange={handleChange}
                    placeholder="Enter your password"
                    className="h-12 rounded-lg pl-12 pr-12"
                  />

                  <button
                    type="button"
                    onClick={() => setShowPassword(!showPassword)}
                    className="absolute right-4 top-3 text-slate-400 hover:text-slate-600 transition"
                  >
                    {showPassword ? (
                      <EyeOff size={18} />
                    ) : (
                      <Eye size={18} />
                    )}
                  </button>

                </div>

              </div>

              <Button
                disabled={loading}
                className="h-12 w-full rounded-lg bg-blue-600 text-sm font-semibold hover:bg-blue-700"
              >
                {loading ? "Signing In..." : "Sign In"}

                {!loading && (
                  <ArrowRight className="ml-2 h-4 w-4" />
                )}

              </Button>

            </form>

            <div className="mt-6 text-center text-xs text-slate-500">

              Don't have an account?{" "}

              <Link
                to="/register"
                className="font-semibold text-blue-600 hover:text-blue-700 transition"
              >
                Create Account
              </Link>

            </div>

          </Card>

        </div>

      </div>

    </div>
  );
}

export default LoginPage;