import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getLeads } from "../services/leadService";

import type { Lead } from "../types/Lead";
import type { LeadFilterRequest } from "../types/LeadFilterRequest";

import type { PagedResult } from "@/types/PagedResult";

export function useLeads(
  filter: LeadFilterRequest
) {

  const [data, setData] =
    useState<PagedResult<Lead>>();

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
          await getLeads(filter);

        setData(response);

      }
      catch {

        setError(
          "Leads could not be loaded."
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