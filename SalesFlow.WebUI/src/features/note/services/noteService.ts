import api from "@/api/axios";

import type { ApiResponse } from "@/types/ApiResponse";
import type { PagedResult } from "@/types/PagedResult";

import type { Note } from "../types/Note";
import type { NoteFilterRequest } from "../types/NoteFilterRequest";
import type { CreateNoteRequest } from "../types/CreateNoteRequest";
import type { UpdateNoteRequest } from "../types/UpdateNoteRequest";

export async function getNotes(
  request: NoteFilterRequest
) {
  const response =
    await api.get<ApiResponse<PagedResult<Note>>>(
      "/Notes",
      {
        params: request,
      }
    );

  return response.data.data;
}

export async function getNoteById(
  id: number
) {
  const response =
    await api.get<ApiResponse<Note>>(
      `/Notes/${id}`
    );

  return response.data.data;
}

export async function createNote(
  request: CreateNoteRequest
) {
  const response =
    await api.post<ApiResponse<null>>(
      "/Notes",
      request
    );

  return response.data;
}

export async function updateNote(
  request: UpdateNoteRequest
) {
  const response =
    await api.put<ApiResponse<null>>(
      "/Notes",
      request
    );

  return response.data;
}

export async function deleteNote(
  id: number
) {
  const response =
    await api.delete<ApiResponse<null>>(
      `/Notes/${id}`
    );

  return response.data;
}