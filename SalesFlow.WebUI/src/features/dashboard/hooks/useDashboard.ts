import { useCallback, useEffect, useState } from "react";

import { getDashboard } from "../services/dashboardService";
import type { DashboardDto } from "../types/DashboardDto";

export function useDashboard() {
  const [data, setData] = useState<DashboardDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const loadDashboard = useCallback(async () => {
    try {
      setError("");

      const response = await getDashboard();

      setData(response);
    } catch {
      setError("Dashboard could not be loaded.");
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    loadDashboard();
  }, [loadDashboard]);

  return {
    data,
    loading,
    error,
    reload: loadDashboard,
  };
}