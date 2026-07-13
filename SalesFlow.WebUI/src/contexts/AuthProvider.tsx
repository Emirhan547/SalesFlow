import { useMemo, useState, type ReactNode } from "react";

import { AuthContext } from "./AuthContext";

import {
  clearTokens,
  getAccessToken,
} from "@/features/auth/services/storageService";

type Props = {
  children: ReactNode;
};

function AuthProvider({ children }: Props) {
  const [isAuthenticated, setIsAuthenticated] =
    useState(Boolean(getAccessToken()));

  function login() {
    setIsAuthenticated(true);
  }

  function logout() {
    clearTokens();
    setIsAuthenticated(false);
  }

  const value = useMemo(
    () => ({
      isAuthenticated,
      login,
      logout,
    }),
    [isAuthenticated]
  );

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

export default AuthProvider;