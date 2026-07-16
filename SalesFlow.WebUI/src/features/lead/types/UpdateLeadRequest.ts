import type { CreateLeadRequest } from "./CreateLeadRequest";

export interface UpdateLeadRequest
  extends CreateLeadRequest {
  id: number;
}
