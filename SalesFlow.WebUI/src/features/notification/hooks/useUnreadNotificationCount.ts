import {
  useCallback,
  useEffect,
  useState,
} from "react";

import {
  getUnreadNotificationCount,
} from "../services/notificationService";

export function useUnreadNotificationCount() {

  const [count, setCount] =
    useState(0);

  const [loading, setLoading] =
    useState(true);

  const load =
    useCallback(async () => {

      try {

        const response =
          await getUnreadNotificationCount();

        setCount(response);

      }
      catch {

        setCount(0);

      }
      finally {

        setLoading(false);

      }

    }, []);

  useEffect(() => {

    load();

  }, [load]);

  return {
    count,
    loading,
    reload: load,
  };
}