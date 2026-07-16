import api from "@/api/axios";

import type { ApiResponse } from "@/types/ApiResponse";
import type { PagedResult } from "@/types/PagedResult";

import type { Task } from "../types/Task";
import type { TaskFilterRequest } from "../types/TaskFilterRequest";
import type { CreateTaskRequest } from "../types/CreateTaskRequest";
import type { UpdateTaskRequest } from "../types/UpdateTaskRequest";

export async function getTasks(
  request: TaskFilterRequest
) {
  const response =
    await api.get<ApiResponse<PagedResult<Task>>>(
      "/TaskItems",
      {
        params: request,
      }
    );

  return response.data.data;
}

export async function getTaskById(
  id: number
) {
  const response =
    await api.get<ApiResponse<Task>>(
      `/TaskItems/${id}`
    );

  return response.data.data;
}

export async function createTask(
  request: CreateTaskRequest
) {
  const response =
    await api.post<ApiResponse<null>>(
      "/TaskItems",
      request
    );

  return response.data;
}

export async function updateTask(
  request: UpdateTaskRequest
) {
  const response =
    await api.put<ApiResponse<null>>(
      "/TaskItems",
      request
    );

  return response.data;
}

export async function deleteTask(
  id: number
) {
  const response =
    await api.delete<ApiResponse<null>>(
      `/TaskItems/${id}`
    );

  return response.data;
}