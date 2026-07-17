import api from "@/api/axios";

import type { ApiResponse } from "@/types/ApiResponse";
import type { User } from "../types/User";

export async function getUsers() {
  const response =
    await api.get<ApiResponse<User[]>>(
      "/users"
    );

  return response.data.data;
}