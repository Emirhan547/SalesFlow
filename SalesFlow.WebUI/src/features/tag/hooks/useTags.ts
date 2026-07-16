import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getTags } from "../services/tagService";

import type { PagedResult } from "@/types/PagedResult";
import type { Tag } from "../types/Tag";
import type { TagFilterRequest } from "../types/TagFilterRequest";

export function useTags(
  filter: TagFilterRequest
) {
  const [data, setData] =
    useState<PagedResult<Tag>>();

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
          await getTags(filter);

        setData(response);

      }
      catch {

        setError(
          "Tags could not be loaded."
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