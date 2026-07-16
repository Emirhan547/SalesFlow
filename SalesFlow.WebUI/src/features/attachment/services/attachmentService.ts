import api from "@/api/axios";

import type { ApiResponse } from "@/types/ApiResponse";
import type { PagedResult } from "@/types/PagedResult";

import type { Attachment } from "../types/Attachment";
import type { AttachmentFilterRequest } from "../types/AttachmentFilterRequest";
import type { CreateAttachmentRequest } from "../types/CreateAttachmentRequest";

export async function getAttachments(
  request: AttachmentFilterRequest
) {
  const response =
    await api.get<ApiResponse<PagedResult<Attachment>>>(
      "/Attachments",
      {
        params: request,
      }
    );

  return response.data.data;
}

export async function getAttachmentById(
  id: number
) {
  const response =
    await api.get<ApiResponse<Attachment>>(
      `/Attachments/${id}`
    );

  return response.data.data;
}

export async function createAttachment(
  request: CreateAttachmentRequest
) {
  const formData =
    new FormData();

  formData.append(
    "file",
    request.file
  );

  formData.append(
    "customerId",
    request.customerId.toString()
  );

  const response =
    await api.post<ApiResponse<null>>(
      "/Attachments",
      formData,
      {
        headers: {
          "Content-Type":
            "multipart/form-data",
        },
      }
    );

  return response.data;
}

export async function deleteAttachment(
  id: number
) {
  const response =
    await api.delete<ApiResponse<null>>(
      `/Attachments/${id}`
    );

  return response.data;
}

export async function downloadAttachment(
  id: number
) {
  const response =
    await api.get(
      `/Attachments/download/${id}`,
      {
        responseType: "blob",
      }
    );

  return response.data;
}