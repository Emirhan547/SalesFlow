import api from "@/api/axios";
import type { LeadScore } from "../types/LeadScore";
import type { ApiResponse } from "@/types/ApiResponse";
import type { PagedResult } from "@/types/PagedResult";
import type { Lead } from "../types/Lead";
import type { LeadFilterRequest } from "../types/LeadFilterRequest";
import type { CreateLeadRequest } from "../types/CreateLeadRequest";
import type { UpdateLeadRequest } from "../types/UpdateLeadRequest";
import type { ConvertLeadRequest } from "../types/ConvertLeadRequest";

export async function getLeads(
  request: LeadFilterRequest
) {

  const response =
    await api.get<ApiResponse<PagedResult<Lead>>>(
      "/Leads",
      {
        params: request,
      }
    );

  return response.data.data;

}

export async function getLeadById(
  id: number
) {

  const response =
    await api.get<ApiResponse<Lead>>(
      `/Leads/${id}`
    );

  return response.data.data;

}

export async function createLead(
  request: CreateLeadRequest
) {

  const response =
    await api.post<ApiResponse<null>>(
      "/Leads",
      request
    );

  return response.data;

}

export async function updateLead(
  request: UpdateLeadRequest
) {

  const response =
    await api.put<ApiResponse<null>>(
      "/Leads",
      request
    );

  return response.data;

}

export async function deleteLead(
  id: number
) {

  const response =
    await api.delete<ApiResponse<null>>(
      `/Leads/${id}`
    );

  return response.data;

}

export async function convertLead(
  id: number,
  request: ConvertLeadRequest
) {

  const response =
    await api.post<ApiResponse<null>>(
      `/Leads/${id}/convert`,
      request
    );

  return response.data;

}

export async function exportLeadsExcel() {

  const response =
    await api.get(
      "/Leads/export",
      {
        responseType: "blob",
      }
    );

  return response.data;

}

export async function exportLeadsPdf() {

  const response =
    await api.get(
      "/Leads/export/pdf",
      {
        responseType: "blob",
      }
    );

  return response.data;

}

export async function getLeadScore(
  id: number
) {
  const response =
    await api.get<ApiResponse<LeadScore>>(
      `/Leads/${id}/score`
    );

  return response.data.data;
}
export async function getLeadSummary(
  id: number
) {

  const response =
    await api.get<ApiResponse<string>>(
      `/Leads/${id}/summary`
    );

  return response.data.data;

}