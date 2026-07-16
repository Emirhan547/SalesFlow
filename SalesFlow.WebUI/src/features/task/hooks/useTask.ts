import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getTaskById } from "../services/taskService";

import type { Task } from "../types/Task";

export function useTask(id: number) {

  const [task, setTask] =
    useState<Task>();

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
          await getTaskById(id);

        setTask(response);

      }
      catch {

        setError(
          "Task could not be loaded."
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

    task,

    loading,

    error,

    reload: load,

  };
}