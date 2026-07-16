import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getMeetingById } from "../services/meetingService";

import type { Meeting } from "../types/Meeting";

export function useMeeting(
  id: number
) {

  const [meeting, setMeeting] =
    useState<Meeting>();

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
          await getMeetingById(id);

        setMeeting(response);

      }
      catch {

        setError(
          "Meeting could not be loaded."
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

    meeting,

    loading,

    error,

    reload: load,

  };
}