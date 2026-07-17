
import type { ApiResponse } from "@/types/ApiResponse";
import type { PagedResult } from "@/types/PagedResult";

import type { Notification } from "../types/Notification";
import type { NotificationFilterRequest } from "../types/NotificationFilterRequest";
import api from "@/api/axios";

export async function getNotifications(
  filter: NotificationFilterRequest
) {
  const response =
    await api.get<
      ApiResponse<PagedResult<Notification>>
    >(
      "/notifications",
      {
        params: filter,
      }
    );

  return response.data.data;
}

export async function getNotificationById(
  id: number
) {
  const response =
    await api.get<
      ApiResponse<Notification>
    >(
      `/notifications/${id}`
    );

  return response.data.data;
}

export async function getUnreadNotificationCount() {
  const response =
    await api.get<
      ApiResponse<number>
    >(
      "/notifications/unread-count"
    );

  return response.data.data;
}

export async function markNotificationAsRead(
  id: number
) {
  const response =
    await api.patch<
      ApiResponse<null>
    >(
      `/notifications/${id}/read`
    );

  return response.data;
}

export async function markAllNotificationsAsRead() {
  const response =
    await api.patch<
      ApiResponse<null>
    >(
      "/notifications/read-all"
    );

  return response.data;
}