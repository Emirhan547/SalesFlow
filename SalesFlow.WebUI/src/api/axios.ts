import axios from "axios";

import { getAccessToken } from "@/features/auth/services/storageService";

const api = axios.create({
  baseURL: "https://localhost:7259/api",
  headers: {
    "Content-Type": "application/json",
  },
});

api.interceptors.request.use((config) => {
  const token = getAccessToken();

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

api.interceptors.response.use(
  (response) => response,

  (error) => {
    if (axios.isAxiosError(error)) {
      const message =
        error.response?.data?.message ??
        "An unexpected error occurred.";

      error.message = message;
    }

    return Promise.reject(error);
  }
);

export default api;