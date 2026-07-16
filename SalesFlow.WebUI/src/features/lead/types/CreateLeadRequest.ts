export interface CreateLeadRequest {
  firstName: string;

  lastName: string;

  companyName?: string;

  email: string;

  phoneNumber: string;

  website?: string;

  address?: string;

  description?: string;

  status: number;

  source: number;

  assignedUserId: number | null;
}