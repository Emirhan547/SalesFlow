import api from "@/api/axios";
import type { DashboardDto } from "../types/DashboardDto";
import type { ApiResponse } from "@/types/ApiResponse";

export async function getDashboard() {

    const response =
        await api.get<ApiResponse<DashboardDto>>("/dashboard");

    return response.data.data;
}