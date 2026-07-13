import api from "@/api/axios";

import type { ApiResponse } from "@/types/ApiResponse";

import type { LoginRequest } from "../types/LoginRequest";
import type { LoginResponse } from "../types/LoginResponse";
import type {
  RefreshTokenRequest,
  RefreshTokenResponse,
} from "../types/RefreshToken";

export async function login(
  request: LoginRequest
): Promise<LoginResponse> {
  const response =
    await api.post<ApiResponse<LoginResponse>>(
      "/auths/login",
      request
    );

  return response.data.data;
}

export async function refreshToken(
  request: RefreshTokenRequest
): Promise<RefreshTokenResponse> {
  const response =
    await api.post<ApiResponse<RefreshTokenResponse>>(
      "/auths/refresh-token",
      request
    );

  return response.data.data;
}