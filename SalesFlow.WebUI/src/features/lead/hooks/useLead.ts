import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getLeadById } from "../services/leadService";

import type { Lead } from "../types/Lead";

export function useLead(
  id: number
) {

  const [lead, setLead] =
    useState<Lead>();

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
          await getLeadById(id);

        setLead(response);

      }
      catch {

        setError(
          "Lead could not be loaded."
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

    lead,

    loading,

    error,

    reload: load,

  };

}