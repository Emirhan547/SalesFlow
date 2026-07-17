import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getActivityLogById } from "../services/activityLogService";

import type { ActivityLog } from "../types/ActivityLog";

export function useActivityLog(
  id: number
) {
  const [activityLog, setActivityLog] =
    useState<ActivityLog>();

  const [loading, setLoading] =
    useState(true);

  const [error, setError] =
    useState("");

  const load =
    useCallback(async () => {

      if (!id) {

        setLoading(false);

        return;

      }

      try {

        setLoading(true);

        setError("");

        const response =
          await getActivityLogById(id);

        setActivityLog(response);

      }
      catch {

        setError(
          "Activity log could not be loaded."
        );

      }
      finally {

        setLoading(false);

      }

    }, [id]);

  useEffect(() => {

    load();

  }, [load]);

  return {

    activityLog,

    loading,

    error,

    reload: load,

  };
}