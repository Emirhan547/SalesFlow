import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getAttachments } from "../services/attachmentService";

import type { PagedResult } from "@/types/PagedResult";
import type { Attachment } from "../types/Attachment";
import type { AttachmentFilterRequest } from "../types/AttachmentFilterRequest";

export function useAttachments(
  filter: AttachmentFilterRequest
) {
  const [data, setData] =
    useState<PagedResult<Attachment>>();

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
          await getAttachments(filter);

        setData(response);

      }
      catch {

        setError(
          "Attachments could not be loaded."
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