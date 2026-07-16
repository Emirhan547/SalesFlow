import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getDealById } from "../services/dealService";

import type { Deal } from "../types/Deal";

export function useDeal(
  id: number
) {
  const [deal, setDeal] =
    useState<Deal>();

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
          await getDealById(id);

        setDeal(response);

      }
      catch {

        setError(
          "Deal could not be loaded."
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

    deal,

    loading,

    error,

    reload: load,

  };
}