import { useState } from "react";

import { toast } from "sonner";

import { deleteMeeting } from "../services/meetingService";

export function useDeleteMeeting() {

  const [loading, setLoading] =
    useState(false);

  async function remove(
    id: number
  ) {

    try {

      setLoading(true);

      const response =
        await deleteMeeting(id);

      if (!response.isSuccess) {

        toast.error(response.message);

        return false;

      }

      toast.success(response.message);

      return true;

    }
    catch {

      toast.error(
        "Meeting could not be deleted."
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