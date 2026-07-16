import { toast } from "sonner";

import { useState } from "react";

import { deleteDeal } from "../services/dealService";

export function useDeleteDeal() {

  const [loading, setLoading] =
    useState(false);

  async function remove(
    id: number
  ) {
    try {

      setLoading(true);

      const response =
        await deleteDeal(id);

      if (!response.isSuccess) {

        toast.error(
          response.message
        );

        return false;

      }

      toast.success(
        response.message
      );

      return true;

    }
    catch {

      toast.error(
        "Delete failed."
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