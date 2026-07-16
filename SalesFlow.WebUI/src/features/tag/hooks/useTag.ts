import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getTagById } from "../services/tagService";

import type { Tag } from "../types/Tag";

export function useTag(
  id: number
) {
  const [tag, setTag] =
    useState<Tag>();

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
          await getTagById(id);

        setTag(response);

      }
      catch {

        setError(
          "Tag could not be loaded."
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

    tag,

    loading,

    error,

    reload: load,

  };
}