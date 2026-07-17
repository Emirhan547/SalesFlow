
import type { ApiResponse } from "@/types/ApiResponse";

import type { Profile } from "../types/Profile";
import type { UpdateProfileRequest } from "../types/UpdateProfileRequest";
import type { ChangePasswordRequest } from "../types/ChangePasswordRequest";
import api from "@/api/axios";

export async function getProfile() {

  const response =
    await api.get<ApiResponse<Profile>>(
      "/profiles"
    );

  return response.data.data;

}

export async function updateProfile(
  request: UpdateProfileRequest
) {

  const response =
    await api.put<ApiResponse<null>>(
      "/profiles",
      request
    );

  return response.data;

}

export async function changePassword(
  request: ChangePasswordRequest
) {

  const response =
    await api.put<ApiResponse<null>>(
      "/profiles/change-password",
      request
    );

  return response.data;

}