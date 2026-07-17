import {
  useCallback,
  useEffect,
  useState,
} from "react";

import {
  getUsers,
} from "../services/userService";

import type { User } from "../types/User";

export function useUsers() {

  const [users, setUsers] =
    useState<User[]>([]);

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
          await getUsers();

        setUsers(response);

      }
      catch {

        setError(
          "Users could not be loaded."
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
    users,
    loading,
    error,
    reload: load,
  };
}