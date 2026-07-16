import {
  useCallback,
  useEffect,
  useState,
} from "react";

import type { PagedResult } from "@/types/PagedResult";

import { getTasks } from "../services/taskService";

import type { Task } from "../types/Task";
import type { TaskFilterRequest } from "../types/TaskFilterRequest";

export function useTasks(
  filter: TaskFilterRequest
) {

  const [data, setData] =
    useState<PagedResult<Task>>();

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
          await getTasks(filter);

        setData(response);

      }
      catch {

        setError(
          "Tasks could not be loaded."
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