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

export default api;