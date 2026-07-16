import { useState } from "react";

import { toast } from "sonner";

import { deleteLead } from "../services/leadService";

export function useDeleteLead() {

  const [loading, setLoading] =
    useState(false);

  async function remove(
    id: number
  ) {

    try {

      setLoading(true);

      const response =
        await deleteLead(id);

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
        "Lead could not be deleted."
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