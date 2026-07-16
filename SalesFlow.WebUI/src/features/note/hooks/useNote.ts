import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getNoteById } from "../services/noteService";

import type { Note } from "../types/Note";

export function useNote(
  id: number
) {
  const [note, setNote] =
    useState<Note>();

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
          await getNoteById(id);

        setNote(response);

      }
      catch {

        setError(
          "Note could not be loaded."
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

    note,

    loading,

    error,

    reload: load,

  };
}