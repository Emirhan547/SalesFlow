import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getLeadScore } from "../services/leadService";

import type { LeadScore } from "../types/LeadScore";

export function useLeadScore(
  id: number
) {

  const [score, setScore] =
    useState<LeadScore>();

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
          await getLeadScore(id);

        setScore(response);

      }
      catch {

        setError(
          "Lead score could not be loaded."
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

    score,

    loading,

    error,

    reload: load,

  };

}