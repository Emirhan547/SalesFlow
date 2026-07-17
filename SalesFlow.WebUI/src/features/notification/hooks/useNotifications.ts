import {
  useCallback,
  useEffect,
  useState,
} from "react";

import {
  getNotifications,
} from "../services/notificationService";

import type { PagedResult } from "@/types/PagedResult";
import type { Notification } from "../types/Notification";
import type { NotificationFilterRequest } from "../types/NotificationFilterRequest";

export function useNotifications(
  filter: NotificationFilterRequest
) {
  const [data, setData] =
    useState<PagedResult<Notification>>();

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
          await getNotifications(filter);

        setData(response);

      }
      catch {

        setError(
          "Notifications could not be loaded."
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