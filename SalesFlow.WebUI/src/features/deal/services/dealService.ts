import api from "@/api/axios";

import type { ApiResponse } from "@/types/ApiResponse";
import type { PagedResult } from "@/types/PagedResult";

import type { Deal } from "../types/Deal";
import type { DealFilterRequest } from "../types/DealFilterRequest";
import type { CreateDealRequest } from "../types/CreateDealRequest";
import type { UpdateDealRequest } from "../types/UpdateDealRequest";

export async function getDeals(
  request: DealFilterRequest
) {
  const response =
    await api.get<ApiResponse<PagedResult<Deal>>>(
      "/Deals",
      {
        params: request,
      }
    );

  return response.data.data;
}

export async function getDealById(
  id: number
) {
  const response =
    await api.get<ApiResponse<Deal>>(
      `/Deals/${id}`
    );

  return response.data.data;
}

export async function createDeal(
  request: CreateDealRequest
) {
  const response =
    await api.post<ApiResponse<null>>(
      "/Deals",
      request
    );

  return response.data;
}

export async function updateDeal(
  request: UpdateDealRequest
) {
  const response =
    await api.put<ApiResponse<null>>(
      "/Deals",
      request
    );

  return response.data;
}

export async function deleteDeal(
  id: number
) {
  const response =
    await api.delete<ApiResponse<null>>(
      `/Deals/${id}`
    );

  return response.data;
}

export async function exportDealsExcel() {
  const response =
    await api.get(
      "/Deals/export",
      {
        responseType: "blob",
      }
    );

  return response.data;
}

export async function exportDealsPdf() {
  const response =
    await api.get(
      "/Deals/export/pdf",
      {
        responseType: "blob",
      }
    );

  return response.data;
}