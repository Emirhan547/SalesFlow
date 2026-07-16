import { useState } from "react";

import { toast } from "sonner";

import { deleteTask } from "../services/taskService";

export function useDeleteTask() {

  const [loading, setLoading] =
    useState(false);

  async function remove(
    id: number
  ) {

    try {

      setLoading(true);

      const response =
        await deleteTask(id);

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
        "Task could not be deleted."
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