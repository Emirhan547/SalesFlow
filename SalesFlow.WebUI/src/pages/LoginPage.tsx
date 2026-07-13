import { useState } from "react";
import { useNavigate } from "react-router-dom";

import { Eye, EyeOff, Lock, Mail } from "lucide-react";

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

      navigate("/dashboard", { replace: true });
    } catch {
      alert("Email veya şifre hatalı.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-slate-100 via-white to-slate-200 p-6">
      <Card className="w-full max-w-md p-8 shadow-2xl">

        <div className="mb-8">

          <h1 className="text-4xl font-bold">
            SalesFlow
          </h1>

          <p className="text-muted-foreground mt-2">
            CRM Management System
          </p>

        </div>

        <form
          onSubmit={handleSubmit}
          className="space-y-5"
        >

          <div>

            <Label>Email</Label>

            <div className="relative mt-2">

              <Mail className="absolute left-3 top-3 h-5 w-5 text-gray-400" />

              <Input
                className="pl-10"
                name="email"
                type="email"
                value={form.email}
                onChange={handleChange}
                placeholder="Enter your email"
              />

            </div>

          </div>

          <div>

            <Label>Password</Label>

            <div className="relative mt-2">

              <Lock className="absolute left-3 top-3 h-5 w-5 text-gray-400" />

              <Input
                className="pl-10 pr-10"
                name="password"
                type={showPassword ? "text" : "password"}
                value={form.password}
                onChange={handleChange}
                placeholder="Enter your password"
              />

              <button
                type="button"
                onClick={() => setShowPassword(!showPassword)}
                className="absolute right-3 top-3"
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
            className="w-full"
            disabled={loading}
          >
            {loading ? "Signing In..." : "Sign In"}
          </Button>

        </form>

      </Card>
    </div>
  );
}

export default LoginPage;