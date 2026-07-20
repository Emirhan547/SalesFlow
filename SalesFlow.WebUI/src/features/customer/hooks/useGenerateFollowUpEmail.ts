import {
  useCallback,
  useState,
} from "react";

import { generateFollowUpEmail } from "../services/customerService";

import type { GenerateFollowUpEmailRequest } from "../types/GenerateFollowUpEmailRequest";

export function useGenerateFollowUpEmail() {

  const [email, setEmail] =
    useState("");

  const [loading, setLoading] =
    useState(false);

  const [error, setError] =
    useState("");

  const generate =
    useCallback(
      async (
        customerId: number,
        request: GenerateFollowUpEmailRequest
      ) => {

        try {

          setLoading(true);

          setError("");
          setEmail("");

          const response =
            await generateFollowUpEmail(
              customerId,
              request
            );

          setEmail(response);

        }
        catch {

          setError(
            "Email could not be generated."
          );

        }
        finally {

          setLoading(false);

        }

      },
      []
    );

  const clear = () => {

    setEmail("");

    setError("");

  };

  return {

    email,

    loading,

    error,

    generate,

    clear,

  };

}