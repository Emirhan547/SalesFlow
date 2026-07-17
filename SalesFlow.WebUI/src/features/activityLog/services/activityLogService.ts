import api from "@/api/axios";

import type { ApiResponse } from "@/types/ApiResponse";
import type { PagedResult } from "@/types/PagedResult";

import type { ActivityLog } from "../types/ActivityLog";
import type { ActivityLogFilterRequest } from "../types/ActivityLogFilterRequest";

export async function getActivityLogs(
  request: ActivityLogFilterRequest
) {
  const response =
    await api.get<ApiResponse<PagedResult<ActivityLog>>>(
      "/ActivityLogs",
      {
        params: request,
      }
    );

  return response.data.data;
}

export async function getActivityLogById(
  id: number
) {
  const response =
    await api.get<ApiResponse<ActivityLog>>(
      `/ActivityLogs/${id}`
    );

  return response.data.data;
}