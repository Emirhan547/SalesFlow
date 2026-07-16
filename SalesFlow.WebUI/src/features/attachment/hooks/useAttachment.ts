import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getAttachmentById } from "../services/attachmentService";

import type { Attachment } from "../types/Attachment";

export function useAttachment(
  id: number
) {
  const [attachment, setAttachment] =
    useState<Attachment>();

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
          await getAttachmentById(id);

        setAttachment(response);

      }
      catch {

        setError(
          "Attachment could not be loaded."
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

    attachment,

    loading,

    error,

    reload: load,

  };
}