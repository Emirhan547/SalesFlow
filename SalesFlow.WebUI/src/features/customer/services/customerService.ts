import api from "@/api/axios";

import type { ApiResponse } from "@/types/ApiResponse";
import type { PagedResult } from "@/types/PagedResult";

import type { Customer } from "../types/Customer";
import type { CustomerFilterRequest } from "../types/CustomerFilterRequest";
import type { CreateCustomerRequest } from "../types/CreateCustomerRequest";
import type { UpdateCustomerRequest } from "../types/UpdateCustomerRequest";
import type { GenerateFollowUpEmailRequest } from "../types/GenerateFollowUpEmailRequest";

export async function getCustomers(
  request: CustomerFilterRequest
) {
  const response =
    await api.get<ApiResponse<PagedResult<Customer>>>(
      "/Customers",
      {
        params: request,
      }
    );

  return response.data.data;
}

export async function getCustomerById(
  id: number
) {
  const response =
    await api.get<ApiResponse<Customer>>(
      `/Customers/${id}`
    );

  return response.data.data;
}

export async function createCustomer(
  request: CreateCustomerRequest
) {
  const response =
    await api.post<ApiResponse<null>>(
      "/Customers",
      request
    );

  return response.data;
}

export async function updateCustomer(
  request: UpdateCustomerRequest
) {
  const response =
    await api.put<ApiResponse<null>>(
      "/Customers",
      request
    );

  return response.data;
}

export async function deleteCustomer(
  id: number
) {
  const response =
    await api.delete<ApiResponse<null>>(
      `/Customers/${id}`
    );

  return response.data;
}

export async function exportCustomersExcel() {

  const response =
    await api.get(
      "/Customers/export",
      {
        responseType: "blob",
      }
    );

  return response.data;
}

export async function exportCustomersPdf() {

  const response =
    await api.get(
      "/Customers/export/pdf",
      {
        responseType: "blob",
      }
    );

  return response.data;
}
export async function getCustomerInsights(
  id: number
) {
  const response =
    await api.get<ApiResponse<string>>(
      `/Customers/${id}/ai-insights`
    );

  return response.data.data;
}

export async function generateFollowUpEmail(
  id: number,
  request: GenerateFollowUpEmailRequest
) {
  const response =
    await api.post<ApiResponse<string>>(
      `/Customers/${id}/generate-followup-email`,
      request
    );

  return response.data.data;
}