import api from "@/api/axios";

import type { ApiResponse } from "@/types/ApiResponse";
import type { PagedResult } from "@/types/PagedResult";

import type { Meeting } from "../types/Meeting";
import type { MeetingFilterRequest } from "../types/MeetingFilterRequest";
import type { CreateMeetingRequest } from "../types/CreateMeetingRequest";
import type { UpdateMeetingRequest } from "../types/UpdateMeetingRequest";

export async function getMeetings(
  request: MeetingFilterRequest
) {
  const response =
    await api.get<ApiResponse<PagedResult<Meeting>>>(
      "/Meetings",
      {
        params: request,
      }
    );

  return response.data.data;
}

export async function getMeetingById(
  id: number
) {
  const response =
    await api.get<ApiResponse<Meeting>>(
      `/Meetings/${id}`
    );

  return response.data.data;
}

export async function createMeeting(
  request: CreateMeetingRequest
) {
  const response =
    await api.post<ApiResponse<null>>(
      "/Meetings",
      request
    );

  return response.data;
}

export async function updateMeeting(
  request: UpdateMeetingRequest
) {
  const response =
    await api.put<ApiResponse<null>>(
      "/Meetings",
      request
    );

  return response.data;
}

export async function deleteMeeting(
  id: number
) {
  const response =
    await api.delete<ApiResponse<null>>(
      `/Meetings/${id}`
    );

  return response.data;
}