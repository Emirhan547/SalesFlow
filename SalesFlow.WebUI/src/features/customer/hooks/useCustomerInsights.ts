import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getCustomerInsights } from "../services/customerService";

export function useCustomerInsights(
  id: number,
  autoLoad = true
) {
  const [insights, setInsights] =
    useState<string>();

  const [loading, setLoading] =
  useState(false);

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
          await getCustomerInsights(id);

        setInsights(response);

      }
      catch {

        setError(
          "AI insights could not be loaded."
        );

      }
      finally {

        setLoading(false);

      }

    }, [id]);

  useEffect(() => {

  if (autoLoad)
    load();

}, [load, autoLoad]);

  return {

    insights,

    loading,

    error,

    reload: load,

  };
}