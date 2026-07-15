import { useState } from "react";

import { toast } from "sonner";

import { deleteCustomer } from "../services/customerService";

export function useDeleteCustomer() {

  const [loading, setLoading] =
    useState(false);

  async function remove(id: number) {

    try {

      setLoading(true);

      const response =
        await deleteCustomer(id);

      if (!response.isSuccess) {

        toast.error(response.message);

        return false;

      }

      toast.success(response.message);

      return true;

    }
    catch {

      toast.error(
        "Customer could not be deleted."
      );

      return false;

    }
    finally {

      setLoading(false);

    }

  }

  return {

    loading,

    remove,

  };

}