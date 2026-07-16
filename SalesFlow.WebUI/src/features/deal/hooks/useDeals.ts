import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getDeals } from "../services/dealService";

import type { Deal } from "../types/Deal";
import type { DealFilterRequest } from "../types/DealFilterRequest";
import type { PagedResult } from "@/types/PagedResult";

export function useDeals(
  filter: DealFilterRequest
) {
  const [data, setData] =
    useState<PagedResult<Deal>>();

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
          await getDeals(filter);

        setData(response);

      }
      catch {

        setError(
          "Deals could not be loaded."
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