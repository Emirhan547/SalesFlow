import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getCustomerById } from "../services/customerService";

import type { Customer } from "../types/Customer";

export function useCustomer(
  id: number
) {
  const [customer, setCustomer] =
    useState<Customer>();

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
          await getCustomerById(id);

        setCustomer(response);
      }
      catch {
        setError(
          "Customer could not be loaded."
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
    customer,
    loading,
    error,
    reload: load,
  };
}
