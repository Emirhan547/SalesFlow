import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getLeadSummary } from "../services/leadService";

export function useLeadSummary(
  id: number
) {

  const [summary, setSummary] =
    useState<string>();

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
          await getLeadSummary(id);

        setSummary(response);

      }
      catch {

        setError(
          "Summary could not be loaded."
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

    summary,

    loading,

    error,

    reload: load,

  };

}