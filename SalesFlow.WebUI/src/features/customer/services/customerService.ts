import api from "@/api/axios";


import type { Customer } from "@/features/customer/types/Customer";
import type { ApiResponse } from "@/types/ApiResponse";
import type { PagedResult } from "@/types/PagedResult";

export async function getCustomers(
    page: number,
    pageSize: number,
    search: string
): Promise<PagedResult<Customer>> {

    const response =
        await api.get<ApiResponse<PagedResult<Customer>>>(
            "/customers",
            {
                params: {
                    page,
                    pageSize,
                    search
                }
            });

    return response.data.data;
}