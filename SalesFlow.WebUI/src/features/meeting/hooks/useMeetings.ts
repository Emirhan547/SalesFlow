import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getMeetings } from "../services/meetingService";

import type { Meeting } from "../types/Meeting";
import type { MeetingFilterRequest } from "../types/MeetingFilterRequest";

import type { PagedResult } from "@/types/PagedResult";

export function useMeetings(
  filter: MeetingFilterRequest
) {

  const [data, setData] =
    useState<PagedResult<Meeting>>();

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
          await getMeetings(filter);

        setData(response);

      }
      catch {

        setError(
          "Meetings could not be loaded."
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