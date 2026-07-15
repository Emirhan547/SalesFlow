export interface UpdateCustomerRequest {
  id: number;

  customerType: number;

  companyName?: string;

  contactFirstName: string;

  contactLastName: string;

  email: string;

  phoneNumber: string;

  website?: string;

  taxNumber?: string;

  address?: string;

  description?: string;

  assignedUserId: number | null;
}