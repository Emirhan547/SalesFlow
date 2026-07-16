import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getNotes } from "../services/noteService";

import type { PagedResult } from "@/types/PagedResult";
import type { Note } from "../types/Note";
import type { NoteFilterRequest } from "../types/NoteFilterRequest";

export function useNotes(
  filter: NoteFilterRequest
) {
  const [data, setData] =
    useState<PagedResult<Note>>();

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
          await getNotes(filter);

        setData(response);

      }
      catch {

        setError(
          "Notes could not be loaded."
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