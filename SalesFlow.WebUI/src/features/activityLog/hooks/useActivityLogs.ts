import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getActivityLogs } from "../services/activityLogService";

import type { PagedResult } from "@/types/PagedResult";
import type { ActivityLog } from "../types/ActivityLog";
import type { ActivityLogFilterRequest } from "../types/ActivityLogFilterRequest";

export function useActivityLogs(
  filter: ActivityLogFilterRequest
) {
  const [data, setData] =
    useState<PagedResult<ActivityLog>>();

  const [loading, setLoading] =
    useState(true);

  const [error, setError] =
    useState("");

  const load =
    useCallback(async () => {

      try {

        setLoading(true);

        setError("");

        const response =
          await getActivityLogs(filter);

        setData(response);

      }
      catch {

        setError(
          "Activity logs could not be loaded."
        );

      }
      finally {

        setLoading(false);

      }

    }, [filter]);

  useEffect(() => {

    load();

  }, [load]);

  return {

    data,

    loading,

    error,

    reload: load,

  };
}