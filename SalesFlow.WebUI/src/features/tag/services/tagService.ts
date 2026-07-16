import api from "@/api/axios";

import type { ApiResponse } from "@/types/ApiResponse";
import type { PagedResult } from "@/types/PagedResult";

import type { Tag } from "../types/Tag";
import type { TagFilterRequest } from "../types/TagFilterRequest";
import type { CreateTagRequest } from "../types/CreateTagRequest";
import type { UpdateTagRequest } from "../types/UpdateTagRequest";

export async function getTags(
  request: TagFilterRequest
) {
  const response =
    await api.get<ApiResponse<PagedResult<Tag>>>(
      "/Tags",
      {
        params: request,
      }
    );

  return response.data.data;
}

export async function getTagById(
  id: number
) {
  const response =
    await api.get<ApiResponse<Tag>>(
      `/Tags/${id}`
    );

  return response.data.data;
}

export async function createTag(
  request: CreateTagRequest
) {
  const response =
    await api.post<ApiResponse<null>>(
      "/Tags",
      request
    );

  return response.data;
}

export async function updateTag(
  request: UpdateTagRequest
) {
  const response =
    await api.put<ApiResponse<null>>(
      "/Tags",
      request
    );

  return response.data;
}

export async function deleteTag(
  id: number
) {
  const response =
    await api.delete<ApiResponse<null>>(
      `/Tags/${id}`
    );

  return response.data;
}