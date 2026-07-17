import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { getProfile } from "../services/profileService";

import type { Profile } from "../types/Profile";

export function useProfile() {

  const [profile, setProfile] =
    useState<Profile>();

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
          await getProfile();

        setProfile(response);

      }
      catch {

        setError(
          "Profile could not be loaded."
        );

      }
      finally {

        setLoading(false);

      }

    }, []);

  useEffect(() => {

    load();

  }, [load]);

  return {
    profile,
    loading,
    error,
    reload: load,
  };

}